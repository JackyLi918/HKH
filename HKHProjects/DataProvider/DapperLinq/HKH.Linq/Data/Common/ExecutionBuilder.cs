﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using HKH.Data.Linq;

namespace HKH.Linq.Data.Common
{
    /// <summary>
    /// Builds an execution plan for a query expression
    /// </summary>
    public class ExecutionBuilder : DbExpressionVisitor
    {
        private QueryPolicy policy;
        private QueryLinguist linguist;
        private Expression executor;
        private Scope scope;
        private bool isTop = true;
        private MemberInfo receivingMember;
        private int nReaders = 0;
        private List<ParameterExpression> variables = new List<ParameterExpression>();
        private List<Expression> initializers = new List<Expression>();
        private Dictionary<string, Expression> variableMap = new Dictionary<string, Expression>();

        private ExecutionBuilder(QueryLinguist linguist, QueryPolicy policy, Expression executor)
        {
            this.linguist = linguist;
            this.policy = policy;
            this.executor = executor;
        }

        public static Expression Build(QueryLinguist linguist, QueryPolicy policy, Expression expression, Expression provider)
        {
            var executor = Expression.Parameter(typeof(QueryExecutor), "executor");
            var builder = new ExecutionBuilder(linguist, policy, executor);
            builder.variables.Add(executor);
            builder.initializers.Add(Expression.Call(Expression.Convert(provider, typeof(ICreateExecutor)), nameof(ICreateExecutor.CreateExecutor), null, null));
            var result = builder.Build(expression);
            return result;
        }

        private Expression Build(Expression expression)
        {
            expression = this.Visit(expression);
            expression = this.AddVariables(expression);
            return expression;
        }

        private Expression AddVariables(Expression expression)
        {
            // add variable assignments up front
            if (this.variables.Count > 0)
            {
                List<Expression> exprs = new List<Expression>();
                for (int i = 0, n = this.variables.Count; i < n; i++)
                {
                    exprs.Add(MakeAssign(this.variables[i], this.initializers[i]));
                }
                exprs.Add(expression);
                Expression sequence = MakeSequence(exprs);  // yields last expression value

                // use invoke/lambda to create variables via parameters in scope
                Expression[] nulls = this.variables.Select(v => Expression.Constant(null, v.Type)).ToArray();
                expression = Expression.Invoke(Expression.Lambda(sequence, this.variables.ToArray()), nulls);
            }

            return expression;
        }

        private static Expression MakeSequence(IList<Expression> expressions)
        {
            Expression last = expressions[expressions.Count - 1];
            expressions = expressions.Select(e => e.Type.IsValueType ? Expression.Convert(e, typeof(object)) : e).ToList();
            return Expression.Convert(Expression.Call(typeof(ExecutionBuilder), nameof(ExecutionBuilder.Sequence), null, Expression.NewArrayInit(typeof(object), expressions)), last.Type);
        }

        public static object Sequence(params object[] values)
        {
            return values[values.Length - 1];
        }

        public static IEnumerable<R> Batch<T, R>(IEnumerable<T> items, Func<T, R> selector, bool stream)
        {
            var result = items.Select(selector);
            if (!stream)
            {
                return result.ToList();
            }
            else
            {
                return new EnumerateOnce<R>(result);
            }
        }

        private static Expression MakeAssign(ParameterExpression variable, Expression value)
        {
            return Expression.Call(typeof(ExecutionBuilder), nameof(ExecutionBuilder.Assign), new Type[] { variable.Type }, variable, value);
        }

        public static T Assign<T>(ref T variable, T value)
        {
            variable = value;
            return value;
        }

        private Expression BuildInner(Expression expression)
        {
            var eb = new ExecutionBuilder(this.linguist, this.policy, this.executor);
            eb.scope = this.scope;
            eb.receivingMember = this.receivingMember;
            eb.nReaders = this.nReaders;
            eb.nLookup = this.nLookup;
            eb.variableMap = this.variableMap;
            return eb.Build(expression);
        }

