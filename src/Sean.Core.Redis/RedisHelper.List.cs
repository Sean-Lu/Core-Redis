using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;

namespace Sean.Core.Redis;

public partial class RedisHelper
{
    #region Synchronization method
    /// <summary>
    /// Remove all elements equal to value from the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to remove from the list.</param>
    /// <returns>The number of removed elements.</returns>
    public static long ListRemove<T>(string key, T value)
    {
        return Execute(db => db.ListRemove(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Returns the specified elements of the list stored at key. The offsets start and stop are zero-based indexes, with 0 being the first element of the list (the head of the list), 1 being the next element and so on. These offsets can also be negative numbers indicating offsets starting at the end of the list.For example, -1 is the last element of the list, -2 the penultimate, and so on. Note that if you have a list of numbers from 0 to 100, LRANGE list 0 10 will return 11 elements, that is, the rightmost item is included.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="start">The start index of the list.</param>
    /// <param name="stop">The stop index of the list.</param>
    /// <returns>List of elements in the specified range.</returns>
    public static List<T> ListRange<T>(string key, long start = 0L, long stop = -1L)
    {
        return Execute<T>(db => db.ListRange(key, start, stop));
    }

    /// <summary>
    /// Insert the specified value at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to add to the tail of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static long ListRightPush<T>(string key, T value)
    {
        return Execute(db => db.ListRightPush(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }
    /// <summary>
    /// Insert all the specified values at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation. Elements are inserted one after the other to the tail of the list, from the leftmost element to the rightmost element. So for instance the command RPUSH mylist a b c will result into a list containing a as first element, b as second element and c as third element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="values">The values to add to the tail of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static long ListRightPush<T>(string key, List<T> values)
    {
        return Execute(db => db.ListRightPush(key, values.ToRedisValueArray(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Removes and returns the last element of the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <returns>The element being popped.</returns>
    public static T ListRightPop<T>(string key)
    {
        return Execute<T>(db => db.ListRightPop(key));
    }

    /// <summary>
    /// Insert the specified value at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to add to the head of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static long ListLeftPush<T>(string key, T value)
    {
        return Execute(db => db.ListLeftPush(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }
    /// <summary>
    /// Insert all the specified values at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations. Elements are inserted one after the other to the head of the list, from the leftmost element to the rightmost element. So for instance the command LPUSH mylist a b c will result into a list containing c as first element, b as second element and a as third element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="values">The values to add to the head of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static long ListLeftPush<T>(string key, List<T> values)
    {
        return Execute(db => db.ListLeftPush(key, values.ToRedisValueArray(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Removes and returns the first element of the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <returns>The value of the first element, or nil when key does not exist.</returns>
    public static T ListLeftPop<T>(string key)
    {
        return Execute<T>(db => db.ListLeftPop(key));
    }

    /// <summary>
    /// Returns the length of the list stored at key. If key does not exist, it is interpreted as an empty list and 0 is returned.
    /// </summary>
    /// <param name="key">The key of the list.</param>
    /// <returns>The length of the list at key.</returns>
    public static long ListLength(string key)
    {
        return Execute(db => db.ListLength(key));
    }
    #endregion

    #region Asynchronous method
    /// <summary>
    /// Remove all elements equal to value from the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to remove from the list.</param>
    /// <returns>The number of removed elements.</returns>
    public static async Task<long> ListRemoveAsync<T>(string key, T value)
    {
        return await ExecuteAsync(db => db.ListRemoveAsync(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Returns the specified elements of the list stored at key. The offsets start and stop are zero-based indexes, with 0 being the first element of the list (the head of the list), 1 being the next element and so on. These offsets can also be negative numbers indicating offsets starting at the end of the list.For example, -1 is the last element of the list, -2 the penultimate, and so on. Note that if you have a list of numbers from 0 to 100, LRANGE list 0 10 will return 11 elements, that is, the rightmost item is included.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="start">The start index of the list.</param>
    /// <param name="stop">The stop index of the list.</param>
    /// <returns>List of elements in the specified range.</returns>
    public static async Task<List<T>> ListRangeAsync<T>(string key, long start = 0L, long stop = -1L)
    {
        return await ExecuteAsync<T>(db => db.ListRangeAsync(key, start, stop));
    }

    /// <summary>
    /// Insert the specified value at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to add to the tail of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static async Task<long> ListRightPushAsync<T>(string key, T value)
    {
        return await ExecuteAsync(db => db.ListRightPushAsync(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }
    /// <summary>
    /// Insert all the specified values at the tail of the list stored at key. If key does not exist, it is created as empty list before performing the push operation. Elements are inserted one after the other to the tail of the list, from the leftmost element to the rightmost element. So for instance the command RPUSH mylist a b c will result into a list containing a as first element, b as second element and c as third element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="values">The values to add to the tail of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static async Task<long> ListRightPushAsync<T>(string key, List<T> values)
    {
        return await ExecuteAsync(db => db.ListRightPushAsync(key, values.ToRedisValueArray(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Removes and returns the last element of the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <returns>The element being popped.</returns>
    public static async Task<T> ListRightPopAsync<T>(string key)
    {
        return await ExecuteAsync<T>(db => db.ListRightPopAsync(key));
    }

    /// <summary>
    /// Insert the specified value at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="value">The value to add to the head of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static async Task<long> ListLeftPushAsync<T>(string key, T value)
    {
        return await ExecuteAsync(db => db.ListLeftPushAsync(key, value.ToRedisValue(RedisManager.DefaultSerializeType)));
    }
    /// <summary>
    /// Insert all the specified values at the head of the list stored at key. If key does not exist, it is created as empty list before performing the push operations. Elements are inserted one after the other to the head of the list, from the leftmost element to the rightmost element. So for instance the command LPUSH mylist a b c will result into a list containing c as first element, b as second element and a as third element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <param name="values">The values to add to the head of the list.</param>
    /// <returns>The length of the list after the push operation.</returns>
    public static async Task<long> ListLeftPushAsync<T>(string key, List<T> values)
    {
        return await ExecuteAsync(db => db.ListLeftPushAsync(key, values.ToRedisValueArray(RedisManager.DefaultSerializeType)));
    }

    /// <summary>
    /// Removes and returns the first element of the list stored at key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key of the list.</param>
    /// <returns>The value of the first element, or nil when key does not exist.</returns>
    public static async Task<T> ListLeftPopAsync<T>(string key)
    {
        return await ExecuteAsync<T>(db => db.ListLeftPopAsync(key));
    }

    /// <summary>
    /// Returns the length of the list stored at key. If key does not exist, it is interpreted as an empty list and 0 is returned.
    /// </summary>
    /// <param name="key">The key of the list.</param>
    /// <returns>The length of the list at key.</returns>
    public static async Task<long> ListLengthAsync(string key)
    {
        return await ExecuteAsync(db => db.ListLengthAsync(key));
    }
    #endregion
}