using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;

namespace Sean.Core.Redis
{
    public partial class RedisHelper
    {
        #region Synchronization method
        /// <summary>
        /// Returns if field is an existing field in the hash stored at key.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to check.</param>
        /// <returns>Return true if the hash contains field. Return false if the hash does not contain field, or key does not exist.</returns>
        public static bool HashExists(string key, string hashField)
        {
            return Execute(db => db.HashExists(key, hashField));
        }

        /// <summary>
        /// Sets field in the hash stored at key to value. If key does not exist, a new key holding a hash is created. If field already exists in the hash, it is overwritten.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field to set in the hash.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>Return true if field is a new field in the hash and value was set. Return false if field already exists in the hash and the value was updated.</returns>
        public static bool HashSet<T>(string key, string hashField, T value)
        {
            return Execute(db => db.HashSet(key, hashField, value.ToRedisValue(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns false.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to delete.</param>
        /// <returns>The number of fields that were removed.</returns>
        public static bool HashDelete(string key, string hashField)
        {
            return Execute(db => db.HashDelete(key, hashField));
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns false.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashFields">The fields in the hash to delete.</param>
        /// <returns>The number of fields that were removed.</returns>
        public static long HashDelete(string key, IList<string> hashFields)
        {
            return Execute(db => db.HashDelete(key, hashFields.ToRedisValueArray(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Returns the value associated with field in the hash stored at key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to get.</param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string hashField)
        {
            return Execute<T>(db => db.HashGet(key, hashField));
        }

        /// <summary>
        /// Increment the specified field of an hash stored at key, and representing a floating point number, by the specified increment. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to increment.</param>
        /// <param name="value">The amount to increment by.</param>
        /// <returns>The value at field after the increment operation.</returns>
        public static double HashIncrement(string key, string hashField, double value = 1)
        {
            return Execute(db => db.HashIncrement(key, hashField, value));
        }

        /// <summary>
        /// Decrement the specified field of an hash stored at key, and representing a floating point number, by the specified decrement. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to decrement.</param>
        /// <param name="value">The amount to decrement by.</param>
        /// <returns>The value at field after the decrement operation.</returns>
        public static double HashDecrement(string key, string hashField, double value = 1)
        {
            return Execute(db => db.HashDecrement(key, hashField, value));
        }

        /// <summary>
        /// Returns all field names in the hash stored at key.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <returns>List of fields in the hash, or an empty list when key does not exist.</returns>
        public static List<string> HashKeys(string key)
        {
            return Execute<string>(db => db.HashKeys(key));
        }
        #endregion

        #region Asynchronous method
        /// <summary>
        /// Returns if field is an existing field in the hash stored at key.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to check.</param>
        /// <returns>Return true if the hash contains field. Return false if the hash does not contain field, or key does not exist.</returns>
        public static async Task<bool> HashExistsAsync(string key, string hashField)
        {
            return await ExecuteAsync(db => db.HashExistsAsync(key, hashField));
        }

        /// <summary>
        /// Sets field in the hash stored at key to value. If key does not exist, a new key holding a hash is created. If field already exists in the hash, it is overwritten.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field to set in the hash.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>Return true if field is a new field in the hash and value was set. Return false if field already exists in the hash and the value was updated.</returns>
        public static async Task<bool> HashSetAsync<T>(string key, string hashField, T value)
        {
            return await ExecuteAsync(db => db.HashSetAsync(key, hashField, value.ToRedisValue(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns false.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to delete.</param>
        /// <returns>The number of fields that were removed.</returns>
        public static async Task<bool> HashDeleteAsync(string key, string hashField)
        {
            return await ExecuteAsync(db => db.HashDeleteAsync(key, hashField));
        }

        /// <summary>
        /// Removes the specified fields from the hash stored at key. Non-existing fields are ignored. Non-existing keys are treated as empty hashes and this command returns false.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashFields">The fields in the hash to delete.</param>
        /// <returns>The number of fields that were removed.</returns>
        public static async Task<long> HashDeleteAsync(string key, IList<string> hashFields)
        {
            return await ExecuteAsync(db => db.HashDeleteAsync(key, hashFields.ToRedisValueArray(RedisManager.DefaultSerializeType)));
        }

        /// <summary>
        /// Returns the value associated with field in the hash stored at key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to get.</param>
        /// <returns></returns>
        public static async Task<T> HashGetAsync<T>(string key, string hashField)
        {
            return await ExecuteAsync<T>(db => db.HashGetAsync(key, hashField));
        }

        /// <summary>
        /// Increment the specified field of an hash stored at key, and representing a floating point number, by the specified increment. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to increment.</param>
        /// <param name="value">The amount to increment by.</param>
        /// <returns>The value at field after the increment operation.</returns>
        public static async Task<double> HashIncrementAsync(string key, string hashField, double value = 1)
        {
            return await ExecuteAsync(db => db.HashIncrementAsync(key, hashField, value));
        }

        /// <summary>
        /// Decrement the specified field of an hash stored at key, and representing a floating point number, by the specified decrement. If the field does not exist, it is set to 0 before performing the operation.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashField">The field in the hash to decrement.</param>
        /// <param name="value">The amount to decrement by.</param>
        /// <returns>The value at field after the decrement operation.</returns>
        public static async Task<double> HashDecrementAsync(string key, string hashField, double value = 1)
        {
            return await ExecuteAsync(db => db.HashDecrementAsync(key, hashField, value));
        }

        /// <summary>
        /// Returns all field names in the hash stored at key.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <returns>List of fields in the hash, or an empty list when key does not exist.</returns>
        public static async Task<List<string>> HashKeysAsync(string key)
        {
            return await ExecuteAsync<string>(db => db.HashKeysAsync(key));
        }
        #endregion
    }
}
