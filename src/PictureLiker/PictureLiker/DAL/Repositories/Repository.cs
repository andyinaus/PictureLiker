﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PictureLiker.DAL.Entities;

namespace PictureLiker.DAL.Repositories
{   //TODO: Unit Tests
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly PictureLikerContext _dbContext;

        public Repository(PictureLikerContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public Task<T> FirstOrDefaultAsync()
        {
            return _dbContext.Set<T>().FirstOrDefaultAsync();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }


        public async Task<IList<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }

        public IEnumerable<T> TakeAtPage(int page, int numberOfRecordsPerPage, Expression<Func<T, bool>> predicate)
        {
            if (page <= 0) throw new ArgumentException("Invalid page number - must be greater than 0.", nameof(page));

            return _dbContext.Set<T>()
                .Where(predicate)
                .Skip(numberOfRecordsPerPage * (page - 1))
                .Take(numberOfRecordsPerPage)
                .AsEnumerable();
        }

        public IEnumerable<T> FromSql(string query, params object[] parameters)
        {
            return _dbContext.Set<T>().FromSql(query, parameters).AsEnumerable();
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public void Edit(T entity)
        {
            _dbContext.Update(entity);
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).LongCount();
        }
    }
}
