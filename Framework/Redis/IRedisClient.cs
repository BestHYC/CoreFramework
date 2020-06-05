using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IRedisClient
    {
        Task<long> HDelAsync(string key, params string[] fields);




        /// <summary>
        /// Determine if a hash field exists
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to check</param>
        /// <returns>True if hash field exists</returns>
        Task<bool> HExistsAsync(string key, string field);
        /// <summary>
        /// Get the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to get</param>
        /// <returns>Value of hash field</returns>
        Task<string> HGetAsync(string key, string field);
        Task<byte[]> HGetBytesAsync(string key, string field);
        /// <summary>
        /// Get all the fields and values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Dictionary mapped from string</returns>
        Task<Dictionary<string, string>> HGetAllAsync(string key);



        /// <summary>
        /// Increment the integer value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        Task<long> HIncrByAsync(string key, string field, long increment);




        /// <summary>
        /// Increment the float value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        Task<decimal> HIncrByFloatAsync(string key, string field, decimal increment);




        /// <summary>
        /// Get all the fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>All hash field names</returns>
        Task<string[]> HKeysAsync(string key);




        /// <summary>
        /// Get the number of fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Number of fields in hash</returns>
        Task<long> HLenAsync(string key);




        /// <summary>
        /// Get the values of all the given hash fields
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="fields">Fields to return</param>
        /// <returns>Values of given fields</returns>
        Task<string[]> HMGetAsync(string key, params string[] fields);
        Task<byte[][]> HMGetBytesAsync(string key, params string[] fields);



        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="dict">Dictionary mapping of hash</param>
        /// <returns>Status code</returns>
        Task<string> HMSetAsync(string key, Dictionary<string, object> dict);




        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <typeparam name="T">Type of object to map hash</typeparam>
        /// <param name="key">Hash key</param>
        /// <param name="obj">Object mapping of hash</param>
        /// <returns>Status code</returns>
        Task<string> HMSetAsync<T>(string key, T obj)
                    where T : class;




        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="keyValues">Array of [key,value,key,value,..]</param>
        /// <returns>Status code</returns>
        Task<string> HMSetAsync(string key, params object[] keyValues);




        /// <summary>
        /// Set the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field is new</returns>
        Task<bool> HSetAsync(string key, string field, object value);




        /// <summary>
        /// Set the value of a hash field, only if the field does not exist
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field was set to value</returns>
        Task<bool> HSetNxAsync(string key, string field, object value);




        /// <summary>
        /// Get all the values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Array of all values in hash</returns>
        Task<string[]> HValsAsync(string key);
        Task<byte[][]> HValsBytesAsync(string key);

        /// <summary>
        /// Iterate the keys and values of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<Tuple<string, string>>> HScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<Tuple<string, byte[]>>> HScanBytesAsync(string key, long cursor, string pattern = null, long? count = null);
        /// <summary>
        /// Get an element from a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">Zero-based index of item to return</param>
        /// <returns>Element at index</returns>
        Task<string> LIndexAsync(string key, long index);
        Task<byte[]> LIndexBytesAsync(string key, long index);



        /// <summary>
        /// Insert an element before or after another element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="insertType">Relative position</param>
        /// <param name="pivot">Relative element</param>
        /// <param name="value">Element to insert</param>
        /// <returns>Length of list after insert or -1 if pivot not found</returns>
        Task<long> LInsertAsync(string key, RedisInsert insertType, object pivot, object value);




        /// <summary>
        /// Get the length of a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of list at key</returns>
        Task<long> LLenAsync(string key);




        /// <summary>
        /// Remove and get the first element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>First element in list</returns>
        Task<string> LPopAsync(string key);
        Task<byte[]> LPopBytesAsync(string key);



        /// <summary>
        /// Prepend one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> LPushAsync(string key, params object[] values);




        /// <summary>
        /// Prepend a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> LPushXAsync(string key, object value);




        /// <summary>
        /// Get a range of elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in range</returns>
        Task<string[]> LRangeAsync(string key, long start, long stop);
        Task<byte[][]> LRangeBytesAsync(string key, long start, long stop);



        /// <summary>
        /// Remove elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="count">&gt;0: remove N elements from head to tail; &lt;0: remove N elements from tail to head; =0: remove all elements</param>
        /// <param name="value">Remove elements equal to value</param>
        /// <returns>Number of removed elements</returns>
        Task<long> LRemAsync(string key, long count, object value);




        /// <summary>
        /// Set the value of an element in a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">List index to modify</param>
        /// <param name="value">New element value</param>
        /// <returns>Status code</returns>
        Task<string> LSetAsync(string key, long index, object value);




        /// <summary>
        /// Trim a list to the specified range
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Zero-based start index</param>
        /// <param name="stop">Zero-based stop index</param>
        /// <returns>Status code</returns>
        Task<string> LTrimAsync(string key, long start, long stop);




        /// <summary>
        /// Remove and get the last elment in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Value of last list element</returns>
        Task<string> RPopAsync(string key);
        Task<byte[]> RPopBytesAsync(string key);



        /// <summary>
        /// Remove the last elment in a list, append it to another list and return it
        /// </summary>
        /// <param name="source">List source key</param>
        /// <param name="destination">Destination key</param>
        /// <returns>Element being popped and pushed</returns>
        Task<string> RPopLPushAsync(string source, string destination);
        Task<byte[]> RPopBytesLPushAsync(string source, string destination);



        /// <summary>
        /// Append one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> RPushAsync(string key, params object[] values);




        /// <summary>
        /// Append a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> RPushXAsync(string key, object value);

        #region Sets
        /// <summary>
        /// Add one or more members to a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Members to add to set</param>
        /// <returns>Number of elements added to set</returns>
        Task<long> SAddAsync(string key, params object[] members);




        /// <summary>
        /// Get the number of members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in set</returns>
        Task<long> SCardAsync(string key);




        /// <summary>
        /// Subtract multiple sets
        /// </summary>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SDiffAsync(params string[] keys);
        Task<byte[][]> SDiffBytesAsync(params string[] keys);



        /// <summary>
        /// Subtract multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Number of elements in the resulting set</returns>
        Task<long> SDiffStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Intersect multiple sets
        /// </summary>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SInterAsync(params string[] keys);
        Task<byte[][]> SInterBytesAsync(params string[] keys);



        /// <summary>
        /// Intersect multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Number of elements in resulting set</returns>
        Task<long> SInterStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Determine if a given value is a member of a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>True if member exists in set</returns>
        Task<bool> SIsMemberAsync(string key, object member);




        /// <summary>
        /// Get all the members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All elements in the set</returns>
        Task<string[]> SMembersAsync(string key);
        Task<byte[][]> SMembersBytesAsync(string key);



        /// <summary>
        /// Move a member from one set to another
        /// </summary>
        /// <param name="source">Source key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="member">Member to move</param>
        /// <returns>True if element was moved</returns>
        Task<bool> SMoveAsync(string source, string destination, object member);




        /// <summary>
        /// Remove and return a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>The removed element</returns>
        Task<string> SPopAsync(string key);
        Task<byte[]> SPopBytesAsync(string key);


        /// <summary>
        /// Remove and return one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to remove and return</param>
        /// <returns></returns>
        Task<string[]> SPopAsync(string key, long count);
        Task<byte[][]> SPopBytesAsync(string key, long count);


        /// <summary>
        /// Get a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>One random element from set</returns>
        Task<string> SRandMemberAsync(string key);
        Task<byte[]> SRandMemberBytesAsync(string key);



        /// <summary>
        /// Get one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>One or more random elements from set</returns>
        Task<string[]> SRandMembersAsync(string key, long count);
        Task<byte[][]> SRandMembersBytesAsync(string key, long count);



        /// <summary>
        /// Remove one or more members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Set members to remove</param>
        /// <returns>Number of elements removed from set</returns>
        Task<long> SRemAsync(string key, params object[] members);




        /// <summary>
        /// Add multiple sets
        /// </summary>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SUnionAsync(params string[] keys);
        Task<byte[][]> SUnionBytesAsync(params string[] keys);



        /// <summary>
        /// Add multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Number of elements in resulting set</returns>
        Task<long> SUnionStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Iterate the elements of a set field
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<byte[]>> SScanBytesAsync(string key, long cursor, string pattern = null, long? count = null);


        #endregion

        #region Sorted Sets
        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores to add to sorted set</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        Task<long> ZAddAsync<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers);




        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores [s1, m1, s2, m2, ..]</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        Task<long> ZAddAsync(string key, params object[] scoreMembers);




        /// <summary>
        /// Get the number of members in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        Task<long> ZCardAsync(string key);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZCountAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZCountAsync(string key, string min, string max);




        /// <summary>
        /// Increment the score of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="increment">Increment by value</param>
        /// <param name="member">Sorted set member to increment</param>
        /// <returns>New score of member</returns>
        Task<decimal> ZIncrByAsync(string key, decimal increment, object member);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZInterStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZInterStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Return a range of members in a sorted set, by index
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>Array of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeAsync(string key, long start, long stop, bool withScores = false);
        Task<byte[][]> ZRangeBytesAsync(string key, long start, long stop, bool withScores = false);




        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Array of elements in the specified range with scores</returns>
        Task<Tuple<string, decimal>[]> ZRangeWithScoresAsync(string key, long start, long stop);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesWithScoresAsync(string key, long start, long stop);



        /// <summary>
        /// Return a range of members in a sorted set, by score
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member or null if key does not exist</returns>
        Task<long?> ZRankAsync(string key, object member);




        /// <summary>
        /// Remove one or more members from a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="members">Members to remove</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemAsync(string key, params object[] members);




        /// <summary>
        /// Remove all members in a sorted set within the given indexes
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByRankAsync(string key, long start, long stop);




        /// <summary>
        /// Remove all members in a sorted set within the given scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);
        Task<long> ZRemRangeByScoreAsync(string key, string min, string max);




        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRevRangeAsync(string key, long start, long stop, bool withScores = false);
        Task<byte[][]> ZRevRangeBytesAsync(string key, long start, long stop, bool withScores = false);



        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeWithScoresAsync(string key, long start, long stop);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesWithScoresAsync(string key, long start, long stop);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member, or null if member does not exist</returns>
        Task<long?> ZRevRankAsync(string key, object member);




        /// <summary>
        /// Get the score associated with the given member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Score of member, or null if member does not exist</returns>
        Task<decimal?> ZScoreAsync(string key, object member);




        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZUnionStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Iterate the scores and elements of a sorted set field
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<Tuple<string, decimal>>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<Tuple<byte[], decimal>>> ZScanBytesAsync(string key, long cursor, string pattern = null, long? count = null);



        /// <summary>
        /// Retrieve all the elements in a sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="offset">Limit result set by offset</param>
        /// <param name="count">Limimt result set by size</param>
        /// <returns>List of elements in the specified range</returns>
        Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByLexAsync(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Remove all elements in the sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByLexAsync(string key, string min, string max);




        /// <summary>
        /// Returns the number of elements in the sorted set with a value between min and max.
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZLexCountAsync(string key, string min, string max);
        #endregion
    }
}
