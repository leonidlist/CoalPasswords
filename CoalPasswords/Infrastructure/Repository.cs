using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoalPasswords
{
    class Repository<T> : IRepository<T>
    {
        public IList<T> RepositoryContext { get; set; }
        public Repository()
        {
            RepositoryContext = new List<T>();
        }
        public Repository(IList<T> repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public void Add(T entity)
        {
            RepositoryContext.Add(entity);
        }

        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return RepositoryContext;
        }

        public void Remove(T entity)
        {
            RepositoryContext.Remove(entity);
        }
    }
}
