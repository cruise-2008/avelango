using System.Data.Entity;
using Avelango.Models.Abstractions.UnitOfWork;

namespace Avelango.Models.Abstractions.Contracts
{
    public interface IQueryableUnitOfWork : IUnitOfWork, ISql
    {
        DbSet<T> CreateSet<T>() where T : class;
        void Attach<T>(T item) where T : class;
        void SetModified<T>(T item) where T : class;
        void ApplyCurrentValues<T>(T original, T current) where T : class;

    }
}
