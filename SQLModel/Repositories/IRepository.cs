using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;

namespace SQLModel.Repositories
{
	public interface IGenericRepository<TEntity> : IDisposable
		where TEntity : class
	{
		DbContext _context { get; set; }

		/// <summary>
		/// 新增資料到指定實體
		/// </summary>
		/// <param name="instance"></param>
		void Create(TEntity instance);

		/// <summary>
		/// 更新資料到指定實體
		/// </summary>
		/// <param name="instance"></param>
		void Update(TEntity instance);

		/// <summary>
		/// 更新多筆資料到指定實體
		/// </summary>
		/// <param name="updateList">資料實體</param>
		void UpdateMultiple(List<TEntity> updateList);

		/// <summary>
		/// 由資料實體刪除資料
		/// </summary>
		/// <param name="instance"></param>
		void Delete(TEntity instance);

		/// <summary>
		/// 取得資料
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		TEntity Get(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 根據條件尋找 TEntity
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 取得所有資料
		/// </summary>
		/// <returns></returns>
		IQueryable<TEntity> GetAll();

		/// <summary>
		/// 儲存
		/// </summary>
		void SaveChanges();
	}
}
