using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;

namespace Sean.Core.Redis;

public static partial class RedisHelper
{
    #region Synchronization method
    /// <summary>
    /// Returns if key exists.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>1 if the key exists. 0 if the key does not exist.</returns>
    public static bool KeyExists(string key)
    {
        return Execute(db => db.KeyExists(key));
    }

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist. If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    /// <returns>True if the key was removed.</returns>
    public static bool KeyDelete(string key)
    {
        return Execute(db => db.KeyDelete(key));
    }
    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist. If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="keys">The key to delete.</param>
    /// <returns>The number of keys that were removed.</returns>
    public static long KeyDelete(IList<string> keys)
    {
        return Execute(db => db.KeyDelete(keys.ToRedisKeyArray()));
    }

    /// <summary>
    /// Renames key to newkey. It returns an error when the source and destination names are the same, or when key does not exist.
    /// </summary>
    /// <param name="key">The key to rename.</param>
    /// <param name="newKey">The key to rename to.</param>
    /// <returns>True if the key was renamed, false otherwise.</returns>
    public static bool KeyRename(string key, string newKey)
    {
        return Execute(db => db.KeyRename(key, newKey));
    }

    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="key">The key to set the expiration for.</param>
    /// <param name="expiry">The exact date to expiry to set.</param>
    /// <returns>1 if the timeout was set. 0 if key does not exist or the timeout could not be set.</returns>
    public static bool KeyExpire(string key, DateTime? expiry)
    {
        return Execute(db => db.KeyExpire(key, expiry));
    }
    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="key">The key to set the expiration for.</param>
    /// <param name="expiry">The timeout to set.</param>
    /// <returns>1 if the timeout was set. 0 if key does not exist or the timeout could not be set.</returns>
    public static bool KeyExpire(string key, TimeSpan? expiry)
    {
        return Execute(db => db.KeyExpire(key, expiry));
    }

    /// <summary>
    /// TTL (time to live)
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
    public static TimeSpan? KeyTimeToLive(string key)
    {
        // Redis TTL 命令 - 以秒为单位，返回给定 key 的剩余生存时间(TTL, time to live)。
        // 当 key 存在但没有设置剩余生存时间时，返回 -1 。
        return Execute(db => db.KeyTimeToLive(key));
    }
    #endregion

    #region Asynchronous method
    /// <summary>
    /// Returns if key exists.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>1 if the key exists. 0 if the key does not exist.</returns>
    public static async Task<bool> KeyExistsAsync(string key)
    {
        return await ExecuteAsync(db => db.KeyExistsAsync(key));
    }

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist. If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    /// <returns>True if the key was removed.</returns>
    public static async Task<bool> KeyDeleteAsync(string key)
    {
        return await ExecuteAsync(db => db.KeyDeleteAsync(key));
    }

    /// <summary>
    /// Removes the specified key. A key is ignored if it does not exist. If UNLINK is available (Redis 4.0+), it will be used.
    /// </summary>
    /// <param name="keys">The key to delete.</param>
    /// <returns>The number of keys that were removed.</returns>
    public static async Task<long> KeyDeleteAsync(IList<string> keys)
    {
        return await ExecuteAsync(db => db.KeyDeleteAsync(keys.ToRedisKeyArray()));
    }

    /// <summary>
    /// Renames key to newkey. It returns an error when the source and destination names are the same, or when key does not exist.
    /// </summary>
    /// <param name="key">The key to rename.</param>
    /// <param name="newKey">The key to rename to.</param>
    /// <returns>True if the key was renamed, false otherwise.</returns>
    public static async Task<bool> KeyRenameAsync(string key, string newKey)
    {
        return await ExecuteAsync(db => db.KeyRenameAsync(key, newKey));
    }

    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="key">The key to set the expiration for.</param>
    /// <param name="expiry">The exact date to expiry to set.</param>
    /// <returns>1 if the timeout was set. 0 if key does not exist or the timeout could not be set.</returns>
    public static async Task<bool> KeyExpireAsync(string key, DateTime? expiry)
    {
        return await ExecuteAsync(db => db.KeyExpireAsync(key, expiry));
    }
    /// <summary>
    /// Set a timeout on key. After the timeout has expired, the key will automatically be deleted. A key with an associated timeout is said to be volatile in Redis terminology.
    /// </summary>
    /// <param name="key">The key to set the expiration for.</param>
    /// <param name="expiry">The timeout to set.</param>
    /// <returns>1 if the timeout was set. 0 if key does not exist or the timeout could not be set.</returns>
    public static async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry)
    {
        return await ExecuteAsync(db => db.KeyExpireAsync(key, expiry));
    }

    /// <summary>
    /// TTL (time to live)
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>TTL, or nil when key does not exist or does not have a timeout.</returns>
    public static async Task<TimeSpan?> KeyTimeToLiveAsync(string key)
    {
        return await ExecuteAsync(db => db.KeyTimeToLiveAsync(key));
    }
    #endregion
}