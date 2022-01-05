using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;
using StackExchange.Redis;

namespace Sean.Core.Redis
{
    public partial class RedisHelper
    {
        #region Synchronization method
        /// <summary>
        /// Returns the rank of member in the sorted set stored at key, by default with the scores ordered from low to high. The rank (or index) is 0-based, which means that the member with the lowest score has rank 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to get the rank of.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>If member exists in the sorted set, the rank of member; If member does not exist in the sorted set or key does not exist, null</returns>
        public static long? SortedSetRank<T>(string key, T member, Order order = Order.Ascending)
        {
            return Execute(db => db.SortedSetRank(key, member.ToRedisValue(RedisManager.DefaultSerializeType), order));
        }

        /// <summary>
        /// Returns if member exists in the sorted set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to get the rank of.</param>
        /// <returns>True if member exists in the sorted set; False otherwise.</returns>
        public static bool SortedSetExists<T>(string key, T member)
        {
            var rank = Execute(db => db.SortedSetRank(key, member.ToRedisValue(RedisManager.DefaultSerializeType)));
            return rank.HasValue && rank.Value >= 0;
        }

        /// <summary>
        /// Adds the specified member with the specified score to the sorted set stored at key. If the specified member is already a member of the sorted set, the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to add to the sorted set.</param>
        /// <param name="score">The score for the member to add to the sorted set.</param>
        /// <returns>True if the value was added, False if it already existed (the score is still updated)</returns>
        public static bool SortedSetAdd<T>(string key, T member, double score)
        {
            return Execute(db => db.SortedSetAdd(key, member.ToRedisValue(RedisManager.DefaultSerializeType), score));
        }

        /// <summary>
        /// Removes the specified member from the sorted set stored at key. Non existing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to remove.</param>
        /// <returns>True if the member existed in the sorted set and was removed; False otherwise.</returns>
        public static bool SortedSetRemove<T>(string key, T member)
        {
            return Execute(db => db.SortedSetRemove(key, member.ToRedisValue(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score. Both start and stop are zero-based indexes, where 0 is the first element, 1 is the next element and so on. They can also be negative numbers indicating offsets from the end of the sorted set, with -1 being the last element of the sorted set, -2 the penultimate element and so on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="start">The start index to get.</param>
        /// <param name="stop">The stop index to get.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>List of elements in the specified range.</returns>
        public static List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return Execute<T>(db => db.SortedSetRangeByRank(key, start, stop, order));
        }

        /// <summary>
        ///  Returns the sorted set cardinality (number of elements) of the sorted set stored at key.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <returns>The cardinality (number of elements) of the sorted set, or 0 if key does not exist.</returns>
        public static long SortedSetLength(string key)
        {
            return Execute(db => db.SortedSetLength(key));
        }
        #endregion

        #region Asynchronous method
        /// <summary>
        /// Returns the rank of member in the sorted set stored at key, by default with the scores ordered from low to high. The rank (or index) is 0-based, which means that the member with the lowest score has rank 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to get the rank of.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>If member exists in the sorted set, the rank of member; If member does not exist in the sorted set or key does not exist, null</returns>
        public static async Task<long?> SortedSetRankAsync<T>(string key, T member, Order order = Order.Ascending)
        {
            return await ExecuteAsync(db => db.SortedSetRankAsync(key, member.ToRedisValue(RedisManager.DefaultSerializeType), order));
        }

        /// <summary>
        /// Returns if member exists in the sorted set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to get the rank of.</param>
        /// <returns>True if member exists in the sorted set; False otherwise.</returns>
        public static async Task<bool> SortedSetExistsAsync<T>(string key, T member)
        {
            var rank = await Execute(db => db.SortedSetRankAsync(key, member.ToRedisValue(RedisManager.DefaultSerializeType)));
            return rank.HasValue && rank.Value >= 0;
        }

        /// <summary>
        /// Adds the specified member with the specified score to the sorted set stored at key. If the specified member is already a member of the sorted set, the score is updated and the element reinserted at the right position to ensure the correct ordering.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to add to the sorted set.</param>
        /// <param name="score">The score for the member to add to the sorted set.</param>
        /// <returns>True if the value was added, False if it already existed (the score is still updated)</returns>
        public static async Task<bool> SortedSetAddAsync<T>(string key, T member, double score)
        {
            return await ExecuteAsync(db => db.SortedSetAddAsync(key, member.ToRedisValue(RedisManager.DefaultSerializeType), score));
        }

        /// <summary>
        /// Removes the specified member from the sorted set stored at key. Non existing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to remove.</param>
        /// <returns>True if the member existed in the sorted set and was removed; False otherwise.</returns>
        public static async Task<bool> SortedSetRemoveAsync<T>(string key, T member)
        {
            return await ExecuteAsync(db => db.SortedSetRemoveAsync(key, member.ToRedisValue(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Returns the specified range of elements in the sorted set stored at key. By default the elements are considered to be ordered from the lowest to the highest score. Lexicographical order is used for elements with equal score. Both start and stop are zero-based indexes, where 0 is the first element, 1 is the next element and so on. They can also be negative numbers indicating offsets from the end of the sorted set, with -1 being the last element of the sorted set, -2 the penultimate element and so on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="start">The start index to get.</param>
        /// <param name="stop">The stop index to get.</param>
        /// <param name="order">The order to sort by (defaults to ascending).</param>
        /// <returns>List of elements in the specified range.</returns>
        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return await ExecuteAsync<T>(db => db.SortedSetRangeByRankAsync(key, start, stop, order));
        }

        /// <summary>
        ///  Returns the sorted set cardinality (number of elements) of the sorted set stored at key.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <returns>The cardinality (number of elements) of the sorted set, or 0 if key does not exist.</returns>
        public static async Task<long> SortedSetLengthAsync(string key)
        {
            return await ExecuteAsync(db => db.SortedSetLengthAsync(key));
        }
        #endregion
    }
}
