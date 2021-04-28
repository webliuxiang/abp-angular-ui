using System.Linq;
using Abp;
using JetBrains.Annotations;
// ReSharper disable once CheckNamespace


namespace System.Collections.Generic
{
    /// <summary>
    /// 集合的扩展方法
    /// </summary>
    public static class AbpCollectionExtensions
    {



        /// <summary>
        ///如果项尚未在集合中，则将其添加到集合中。
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="item">要检查和添加的项</param>
        /// <typeparam name="T">集合中待添加项的泛型类型</typeparam>
        /// <returns>返回添加的项。</returns>
        public static IEnumerable<T> AddIfNotContains<T>([NotNull] this ICollection<T> source, IEnumerable<T> items)
        {
            Check.NotNull(source, nameof(source));

            var addedItems = new List<T>();

            foreach (var item in items)
            {
                if (source.Contains(item))
                {
                    continue;
                }

                source.Add(item);
                addedItems.Add(item);
            }

            return addedItems;
        }

        /// <summary>
        /// 根据给定的谓词将尚未在集合中的项添加到集合中 <paramref name="predicate"/>.
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="predicate">确定项是否已在集合中的条件</param>
        /// <param name="itemFactory">返回项目的工厂</param>
        /// <typeparam name="T">集合中待添加项的泛型类型</typeparam>
        /// <returns>如果添加，返回True;如果不添加，返回False。</returns>
        public static bool AddIfNotContains<T>([NotNull] this ICollection<T> source, [NotNull] Func<T, bool> predicate, [NotNull] Func<T> itemFactory)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));
            Check.NotNull(itemFactory, nameof(itemFactory));

            if (source.Any(predicate))
            {
                return false;
            }

            source.Add(itemFactory());
            return true;
        }

        /// <summary>
        /// 从集合中删除满足给定条件的所有项<paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">集合中待添加项的泛型类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="predicate">删除项的条件</param>
        /// <returns>已移除项目列表</returns>
        public static IList<T> RemoveAll<T>([NotNull] this ICollection<T> source, Func<T, bool> predicate)
        {
            var items = source.Where(predicate).ToList();

            foreach (var item in items)
            {
                source.Remove(item);
            }

            return items;
        }
    }
}
