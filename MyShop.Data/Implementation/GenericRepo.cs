using Microsoft.EntityFrameworkCore;
using MyShop.Models.Repository;
using MyShop.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.Implementation
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepo(AppDbContext context)
        {
            _context = context;
            _dbSet =_context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate=null, string? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            { 
            query=query.Where(perdicate);
            }
            if( include != null)
            {
                foreach (var item in include.Split(new char []{','},StringSplitOptions.RemoveEmptyEntries))
               query=query.Include(include);
            }
                return query.ToList();
        }

        public T Get(Expression<Func<T, bool>>? perdicate=null, string? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (include != null)
            {
                foreach (var item in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(include);
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
           _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
    
}
