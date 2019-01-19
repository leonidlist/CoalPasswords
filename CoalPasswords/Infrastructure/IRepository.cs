using System.Collections.Generic;

namespace CoalPasswords
{
    interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
    }
}
