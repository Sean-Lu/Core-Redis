using System;
using System.Threading.Tasks;

namespace Sean.Core.Redis
{
    public partial class RedisHelper
    {
        // Lock（分布式锁）：由于 Redis 是单线程模型，命令操作原子性，所以利用这个特性可以很容易的实现分布式锁。

        #region Synchronization method
        /// <summary>
        /// Takes a lock (specifying a token value) if it is not already taken.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value to set at the key.</param>
        /// <param name="expiry">The expiration of the lock key.</param>
        /// <returns>True if the lock was successfully taken, false otherwise.</returns>
        public static bool LockTake(string key, string value, TimeSpan expiry)
        {
            return Execute(db => db.LockTake(key, value, expiry));
        }
        /// <summary>
        /// Releases a lock, if the token value is correct.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value at the key that must match.</param>
        /// <returns>True if the lock was successfully released, false otherwise.</returns>
        public static bool LockRelease(string key, string value)
        {
            return Execute(db => db.LockRelease(key, value));
        }
        #endregion

        #region Asynchronous method
        /// <summary>
        /// Takes a lock (specifying a token value) if it is not already taken.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value to set at the key.</param>
        /// <param name="expiry">The expiration of the lock key.</param>
        /// <returns>True if the lock was successfully taken, false otherwise.</returns>
        public static async Task<bool> LockTakeAsync(string key, string value, TimeSpan expiry)
        {
            return await ExecuteAsync(db => db.LockTakeAsync(key, value, expiry));
        }
        /// <summary>
        /// Releases a lock, if the token value is correct.
        /// </summary>
        /// <param name="key">The key of the lock.</param>
        /// <param name="value">The value at the key that must match.</param>
        /// <returns>True if the lock was successfully released, false otherwise.</returns>
        public static async Task<bool> LockReleaseAsync(string key, string value)
        {
            return await ExecuteAsync(db => db.LockReleaseAsync(key, value));
        }
        #endregion
    }
}