        protected override MemberBinding VisitBinding(MemberBinding binding)
        {
            var save = this.receivingMember;
            this.receivingMember = binding.Member;
            var result = base.VisitBinding(binding);
            this.receivingMember = save;
            return result;
        }

        int nLookup = 0;

        private Expression MakeJoinKey(IList<Expression> key)
        {
            if (key.Count == 1)
            {
                return key[0];
            }
            else
            {
                return Expression.New(
                    typeof(CompoundKey).GetConstructors()[0],
                    Expression.NewArrayInit(typeof(object), key.Select(k => (Expression)Expression.Convert(k, typeof(object))))
                    );
            }
        }

        protected override Expression VisitClientJoin(ClientJoinExpression join)
        {
            // convert client join into a up-front lookup table builder & replace client-join in tree with lookup accessor

            // 1) lookup = query.Select(e => new KVP(key: inner, value: e)).ToLookup(kvp => kvp.Key, kvp => kvp.Value)
            Expression innerKey = MakeJoinKey(join.InnerKey);
            Expression outerKey = MakeJoinKey(join.OuterKey);

            ConstructorInfo kvpConstructor = typeof(KeyValuePair<,>).MakeGenericType(innerKey.Type, join.Projection.Projector.Type).GetConstructor(new Type[] { innerKey.Type, join.Projection.Projector.Type });
            Expression constructKVPair = Expression.New(kvpConstructor, innerKey, join.Projection.Projector);
            ProjectionExpression newProjection = new ProjectionExpression(join.Projection.Select, constructKVPair);

            int iLookup = ++nLookup;
            Expression execution = this.ExecuteProjection(newProjection, okayToDefer: false, isTopLevel: false);

            ParameterExpression kvp = Expression.Parameter(constructKVPair.Type, "kvp");

            // filter out nulls
            if (join.Projection.Projector.NodeType == (ExpressionType)DbExpressionType.OuterJoined)
            {
                LambdaExpression pred = Expression.Lambda(
                    Expression.PropertyOrField(kvp, "Value").NotEqual(TypeHelper.GetNullConstant(join.Projection.Projector.Type)),
                    kvp
                    );
                execution = Expression.Call(typeof(Enumerable), nameof(Enumerable.Where), new Type[] { kvp.Type }, execution, pred);
            }

            // make lookup
            LambdaExpression keySelector = Expression.Lambda(Expression.PropertyOrField(kvp, "Key"), kvp);
            LambdaExpression elementSelector = Expression.Lambda(Expression.PropertyOrField(kvp, "Value"), kvp);
            Expression toLookup = Expression.Call(typeof(Enumerable), nameof(Enumerable.ToLookup), new Type[] { kvp.Type, outerKey.Type, join.Projection.Projector.Type }, execution, keySelector, elementSelector);

            // 2) agg(lookup[outer])
            ParameterExpression lookup = Expression.Parameter(toLookup.Type, "lookup" + iLookup);
            PropertyInfo property = lookup.Type.GetProperty("Item");
            Expression access = Expression.Call(lookup, property.GetGetMethod(), this.Visit(outerKey));
            if (join.Projection.Aggregator != null)
            {
                // apply aggregator
                access = DbExpressionReplacer.Replace(join.Projection.Aggregator.Body, join.Projection.Aggregator.Parameters[0], access);
            }

            this.variables.Add(lookup);
            this.initializers.Add(toLookup);

            return access;
        }

        protected override Expression VisitProjection(ProjectionExpression projection)
        {
            if (this.isTop)
            {
                this.isTop = false;
                return this.ExecuteProjection(projection, okayToDefer: this.scope != null, isTopLevel: true);
            }
            else
            {
                return this.BuildInner(projection);
            }
        }

        protected virtual Expression Parameterize(Expression expression)
        {
            if (this.variableMap.Count > 0)
            {
                expression = VariableSubstitutor.Substitute(this.variableMap, expression);
            }
            return this.linguist.Parameterize(expression);
        }

