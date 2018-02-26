using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Avelango.Models.Abstractions.Contracts;

namespace Avelango.DbOrm.UnitOfWork
{
    public class UnitOfWork : AvelangoDbEntities, IQueryableUnitOfWork
    {
        public DbSet<T> CreateSet<T>() where T : class
        {
            return Set<T>();
        }

        public void Attach<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<T>(T item) where T : class
        {
            Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<T>(T original, T current) where T : class
        {
            Entry(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
                base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;
            do
            {
                try
                {
                    SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            } 
            while (saveFailed);

        }

        public void RollbackChanges()
        {
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<T>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
    
    }
}
