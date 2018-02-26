using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Avelango.Models.Abstractions.Contracts;
using Avelango.Models.Abstractions.Specification;
using Avelango.Models.Abstractions.UnitOfWork;

namespace Avelango.DbOrm.UnitOfWork
{
    public class Repository<T> : IRepository<T> where T : class {

        private readonly IQueryableUnitOfWork _unitOfWork;
        

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            //if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            _unitOfWork = unitOfWork;
        }


        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }


        public virtual void Add(T item)
        {

            if (item != (T) null)
            {
                GetSet().Add(item); 
            }
        }

        
        public virtual void Remove(T item)
        {
            if (item == (T) null) return;
            _unitOfWork.Attach(item);
            GetSet().Remove(item);
        }


        public virtual void TrackItem(T item)
        {
            if (item != (T) null)
            {
                _unitOfWork.Attach<T>(item);
            }
        }


        public virtual void Modify(T item)
        {
            if (item != (T) null)
            {
                _unitOfWork.SetModified(item);
            }
        }


        public virtual T Get(int id)
        {
            if (id != 0)
                return GetSet().Find(id);
            else
                return null;
        }


        public virtual IEnumerable<T> GetAll()
        {
            return GetSet();
        }
 

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return GetSet().Where(filter);
        }


        public virtual IEnumerable<T> AllMatching(ISpecification<T> specification)
        {
            return GetSet().Where(specification.SatisfiedBy());
        }


        public virtual IEnumerable<T> GetPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<T, TKProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression).Skip(pageCount*pageIndex).Take(pageCount);
            }
                return set.OrderByDescending(orderByExpression).Skip(pageCount*pageIndex).Take(pageCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetFiltered(Expression<Func<T, bool>> filter)
        {
            return GetSet().Where(filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persisted"></param>
        /// <param name="current"></param>
        public virtual void Merge(T persisted, T current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }



        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
          
            return GetSet().FirstOrDefault(filter);
        }


        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> filter)
        {
            return _unitOfWork.CreateSet<T>().SingleOrDefault(filter);
        }


        public virtual T GetSingleOrDefault(ISpecification<T> specification)
        {
            return _unitOfWork.CreateSet<T>().SingleOrDefault(specification.SatisfiedBy());           
        }

        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _unitOfWork.CreateSet<T>().Where(filter);
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.SingleOrDefault();
            
        }

        public virtual long Count()
        {
            return GetSet().LongCount();
        }

        public virtual long Count(Expression<Func<T, bool>> predicate)
        {
            return GetSet().Where(predicate).LongCount();
        }


        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        private IDbSet<T> GetSet()
        {
            return _unitOfWork.CreateSet<T>();
        }

    }
}
