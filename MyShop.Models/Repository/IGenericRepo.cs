using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? perdicate=null,string? include=null);
        T Get(Expression<Func<T, bool>>? perdicate=null, string? include = null);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
