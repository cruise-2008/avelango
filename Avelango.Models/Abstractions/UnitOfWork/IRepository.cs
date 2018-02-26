using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Avelango.Models.Abstractions.Specification;

namespace Avelango.Models.Abstractions.UnitOfWork
{
    public interface IRepository<T> : IDisposable where T : class {

        IUnitOfWork UnitOfWork { get; }

        void Add(T item);

        void Remove(T item);

        void Modify(T item);

        void TrackItem(T item);

        void Merge(T persisted, T current);

        T Get(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> AllMatching(ISpecification<T> specification);

        IEnumerable<T> GetPaged<Property>(int pageIndex, int pageCount, Expression<Func<T, Property>> orderByExpression, bool ascending);

        IEnumerable<T> GetFiltered(Expression<Func<T, bool>> filter);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        T GetSingleOrDefault(Expression<Func<T, bool>> filter);

        T GetSingleOrDefault(ISpecification<T> specification);

        T GetSingleOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter);

        long Count();

        long Count(Expression<Func<T, bool>> filter);
    }
}
