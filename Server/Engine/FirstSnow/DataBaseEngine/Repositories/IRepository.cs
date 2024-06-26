﻿using DataBaseEngine.Abstract;
using DataBaseEngine.Model;
using FirstSnow.Contract.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataBaseEngine.Repositories
{
    /// <summary>
    /// DB wrapper class' interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Get model list with paging
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="token">token</param>
        /// <returns>PagedResult<T></returns>
        Task<PagedResult<T>> GetAsync(Filter<T> filter, CancellationToken token);

        /// <summary>
        /// Get model list with paging with deleted
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="token">token</param>
        /// <returns>PagedResult<T></returns>
        Task<PagedResult<T>> GetAsyncDeleted(Filter<T> filter, CancellationToken token);

        /// <summary>
        /// Get item of model by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        Task<T> GetAsync(Guid id, CancellationToken token);

        /// <summary>
        /// Get item of model by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        Task<T> GetAsyncDeleted(Guid id, CancellationToken token);

        /// <summary>
        /// add model to db
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="withSave">save after add</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        Task<T> AddAsync(T entity, bool withSave, CancellationToken token);

        /// <summary>
        /// delete model from db
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="withSave">save after add</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        Task<T> DeleteAsync(T entity, bool withSave, CancellationToken token);

        /// <summary>
        /// update model at db
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="withSave">save after add</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity, bool withSave, CancellationToken token);

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}