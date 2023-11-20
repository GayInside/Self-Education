using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Domain.InfrastructureInterfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllByName(string name);

        T? GetById(long id);

        void Delete(T entity);

        void Add(T entity);

        void UpdateEntity(T entity);

        T? Get(string identicName);
    }
}
