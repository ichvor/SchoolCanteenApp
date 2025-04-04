using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolCanteenApp.Data
{
    public class GenericRepository<T> where T : class
    {
        private readonly SchoolCanteenContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SchoolCanteenContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public T GetById(int id) => _dbSet.Find(id);
        public void Add(T entity) => _dbSet.Add(entity);
        public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;
        public void Delete(T entity) => _dbSet.Remove(entity);
        public void Save() => _context.SaveChanges();
    }
}