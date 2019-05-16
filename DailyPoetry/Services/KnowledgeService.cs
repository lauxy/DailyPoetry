using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DailyPoetry.Models.KnowledgeModels;
using Microsoft.EntityFrameworkCore;
using Windows.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DailyPoetry.Services
{
    using PoetryIntermediateType = IntermediateResult<PoetryItem, SimplifiedPoetryItem>;
    using WriterIntermediateType = IntermediateResult<WriterItem, SimplifiedWriterItem>;
    using CategoryIntermediateType = IntermediateResult<CategoryItem, SimplifiedCategoryItem>;

    /// <summary>
    /// 诗词、作者等固定内容的服务
    /// 1. 使用时，使用 using(knowledgeService.Entry) { ... }
    /// 2. 所有查询结果 ToList 返回 Simplified, ToListFull 返回完整的
    ///    Models 中定义了 Simplified 和 原始对象 之间的关系
    /// 3. 使用 Simplified 的对象是因为假定不需要同时获取多个 原始对象，节约内存
    /// </summary>
    public class KnowledgeService : IDisposable
    {
        public KnowledgeContext _knowledgeContext;

        /// <summary>
        /// 初始化数据库，在 app.xaml.cs 中调用
        /// </summary>
        /// <returns></returns>
        public static async Task InitDatabase()
        {
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("db.sqlite3") as StorageFile;
            if (dbFile == null)
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var originalDbFileUri = new Uri("ms-appx:///Assets/db.sqlite3");
                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);

                if (originalDbFile != null)
                {
                    await originalDbFile.CopyAsync(localFolder, "db.sqlite3",
                        NameCollisionOption.ReplaceExisting);
                }
            }
        }

        /// <summary>
        /// 进入资源，KnowledgeService 建议在 using 语句块中使用
        /// </summary>
        /// <returns>return this</returns>
        public KnowledgeService Entry()
        {
            if (_knowledgeContext != null)
                throw new InvalidOperationException("请注意释放资源，使用 using 或者手动调用 Dispose");
            _knowledgeContext = new KnowledgeContext();
            return this;
        }

        /// <summary>
        /// 释放 Context
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:dbService.KnowledgeService"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:dbService.KnowledgeService"/> in an unusable state.
        /// After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:dbService.KnowledgeService"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:dbService.KnowledgeService"/> was occupying.</remarks>
        public void Dispose()
        {
            _knowledgeContext.Dispose();
            _knowledgeContext = null;
        }

        // 根据Id获取对象的一族函数

        public SimplifiedPoetryItem GetSimplifiedPoetryItemById(int id)
        {
            var poetryItem = GetPoetryItemById(id);
            if (poetryItem == null)
                return null;
            return poetryItem.ToSimplified(); ;
        }

        public PoetryItem GetPoetryItemById(int id)
        {
            var poetryItem = _knowledgeContext.PoetryItems
                .Where(pItem => pItem.Id == id);
            if (poetryItem.Any())
                return poetryItem.First();
            return null;
        }

        public SimplifiedWriterItem GetSimplifiedWriterItemById(int id)
        {
            var writerItem = GetWriterItemById(id);
            if (writerItem == null)
                return null;
            return writerItem.ToSimplified();
        }

        public WriterItem GetWriterItemById(int id)
        {
            var writerItem = _knowledgeContext.WriterItems
                .Where(wItem => wItem.Id == id);
            if (writerItem.Any())
                return writerItem.First();
            return null;
        }

        public SimplifiedCategoryItem GetSimplifiedCategoryItem(int id)
        {
            var categoryItem = GetdCategoryItem(id);
            if (categoryItem == null)
                return null;
            return categoryItem.ToSimplified();
        }

        public CategoryItem GetdCategoryItem(int id)
        {
            var categoryItem = _knowledgeContext.CategoryItems
                .Where(wItem => wItem.Id == id);
            if (categoryItem.Any())
                return categoryItem.First();
            return null;
        }

        // 获取所有对象的一族函数

        public PoetryIntermediateType GetAllSimplifiedPoetryItems()
        {
            return new PoetryIntermediateType(_knowledgeContext.PoetryItems);
        }

        public WriterIntermediateType GetAllSimplifiedWriterItems()
        {
            return new WriterIntermediateType(_knowledgeContext.WriterItems);
        }

        public CategoryIntermediateType GetAllSimplifiedCategoryItems()
        {
            return new CategoryIntermediateType(_knowledgeContext.CategoryItems);
        }

        // 查询的一族函数

        /// <summary>
        /// 根据名字获取诗词
        /// </summary>
        /// <returns>The poetry items by name.</returns>
        /// <param name="query">Query.</param>
        public PoetryIntermediateType GetPoetryItemsByName(
            string query, bool exactMode = false)
        {
            return GetPoetryItemsByName(
                GetAllSimplifiedPoetryItems(), query, exactMode);
        }

        /// <summary>
        /// 根据名字获取诗词, 可以从之前的查询结果开始
        /// </summary>
        /// <returns>The poetry items by name.</returns>
        /// <param name="source">Source.</param>
        /// <param name="query">Query.</param>
        /// <param name="exactMode">If set to <c>true</c> exact mode.</param>
        public PoetryIntermediateType GetPoetryItemsByName(
            PoetryIntermediateType source, string query, bool exactMode = false)
        {
            return source.Where(
                GetWhereFunc<PoetryItem>("Name", query, exactMode));
        }

        public PoetryIntermediateType GetPoetryItemsByWriter(
            string query, bool exactMode = false)
        {
            return GetPoetryItemsByWriter(GetAllSimplifiedPoetryItems(),
                query, exactMode);
        }

        public PoetryIntermediateType GetPoetryItemsByWriter(
            PoetryIntermediateType source, string query, bool exactMode = false)
        {
            return source.Where(
                GetWhereFunc<PoetryItem>("AuthorName", query, exactMode));
        }

        public PoetryIntermediateType GetPoetryItemsByContent(
            string query, bool exactMode = false)
        {
            return GetPoetryItemsByContent(GetAllSimplifiedPoetryItems(),
                query, exactMode);
        }

        public PoetryIntermediateType GetPoetryItemsByContent(
            PoetryIntermediateType source, string query, bool exactMode = false)
        {
            return source.Where(
                GetWhereFunc<PoetryItem>("Content", query, exactMode));
        }

        /// 增加删除
        
        public void AddFavoriteItem(int PoetryId)
        {
            var favoriteItem = new FavoriteItem { PoetryId = PoetryId };
            _knowledgeContext.FavoriteItems.Add(favoriteItem);
            _knowledgeContext.SaveChanges();
        }

        public void AddRecentViewItem(int PoetryId)
        {
            var recentViewItem = new RecentViewItem { PoetryItemId = PoetryId };
            _knowledgeContext.RecentViewItems.Add(recentViewItem);
            _knowledgeContext.SaveChanges();
        }

        public void DeleteFavoriteItemByPoetryIdItem(int PoetryId)
        {
            var favoriteItem = _knowledgeContext.FavoriteItems.Single(i => i.PoetryId == PoetryId);
            if (favoriteItem == null)
                throw new InvalidOperationException();
            _knowledgeContext.FavoriteItems.Remove(favoriteItem);
            _knowledgeContext.SaveChanges();
        }

        public void DeleteRecentViewItemByPoetryIdItem(int PoetryId)
        {
            var recentViewItem = _knowledgeContext.RecentViewItems.Single(i => i.PoetryItemId == PoetryId);
            if (recentViewItem == null)
                throw new InvalidOperationException();
            _knowledgeContext.RecentViewItems.Remove(recentViewItem);
            _knowledgeContext.SaveChanges();
        }

        public void DeleteFavoriteItemById(int itemId)
        {
            var favoriteItem = _knowledgeContext.FavoriteItems.Find(itemId);
            if (favoriteItem == null)
                throw new InvalidOperationException();
            _knowledgeContext.FavoriteItems.Remove(favoriteItem);
            _knowledgeContext.SaveChanges();
        }
        public void DeleteRecentViewItemById(int itemId)
        {
            var recentViewItem = _knowledgeContext.RecentViewItems.Find(itemId);
            if (recentViewItem == null)
                throw new InvalidOperationException();
            _knowledgeContext.RecentViewItems.Remove(recentViewItem);
            _knowledgeContext.SaveChanges();
        }

        /// <summary>
        /// 分页获取数据
        /// 
        /// </summary>
        /// <returns>每次返回一页</returns>
        /// <param name="source">数据源</param>
        /// <param name="count">页的大小</param>
        public IEnumerable<IntermediateResult<T, ST>> GetDataByPage<T, ST>(
            IntermediateResult<T, ST> source, int count) where T : IDbItem<ST>
        {
            int current = 0;
            return GetNextPageEnumerator();

            IEnumerable<IntermediateResult<T, ST>> GetNextPageEnumerator()
            {
                var data = source.Skip(current).Take(count);
                while (data.Count() == count)
                {
                    yield return data;
                    current += count;
                    data = source.Skip(current).Take(count);
                }
                yield return data;
            }
        }

        /// <summary>
        /// NEW at 20190507
        /// 使用反射会阻止 EF core 生成 SQL 语句，就是会把所有数据取到内存，在内存中进行反射的操作
        /// 应该使用 dynamic
        /// ----
        /// 获取 Where 的查询函数, 需要注意使用反射可能导致性能问题，到时候再说吧
        /// 如果需要解决，可以使用 fasterflect (http://fasterflect.codeplex.com/)
        /// </summary>
        /// <returns>The where func.</returns>
        /// <param name="propertyName">属性名</param>
        /// <param name="query">要查询的字符串</param>
        /// <param name="exactMode">If set to <c>true</c> 全匹配.</param>
        private System.Linq.Expressions.Expression<Func<T, bool>> GetWhereFunc<T>(
            string propertyName, string query, bool exactMode)
        {
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(query, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);

            //Type type = typeof(T);
            //PropertyInfo prop = type.GetProperty(propertyName);
            //if (prop == null)
            //    throw new MissingMemberException();
            //System.Linq.Expressions.Expression<Func<T, bool>> whereFunc = exactMode ?
            //    (whereFunc = (T pItem) =>
            //        (prop.GetValue(pItem) as string) == query) :
            //    (whereFunc = (T pItem) =>
            //        (prop.GetValue(pItem) as string).Contains(query));
            //return whereFunc;
        }
    }

    /// <summary>
    /// KnowledgeService 的中间结果，使用 ToList 提取数据
    /// 只是一个 Agent 用于在最后一步转换为 Simplified
    /// note: 2019-4-22 failed to add thread safe.
    /// </summary>
    public class IntermediateResult<T, ST> where T : IDbItem<ST>
    {
        public IQueryable<T> Data { get; private set; }

        public IntermediateResult(IQueryable<T> source)
        {
            Data = source;
        }

        public List<T> ToListFull()
        {
            return Data.ToList();
        }

        public Task<List<T>> ToListFullAsync()
        {
            return Task.Run(() => Data.ToList());
        }

        public List<ST> ToList()
        {
            return Data
                .Select(t => t.ToSimplified())
                .ToList();
        }

        public Task<List<ST>> ToListAsync()
        {
            return Task.Run(() => Data
                .Select(t => t.ToSimplified())
                .ToList());
        }

        public IntermediateResult<T, ST> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return new IntermediateResult<T, ST>(Data.Where(predicate));
        }

        public IntermediateResult<T, ST> Select(System.Linq.Expressions.Expression<Func<T, T>> predicate)
        {
            return new IntermediateResult<T, ST>(Data.Select(predicate));
        }

        public IntermediateResult<T, ST> Skip(int count)
        {
            return new IntermediateResult<T, ST>(Data.Skip(count));
        }

        public IntermediateResult<T, ST> Take(int count)
        {
            return new IntermediateResult<T, ST>(Data.Take(count));
        }

        public int Count()
        {
            return Data.Count();
        }
    }
}