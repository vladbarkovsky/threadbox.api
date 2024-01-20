﻿using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Application.Services
{
    public class DbFileStorage : IFileStorage
    {
        private readonly Infrastructure.Persistence.ApplicationDbContext _dbContext;

        public DbFileStorage(Infrastructure.Persistence.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            var data = await _dbContext.DbFiles
                .AsNoTracking()
                .Where(x => x.Path == filePath)
                .Select(x => x.Data)
                .FirstOrDefaultAsync();

            HttpResponseException.ThrowNotFoundIfNull(data);

            return data;
        }

        public async Task SaveFileAsync(string path, byte[] data)
        {
            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            using var transaction = _dbContext.Database.BeginTransaction();
            _dbContext.DbFiles.Add(dbFile);
            await transaction.CommitAsync();
        }

        public async Task DeleteFileAsync(string path)
        {
            var dbFile = await _dbContext.DbFiles
                .Where(x => x.Path == path)
                .FirstOrDefaultAsync();

            HttpResponseException.ThrowNotFoundIfNull(dbFile);

            using var transaction = _dbContext.Database.BeginTransaction();
            _dbContext.DbFiles.Remove(dbFile);
            await transaction.CommitAsync();
        }
    }
}