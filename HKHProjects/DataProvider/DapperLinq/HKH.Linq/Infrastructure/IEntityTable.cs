/*******************************************************
 * Filename: IEntityTable.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/24/2017 7:10:20 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Linq
{
    public interface IEntityProvider : IQueryProvider
    {
        IEntityTable<T> GetTable<T>(string tableId);
        IEntityTable GetTable(Type type, string tableId);
        bool CanBeEvaluatedLocally(Expression expression);
        bool CanBeParameter(Expression expression);
    }

    public interface IEntityTable : IQueryable, IUpdatable
    {
        string TableId { get; }
        object GetById(object id);
    }

    public interface IEntityTable<T> : IQueryable<T>, IEntityTable, IUpdatable<T>, IAsyncEnumerable<T>
    {
        new T GetById(object id);
    }

    public interface IUpdatable : IQueryable
    {
    }

    public interface IUpdatable<T> : IUpdatable, IQueryable<T>
    {
    }
}