        private Expression ExecuteProjection(ProjectionExpression projection, bool okayToDefer, bool isTopLevel)
        {
            // parameterize query
            projection = (ProjectionExpression)this.Parameterize(projection);

            if (this.scope != null)
            {
                // also convert references to outer alias to named values!  these become SQL parameters too
                projection = (ProjectionExpression)OuterParameterizer.Parameterize(this.scope.Alias, projection);
            }

            string commandText = this.linguist.Format(projection.Select);
            ReadOnlyCollection<NamedValueExpression> namedValues = NamedValueGatherer.Gather(projection.Select);
            QueryCommand command = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
            Expression[] values = namedValues.Select(v => Expression.Convert(this.Visit(v.Value), typeof(object))).ToArray();

            string methExecute = okayToDefer
                ? nameof(QueryExecutor.ExecuteDeferred)
                : nameof(QueryExecutor.Execute);

            var plan = Expression.Call(this.executor, methExecute, new Type[] { projection.Projector.Type },
                Expression.Constant(command),
                Expression.NewArrayInit(typeof(object), values)
                );
            return plan;
        }

        private Expression ExecuteProjection(ProjectionExpression projection, bool okayToDefer, QueryCommand command, Expression[] values, bool isTopLevel)
        {
            okayToDefer &= (this.receivingMember != null && this.policy.IsDeferLoaded(this.receivingMember));

            var saveScope = this.scope;
            this.scope = new Scope(this.scope, projection.Select.Alias, projection.Select.Columns);
            this.scope = saveScope;

            var entity = EntityFinder.Find(projection.Projector);

            string methExecute = okayToDefer
                ? nameof(QueryExecutor.ExecuteDeferred)
                : nameof(QueryExecutor.Execute);

            // call low-level execute directly on supplied DbQueryProvider
            Expression result = Expression.Call(this.executor, methExecute, new Type[] { projection.Projector.Type },
                Expression.Constant(command),
                Expression.NewArrayInit(typeof(object), values)
                );

            if (projection.Aggregator != null)
            {
                // apply aggregator
                result = DbExpressionReplacer.Replace(projection.Aggregator.Body, projection.Aggregator.Parameters[0], result);
            }

            return result;
        }

        protected override Expression VisitBatch(BatchExpression batch)
        {
            if (this.linguist.Language.AllowsMultipleCommands || !IsMultipleCommands(batch.Operation.Body as CommandExpression))
            {
                return this.BuildExecuteBatch(batch);
            }
            else
            {
                var source = this.Visit(batch.Input);
                var op = this.Visit(batch.Operation.Body);
                var fn = Expression.Lambda(op, batch.Operation.Parameters[1]);
                return Expression.Call(typeof(ExecutionBuilder), nameof(ExecutionBuilder.Batch), new Type[] { TypeHelper.GetElementType(source.Type), batch.Operation.Body.Type }, source, fn, batch.Stream);
            }
        }

        protected virtual Expression BuildExecuteBatch(BatchExpression batch)
        {
            // parameterize query
            Expression operation = this.Parameterize(batch.Operation.Body);

            string commandText = this.linguist.Format(operation);
            var namedValues = NamedValueGatherer.Gather(operation);
            QueryCommand command = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
            Expression[] values = namedValues.Select(v => Expression.Convert(this.Visit(v.Value), typeof(object))).ToArray();

            Expression paramSets = Expression.Call(typeof(Enumerable), nameof(Enumerable.Select), new Type[] { batch.Operation.Parameters[1].Type, typeof(object[]) },
                batch.Input,
                Expression.Lambda(Expression.NewArrayInit(typeof(object), values), new[] { batch.Operation.Parameters[1] })
                );

            Expression plan = null;

            ProjectionExpression projection = ProjectionFinder.FindProjection(operation);
            if (projection != null)
            {
                var saveScope = this.scope;
                this.scope = new Scope(this.scope, projection.Select.Alias, projection.Select.Columns);
                this.scope = saveScope;

                var entity = EntityFinder.Find(projection.Projector);
                command = new QueryCommand(command.CommandText, command.Parameters);

                plan = Expression.Call(this.executor, nameof(QueryExecutor.ExecuteBatch), new Type[] { projection.Projector.Type },
                    Expression.Constant(command),
                    paramSets,
                    batch.BatchSize,
                    batch.Stream
                    );
            }
            else
            {
                plan = Expression.Call(this.executor, nameof(QueryExecutor.ExecuteBatch), null,
                    Expression.Constant(command),
                    paramSets,
                    batch.BatchSize,
                    batch.Stream
                    );
            }

            return plan;
        }

