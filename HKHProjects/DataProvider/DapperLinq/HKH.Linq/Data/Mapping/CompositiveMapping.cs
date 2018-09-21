using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HKH.Linq.Data.Common;

namespace HKH.Linq.Data.Mapping
{
    public class CompositiveMapping : ImplicitMapping
    {
        Dictionary<string, MappingEntity> entities = new Dictionary<string, MappingEntity>();
        ReaderWriterLock rwLock = new ReaderWriterLock();

        public CompositiveMapping()
        {
        }

        public override MappingEntity GetEntity(Type elementType, string tableId)
        {
            if (tableId == null)
                tableId = GetTableId(elementType);

            MappingEntity entity;
            rwLock.AcquireReaderLock(Timeout.Infinite);
            if (!entities.TryGetValue(tableId, out entity))
            {
                rwLock.ReleaseReaderLock();
                rwLock.AcquireWriterLock(Timeout.Infinite);
                if (!entities.TryGetValue(tableId, out entity))
                {
                    entity = new CompositiveMappingEntity(elementType, tableId);
                    this.entities.Add(tableId, entity);
                }
                rwLock.ReleaseWriterLock();
            }
            else
            {
                rwLock.ReleaseReaderLock();
            }

            return entity;
        }

        public override string GetTableId(Type type)
        {
            return type.Name;
        }

        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsPrimaryKey;

            return base.IsPrimaryKey(entity, member);
        }

        public override bool IsColumn(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null)
                return !mm.NotMapped;

