using System.Threading.Tasks;

namespace Sean.Core.Redis
{
    public partial class RedisHelper
    {
        #region Synchronization method
        /// <summary>
        /// Add the specified member to the set stored at key. Specified members that are already a member of this set are ignored. If key does not exist, a new set is created before adding the specified members.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to add to the set.</param>
        /// <returns>True if the specified member was not already present in the set, else False</returns>
        public static bool SetAdd(string key, string value)
        {
            return Execute(db => db.SetAdd(key, value));
        }

        /// <summary>
        /// Returns the set cardinality (number of elements) of the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <returns>The cardinality (number of elements) of the set, or 0 if key does not exist.</returns>
        public static long SetLength(string key)
        {
            return Execute(db => db.SetLength(key));
        }

        /// <summary>
        /// Returns if member is a member of the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to check for.</param>
        /// <returns>1 if the element is a member of the set. 0 if the element is not a member of the set, or if key does not exist.</returns>
        public static bool SetContains(string key, string value)
        {
            return Execute(db => db.SetContains(key, value));
        }

        /// <summary>
        /// Remove the specified member from the set stored at key. Specified members that are not a member of this set are ignored.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if the specified member was already present in the set, else False</returns>
        public static bool SetRemove(string key, string value)
        {
            return Execute(db => db.SetRemove(key, value));
        }
        #endregion

        #region Asynchronous method
        /// <summary>
        /// Add the specified member to the set stored at key. Specified members that are already a member of this set are ignored. If key does not exist, a new set is created before adding the specified members.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to add to the set.</param>
        /// <returns>True if the specified member was not already present in the set, else False</returns>
        public static async Task<bool> SetAddAsync(string key, string value)
        {
            return await ExecuteAsync(db => db.SetAddAsync(key, value));
        }

        /// <summary>
        /// Returns the set cardinality (number of elements) of the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <returns>The cardinality (number of elements) of the set, or 0 if key does not exist.</returns>
        public static async Task<long> SetLengthAsync(string key)
        {
            return await ExecuteAsync(db => db.SetLengthAsync(key));
        }

        /// <summary>
        /// Returns if member is a member of the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to check for.</param>
        /// <returns>1 if the element is a member of the set. 0 if the element is not a member of the set, or if key does not exist.</returns>
        public static async Task<bool> SetContainsAsync(string key, string value)
        {
            return await ExecuteAsync(db => db.SetContainsAsync(key, value));
        }

        /// <summary>
        /// Remove the specified member from the set stored at key. Specified members that are not a member of this set are ignored.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if the specified member was already present in the set, else False</returns>
        public static async Task<bool> SetRemoveAsync(string key, string value)
        {
            return await ExecuteAsync(db => db.SetRemoveAsync(key, value));
        }
        #endregion
    }
}