        protected override Expression VisitCommand(CommandExpression command)
        {
            if (this.linguist.Language.AllowsMultipleCommands || !IsMultipleCommands(command))
            {
                if (command.NodeType == (ExpressionType)DbExpressionType.PartialUpdate)
                    return this.VisitPartialUpdate((PartialUpdateCommand)command);
                else
                    return this.BuildExecuteCommand(command);
            }
            else
            {
                return base.VisitCommand(command);
            }
        }

        protected virtual bool IsMultipleCommands(CommandExpression command)
        {
            if (command == null)
                return false;
            switch ((DbExpressionType)command.NodeType)
            {
                case DbExpressionType.Insert:
                case DbExpressionType.Delete:
                case DbExpressionType.Update:
                case DbExpressionType.PartialUpdate:
                    return false;
                default:
                    return true;
            }
        }

        //protected override Expression VisitInsert(InsertCommand insert)
        //{
        //    return this.BuildExecuteCommand(insert);
        //}

        //protected override Expression VisitUpdate(UpdateCommand update)
        //{
        //    return this.BuildExecuteCommand(update);
        //}

        protected override Expression VisitPartialUpdate(PartialUpdateCommand update)
        {
            var pcmd = update as PartialUpdateCommand;
            var table = pcmd.Table;
            var where = pcmd.Where;
            var assignments = pcmd.Assignments.Where(ca => (ca.Expression as ConstantExpression).Value != null).ToList();

            pcmd = this.UpdatePartialUpdate(update, table, where, assignments);
            return this.BuildExecuteCommand(pcmd);
        }

        //protected override Expression VisitDelete(DeleteCommand delete)
        //{
        //    return this.BuildExecuteCommand(delete);
        //}

        protected override Expression VisitBlock(BlockCommand block)
        {
            return MakeSequence(this.VisitExpressionList(block.Commands));
        }

        protected override Expression VisitIf(IFCommand ifx)
        {
            var test =
                Expression.Condition(
                    ifx.Check,
                    ifx.IfTrue,
                    ifx.IfFalse != null
                        ? ifx.IfFalse
                        : ifx.IfTrue.Type == typeof(int)
                            ? (Expression)Expression.Property(this.executor, nameof(QueryExecutor.RowsAffected))
                            : (Expression)Expression.Constant(TypeHelper.GetDefault(ifx.IfTrue.Type), ifx.IfTrue.Type)
                            );
            return this.Visit(test);
        }

        protected override Expression VisitFunction(FunctionExpression func)
        {
            if (this.linguist.Language.IsRowsAffectedExpressions(func))
            {
                return Expression.Property(this.executor, nameof(QueryExecutor.RowsAffected));
            }
            return base.VisitFunction(func);
        }

        protected override Expression VisitExists(ExistsExpression exists)
        {
            // how did we get here? Translate exists into count query
            var colType = this.linguist.Language.TypeSystem.GetColumnType(typeof(int));
            var newSelect = exists.Select.SetColumns(
                new[] { new ColumnDeclaration("value", null, new AggregateExpression(typeof(int), "Count", null, false), colType) }
                );

            var projection =
                new ProjectionExpression(
                    newSelect,
                    new ColumnExpression(typeof(int), colType, newSelect.Alias, "value", null),
                    Aggregator.GetAggregator(typeof(int), typeof(IEnumerable<int>))
                    );

            var expression = projection.GreaterThan(Expression.Constant(0));

            return this.Visit(expression);
        }

