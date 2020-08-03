using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    
    public abstract class HBaseRedisHelper <T>
    {
        private static CSRedisClient Instace;
        private static Object m_lock = new object();
        public HBaseRedisHelper(HRedisName name)
        {
            if(Instace == null)
            {
                lock (m_lock)
                {
                    if (Instace != null)
                    {
                        var item = HRedisContext.Get(name);
                        if (item == null)
                        {
                            throw new ArgumentException($"没注入{name}的reids");
                        }
                        RedisHelper<T>.Initialization(item);
                        Instace = RedisHelper<T>.Instance;
                    }
                }
            }
        }
        public async Task<long> HDelAsync(string key, params string[] fields)
        {
            return await RedisHelper<T>.HDelAsync(key, fields);
        }

        public async Task<bool> HExistsAsync(string key, string field)
        {
            return await RedisHelper<T>.HExistsAsync(key, field);
        }

        public Task<K> HGetAllAsync<K>(string key) where K : class
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, string>> HGetAllAsync(string key)
        {
            return await RedisHelper<T>.HGetAllAsync(key);
        }

        public async Task<string> HGetAsync(string key, string field)
        {
            return await RedisHelper<T>.HGetAsync(key, field);
        }

        public async Task<byte[]> HGetBytesAsync(string key, string field)
        {
            return await RedisHelper<T>.HGetAsync<byte[]>(key, field);
        }

        public async Task<long> HIncrByAsync(string key, string field, long increment)
        {
            return await RedisHelper<T>.HIncrByAsync(key, field, increment);
        }

        public async Task<decimal> HIncrByFloatAsync(string key, string field, decimal increment)
        {
            return await RedisHelper<T>.HIncrByFloatAsync(key, field, increment);
        }

        public async Task<string[]> HKeysAsync(string key)
        {
            return await RedisHelper<T>.HKeysAsync(key);
        }

        public async Task<long> HLenAsync(string key)
        {
            return await RedisHelper<T>.HLenAsync(key);
        }

        public async Task<string[]> HMGetAsync(string key, params string[] fields)
        {
            return await RedisHelper<T>.HMGetAsync(key, fields);
        }

        public Task<string> HMSetAsync<K>(string key, K obj) where K : class
        {
            throw new NotImplementedException();
        }

        public Task<string> HMSetAsync(string key, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<Tuple<string, string>>> HScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<Tuple<string, byte[]>>> HScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HSetAsync(string key, string field, object value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HSetNxAsync(string key, string field, object value)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> HValsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> HValsBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> LIndexAsync(string key, long index)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> LIndexBytesAsync(string key, long index)
        {
            throw new NotImplementedException();
        }

        public Task<long> LInsertAsync(string key, RedisInsert insertType, object pivot, object value)
        {
            throw new NotImplementedException();
        }

        public Task<long> LLenAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string> LPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> LPopBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushAsync(string key, params object[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushXAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> LRangeAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> LRangeBytesAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<long> LRemAsync(string key, long count, object value)
        {
            throw new NotImplementedException();
        }

        public Task<string> LSetAsync(string key, long index, object value)
        {
            throw new NotImplementedException();
        }

        public Task<string> LTrimAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<string> RPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> RPopBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> RPopBytesLPushAsync(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Task<string> RPopLPushAsync(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushAsync(string key, params object[] values)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushXAsync(string key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<long> SAddAsync(string key, params object[] members)
        {
            throw new NotImplementedException();
        }

        public Task<long> SCardAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SDiffAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SDiffBytesAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> SDiffStoreAsync(string destination, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SInterAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SInterBytesAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> SInterStoreAsync(string destination, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SIsMemberAsync(string key, object member)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SMembersAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SMembersBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SMoveAsync(string source, string destination, object member)
        {
            throw new NotImplementedException();
        }

        public Task<string> SPopAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SPopAsync(string key, long count)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> SPopBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SPopBytesAsync(string key, long count)
        {
            throw new NotImplementedException();
        }

        public Task<string> SRandMemberAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> SRandMemberBytesAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SRandMembersAsync(string key, long count)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SRandMembersBytesAsync(string key, long count)
        {
            throw new NotImplementedException();
        }

        public Task<long> SRemAsync(string key, params object[] members)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<byte[]>> SScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SUnionAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> SUnionBytesAsync(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> SUnionStoreAsync(string destination, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZAddAsync<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZAddAsync(string key, params object[] scoreMembers)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZCardAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZCountAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZCountAsync(string key, string min, string max)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> ZIncrByAsync(string key, decimal increment, object member)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZInterStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZInterStoreAsync(string destination, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZLexCountAsync(string key, string min, string max)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRangeAsync(string key, long start, long stop, bool withScores = false)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRangeByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRangeBytesAsync(string key, long start, long stop, bool withScores = false)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRangeBytesByLexAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRangeBytesByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRangeBytesByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRangeBytesWithScoresAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRangeWithScoresAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<long?> ZRankAsync(string key, object member)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemAsync(string key, params object[] members)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemRangeByLexAsync(string key, string min, string max)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemRangeByRankAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemRangeByScoreAsync(string key, string min, string max)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRevRangeAsync(string key, long start, long stop, bool withScores = false)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRevRangeBytesAsync(string key, long start, long stop, bool withScores = false)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<byte[], decimal>[]> ZRevRangeBytesWithScoresAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, decimal>[]> ZRevRangeWithScoresAsync(string key, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<long?> ZRevRankAsync(string key, object member)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<Tuple<string, decimal>>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<RedisScan<Tuple<byte[], decimal>>> ZScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> ZScoreAsync(string key, object member)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZUnionStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
