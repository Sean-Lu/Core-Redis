using System;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;

namespace Sean.Core.Redis;

public static partial class RedisHelper
{
    #region Synchronization method
    /// <summary>
    /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="expiry">The expiry to set.</param>
    /// <returns>True if the string was set, false otherwise.</returns>
    public static bool StringSet<T>(string key, T value, TimeSpan? expiry = null)
    {
        return Execute(db => db.StringSet(key, value.ToRedisValue(RedisManager.DefaultSerializeType), expiry));
    }

    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public static string StringGet(string key)
    {
        return Execute(db => db.StringGet(key));
    }
    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public static T StringGet<T>(string key)
    {
        return Execute<T>(db => db.StringGet(key));
    }

    /// <summary>
    /// Increments the string representing a floating point number stored at key by the specified increment. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The amount to increment by (defaults to 1).</param>
    /// <returns>The value of key after the increment.</returns>
    public static double StringIncrement(string key, double value = 1)
    {
        return Execute(db => db.StringIncrement(key, value));
    }
    /// <summary>
    /// Decrements the string representing a floating point number stored at key by the specified decrement. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The amount to decrement by (defaults to 1).</param>
    /// <returns>The value of key after the decrement.</returns>
    public static double StringDecrement(string key, double value = 1)
    {
        return Execute(db => db.StringDecrement(key, value));
    }
    #endregion

    #region Asynchronous method
    /// <summary>
    /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="expiry">The expiry to set.</param>
    /// <returns>True if the string was set, false otherwise.</returns>
    public static async Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        return await ExecuteAsync(db => db.StringSetAsync(key, value.ToRedisValue(RedisManager.DefaultSerializeType), expiry));
    }

    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public static async Task<string> StringGetAsync(string key)
    {
        return await ExecuteAsync(db => db.StringGetAsync(key));
    }
    /// <summary>
    /// Get the value of key. If the key does not exist the special value nil is returned. An error is returned if the value stored at key is not a string, because GET only handles string values.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <returns>The value of key, or nil when key does not exist.</returns>
    public static async Task<T> StringGetAsync<T>(string key)
    {
        return await ExecuteAsync<T>(db => db.StringGetAsync(key));
    }

    /// <summary>
    /// Increments the string representing a floating point number stored at key by the specified increment. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The amount to increment by (defaults to 1).</param>
    /// <returns>The value of key after the increment.</returns>
    public static async Task<double> StringIncrementAsync(string key, double value = 1)
    {
        return await ExecuteAsync(db => db.StringIncrementAsync(key, value));
    }
    /// <summary>
    /// Decrements the string representing a floating point number stored at key by the specified decrement. If the key does not exist, it is set to 0 before performing the operation. The precision of the output is fixed at 17 digits after the decimal point regardless of the actual internal precision of the computation.
    /// </summary>
    /// <param name="key">The key of the string.</param>
    /// <param name="value">The amount to decrement by (defaults to 1).</param>
    /// <returns>The value of key after the decrement.</returns>
    public static async Task<double> StringDecrementAsync(string key, double value = 1)
    {
        return await ExecuteAsync(db => db.StringDecrementAsync(key, value));
    }
    #endregion
}