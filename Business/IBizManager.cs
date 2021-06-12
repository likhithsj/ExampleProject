using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Business
{
    public interface IBizManager<TEntity>
        where TEntity : class
    {
        IList<TEntity> GetAll();
        TEntity GetByID (string id);
        void Add (TEntity entity);
        bool DeleteById (string id);
        void UpdateById(string id, TEntity entity);
    }
}