            return base.IsColumn(entity, member);
        }
        public override bool IsComputed(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsComputed;

            return base.IsComputed(entity, member);
        }

        public override bool IsGenerated(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsGenerated;

            return base.IsGenerated(entity, member);
        }
        public override bool IsReadOnly(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.IsReadOnly;

            return base.IsReadOnly(entity, member);
        }
        public override bool IsMapped(MappingEntity entity, MemberInfo member)
        {
            return IsColumn(entity, member);
        }

        public override string GetTableName(MappingEntity entity)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            if (en.Table != null)
                return en.Table.Name;

            return base.GetTableName(entity);
        }

        public override string GetColumnName(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingEntity en = (CompositiveMappingEntity)entity;
            CompositiveMappingMember mm = en.GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.Name;

            return base.GetColumnName(entity, member);
        }

        public string GetAlias(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingMember mm = ((CompositiveMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.TableAlias;

            return "";
        }
        public override string GetColumnAlias(MappingEntity entity, MemberInfo member)
        {
            CompositiveMappingMember mm = ((CompositiveMappingEntity)entity).GetMappingMember(member.Name);
            if (mm != null && mm.Column != null)
                return mm.Column.Alias;

            return "";
        }
        public override QueryMapper CreateMapper(QueryTranslator translator)
        {
            return new CompositiveMapper(this, translator);
        }

        class CompositiveMappingEntity : MappingEntity
        {
            string entityID;
            Type type;
            Dictionary<string, CompositiveMappingMember> mappingMembers;

            public CompositiveMappingEntity(Type type, string entityID)
            {
                this.entityID = entityID;
                this.type = type;

                CollectMappingMembers();
            }

            public override string TableId
            {
                get { return this.entityID; }
            }

            public override Type ElementType
            {
                get { return this.type; }
            }

            public override Type EntityType
            {
                get { return this.type; }
            }

            internal TableAttribute Table { get; private set; }

            internal CompositiveMappingMember GetMappingMember(string name)
            {
                CompositiveMappingMember mm = null;
                this.mappingMembers.TryGetValue(name, out mm);
                return mm;
            }

            private void CollectMappingMembers()
            {
                Table = Attribute.GetCustomAttribute(EntityType, typeof(TableAttribute)) as TableAttribute;

                mappingMembers = new Dictionary<string, CompositiveMappingMember>();
                foreach (MemberInfo m in EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    CreateMappingMember(m);
                }
                foreach (MemberInfo m in EntityType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    CreateMappingMember(m);
                }
            }

            private void CreateMappingMember(MemberInfo m)
            {
                if (m.CustomAttributes.Count() > 0)
                {
                    foreach (MemberAttribute attr in Attribute.GetCustomAttributes(m, typeof(MemberAttribute)) as MemberAttribute[])
                    {
                        if (mappingMembers.ContainsKey(m.Name))
                            throw new InvalidOperationException(string.Format("AttributeMapping: more than one mapping attribute specified for member '{0}' on type '{1}'", m.Name, EntityType.Name));

                        if (string.IsNullOrEmpty(attr.Member))
                            attr.Member = m.Name;

                        if (attr is ColumnAttribute)
                        {
                            var colAttr = attr as ColumnAttribute;
                            if (string.IsNullOrEmpty(colAttr.Name))
                                colAttr.Name = m.Name;
                            if (colAttr.TableAlias == null)
                                colAttr.TableAlias = "";
                            if (string.IsNullOrEmpty(colAttr.Alias) && colAttr.Name != m.Name)
                                colAttr.Alias = m.Name;
                        }

                        mappingMembers.Add(m.Name, new CompositiveMappingMember(m, attr));
                    }
                }
            }
        }

        internal class CompositiveMappingMember
        {
            MemberInfo member;
            MemberAttribute attribute;

            internal CompositiveMappingMember(MemberInfo member, MemberAttribute attribute)
            {
                this.member = member;
                this.attribute = attribute;
            }

            internal MemberInfo Member
            {
                get { return this.member; }
            }

            internal bool NotMapped
            {
                get { return this.attribute is NotMappedAttribute; }
            }

            internal ColumnAttribute Column
            {
                get { return this.attribute as ColumnAttribute; }
            }

            internal AssociationAttribute Association
            {
                get { return this.attribute as AssociationAttribute; }
            }
        }

        public class CompositiveMapper : BasicMapper
        {
            public CompositiveMapper(BasicMapping mapping, QueryTranslator translator)
                : base(mapping, translator)
            {
            }

            public override ProjectionExpression GetQueryExpression(MappingEntity entity)
            {
                CompositiveMappingEntity mme = (CompositiveMappingEntity)entity;
                if (mme.Table == null || string.IsNullOrEmpty(mme.Table.View))
                {
                    var tableAlias = new TableAlias();
                    var selectAlias = new TableAlias();
                    var columns = new List<ColumnDeclaration>();
                    var aliases = new Dictionary<string, TableAlias>();

                    var table = new TableExpression(tableAlias, entity, (this.Mapping as BasicMapping).GetTableName(entity));

                    this.GetColumns(entity, aliases, columns);
                    SelectExpression root = new SelectExpression(new TableAlias(), columns, table, null);

                    Expression projector = this.GetEntityExpression(table, entity);
                    var pc = ColumnProjector.ProjectColumns(this.Translator.Linguist.Language, projector, null, selectAlias, tableAlias);

                    var proj = new ProjectionExpression(
                        new SelectExpression(selectAlias, pc.Columns, table, null),
                        pc.Projector
                        );

                    return (ProjectionExpression)this.Translator.Police.ApplyPolicy(proj, entity.ElementType);
                }
                else
                {
                    var tableAlias = new TableAlias();
                    var selectAlias = new TableAlias();
                    var columns = new List<ColumnDeclaration>();
                    var aliases = new Dictionary<string, TableAlias>();

                    var table = new TableExpression(tableAlias, entity, mme.Table.View);
                    ColumnDeclaration cd = new ColumnDeclaration("*", "", new AllColumnExpression(tableAlias), new DbQueryType(SqlDbType.Variant, false, 0, 0, 0));
                    columns.Add(cd);

                    SelectExpression root = new SelectExpression(new TableAlias(), columns, table, null);
                    Expression projector = this.GetEntityExpression(table, entity);
                    //var pc = ColumnProjector.ProjectColumns(this.Translator.Linguist.Language, projector, null, selectAlias, tableAlias);

                    var proj = new ProjectionExpression(
                        new SelectExpression(selectAlias, columns, table, null),
                        projector
                        );

                    return (ProjectionExpression)this.Translator.Police.ApplyPolicy(proj, entity.ElementType);
                }
            }
            private void GetColumns(MappingEntity entity, Dictionary<string, TableAlias> aliases, List<ColumnDeclaration> columns)
            {
                CompositiveMapping mapping = Mapping as CompositiveMapping;
                foreach (MemberInfo mi in mapping.GetMappedMembers(entity))
                {
                    if (!mapping.IsAssociationRelationship(entity, mi))
                    {
                        if (mapping.IsColumn(entity, mi))
                        {
                            string name = mapping.GetColumnName(entity, mi);
                            string aliasName = mapping.GetAlias(entity, mi);
                            TableAlias alias;
                            aliases.TryGetValue(aliasName, out alias);
                            var colType = this.GetColumnType(entity, mi);
                            string colAlias = mapping.GetColumnAlias(entity, mi);
                            ColumnExpression ce = new ColumnExpression(TypeHelper.GetMemberType(mi), colType, alias, name, new ColumnAlias(colAlias));
                            ColumnDeclaration cd = new ColumnDeclaration(name, colAlias, ce, colType);
                            columns.Add(cd);
                        }
                    }
                }
            }
        }
    }
}