        protected override Expression VisitDeclaration(DeclarationCommand decl)
        {
            if (decl.Source != null)
            {
                // make query that returns all these declared values as an object[]
                var projection = new ProjectionExpression(
                    decl.Source,
                    Expression.NewArrayInit(
                        typeof(object),
                        decl.Variables.Select(v => v.Expression.Type.IsValueType
                            ? Expression.Convert(v.Expression, typeof(object))
                            : v.Expression).ToArray()
                        ),
                    Aggregator.GetAggregator(typeof(object[]), typeof(IEnumerable<object[]>))
                    );

                // create execution variable to hold the array of declared variables
                var vars = Expression.Parameter(typeof(object[]), "vars");
                this.variables.Add(vars);
                this.initializers.Add(Expression.Constant(null, typeof(object[])));

                // create subsitution for each variable (so it will find the variable value in the new vars array)
                for (int i = 0, n = decl.Variables.Count; i < n; i++)
                {
                    var v = decl.Variables[i];
                    NamedValueExpression nv = new NamedValueExpression(
                        v.Name, v.QueryType,
                        Expression.Convert(Expression.ArrayIndex(vars, Expression.Constant(i)), v.Expression.Type)
                        );
                    this.variableMap.Add(v.Name, nv);
                }

                // make sure the execution of the select stuffs the results into the new vars array
                return MakeAssign(vars, this.Visit(projection));
            }

            // probably bad if we get here since we must not allow mulitple commands
            throw new InvalidOperationException("Declaration query not allowed for this langauge");
        }

        protected virtual Expression BuildExecuteCommand(CommandExpression command)
        {
            // parameterize query
            var expression = this.Parameterize(command);

            string commandText = this.linguist.Format(expression);
            ReadOnlyCollection<NamedValueExpression> namedValues = NamedValueGatherer.Gather(expression);
            QueryCommand qc = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
            Expression[] values = namedValues.Select(v => Expression.Convert(this.Visit(v.Value), typeof(object))).ToArray();

            ProjectionExpression projection = ProjectionFinder.FindProjection(expression);
            if (projection != null)
            {
                return this.ExecuteProjection(projection, false, qc, values, isTopLevel: true);
            }

            var plan = Expression.Call(this.executor, nameof(QueryExecutor.ExecuteCommand), null,
                Expression.Constant(qc),
                Expression.NewArrayInit(typeof(object), values)
                );

            return plan;
        }

        protected override Expression VisitEntity(EntityExpression entity)
        {
            return this.Visit(entity.Expression);
        }

        //protected override Expression VisitOuterJoined(OuterJoinedExpression outer)
        //{
        //    Expression expr = this.Visit(outer.Expression);
        //    ColumnExpression column = (ColumnExpression)outer.Test;
        //    ParameterExpression reader;
        //    int iOrdinal;
        //    if (this.scope.TryGetValue(column, out reader, out iOrdinal))
        //    {
        //        return Expression.Condition(
        //            Expression.Call(reader, "IsDbNull", null, Expression.Constant(iOrdinal)),
        //            Expression.Constant(TypeHelper.GetDefault(outer.Type), outer.Type),
        //            expr
        //            );
        //    }
        //    return expr;
        //}

        //protected override Expression VisitColumn(ColumnExpression column)
        //{
        //    ParameterExpression fieldReader;
        //    int iOrdinal;
        //    if (this.scope != null && this.scope.TryGetValue(column, out fieldReader, out iOrdinal))
        //    {
        //        MethodInfo method = FieldReader.GetReaderMethod(column.Type);
        //        return Expression.Call(fieldReader, method, Expression.Constant(iOrdinal));
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.Fail(string.Format("column not in scope: {0}", column));
        //    }
        //    return column;
        //}

        class Scope
        {
            Scope outer;
            //ParameterExpression fieldReader;
            internal TableAlias Alias { get; private set; }
            Dictionary<string, int> nameMap;

            //internal Scope(Scope outer, ParameterExpression fieldReader, TableAlias alias, IEnumerable<ColumnDeclaration> columns)
            internal Scope(Scope outer, TableAlias alias, IEnumerable<ColumnDeclaration> columns)
            {
                this.outer = outer;
                //this.fieldReader = fieldReader;
                this.Alias = alias;
                this.nameMap = columns.Select((c, i) => new { c, i }).ToDictionary(x => x.c.Name, x => x.i);
            }

