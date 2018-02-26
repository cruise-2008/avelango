using System;

namespace Avelango.Models.Abstractions.Db
{
    public interface IEvents
    {
        void Save(Guid userPk, string name, string decrtiption);
    }
}