            //internal bool TryGetValue(ColumnExpression column, out ParameterExpression fieldReader, out int ordinal)
            //{
            //    for (Scope s = this; s != null; s = s.outer)
            //    {
            //        if (column.Alias == s.Alias && this.nameMap.TryGetValue(column.Name, out ordinal))
            //        {
            //            fieldReader = this.fieldReader;
            //            return true;
            //        }
            //    }
            //    fieldReader = null;
            //    ordinal = 0;
            //    return false;
            //}
        }

        /// <summary>
        /// columns referencing the outer alias are turned into special named-value parameters
        /// </summary>
        class OuterParameterizer : DbExpressionVisitor
        {
            int iParam;
            TableAlias outerAlias;
            Dictionary<ColumnExpression, NamedValueExpression> map = new Dictionary<ColumnExpression, NamedValueExpression>();

            internal static Expression Parameterize(TableAlias outerAlias, Expression expr)
            {
                OuterParameterizer op = new OuterParameterizer();
                op.outerAlias = outerAlias;
                return op.Visit(expr);
            }

            protected override Expression VisitProjection(ProjectionExpression proj)
            {
                SelectExpression select = (SelectExpression)this.Visit(proj.Select);
                return this.UpdateProjection(proj, select, proj.Projector, proj.Aggregator);
            }

            protected override Expression VisitColumn(ColumnExpression column)
            {
                if (column.TableAlias == this.outerAlias)
                {
                    NamedValueExpression nv;
                    if (!this.map.TryGetValue(column, out nv))
                    {
                        nv = new NamedValueExpression("n" + (iParam++), column.QueryType, column);
                        this.map.Add(column, nv);
                    }
                    return nv;
                }
                return column;
            }
        }

        class ColumnGatherer : DbExpressionVisitor
        {
            Dictionary<string, ColumnExpression> columns = new Dictionary<string, ColumnExpression>();

            internal static IEnumerable<ColumnExpression> Gather(Expression expression)
            {
                var gatherer = new ColumnGatherer();
                gatherer.Visit(expression);
                return gatherer.columns.Values;
            }

            protected override Expression VisitColumn(ColumnExpression column)
            {
                if (!this.columns.ContainsKey(column.Name))
                {
                    this.columns.Add(column.Name, column);
                }
                return column;
            }
        }

        class ProjectionFinder : DbExpressionVisitor
        {
            ProjectionExpression found = null;

            internal static ProjectionExpression FindProjection(Expression expression)
            {
                var finder = new ProjectionFinder();
                finder.Visit(expression);
                return finder.found;
            }

            protected override Expression VisitProjection(ProjectionExpression proj)
            {
                this.found = proj;
                return proj;
            }
        }

        class VariableSubstitutor : DbExpressionVisitor
        {
            Dictionary<string, Expression> map;

            private VariableSubstitutor(Dictionary<string, Expression> map)
            {
                this.map = map;
            }

            public static Expression Substitute(Dictionary<string, Expression> map, Expression expression)
            {
                return new VariableSubstitutor(map).Visit(expression);
            }

            protected override Expression VisitVariable(VariableExpression vex)
            {
                Expression sub;
                if (this.map.TryGetValue(vex.Name, out sub))
                {
                    return sub;
                }
                return vex;
            }
        }

        class EntityFinder : DbExpressionVisitor
        {
            MappingEntity entity;

            public static MappingEntity Find(Expression expression)
            {
                var finder = new EntityFinder();
                finder.Visit(expression);
                return finder.entity;
            }

            protected override Expression Visit(Expression exp)
            {
                if (entity == null)
                    return base.Visit(exp);
                return exp;
            }

            protected override Expression VisitEntity(EntityExpression entity)
            {
                if (this.entity == null)
                    this.entity = entity.Entity;
                return entity;
            }

            protected override NewExpression VisitNew(NewExpression nex)
            {
                return nex;
            }

            protected override Expression VisitMemberInit(MemberInitExpression init)
            {
                return init;
            }
        }
    }
}