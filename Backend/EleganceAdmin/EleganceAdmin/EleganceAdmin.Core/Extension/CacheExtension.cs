using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SageTools.Extension;
using StackExchange.Profiling.Internal;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace EleganceAdmin.Core.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 获取缓存值，如果缓存不存在，则执行 factory 方法，并将 factory 方法的返回值存入缓存
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cache">IDistributedCache实例</param>
        /// <param name="key">缓存key</param>
        /// <param name="factory">业务方法</param>
        /// <param name="expiry">过期时间(绝对时间策略)</param>
        /// <returns></returns>
        public static async Task<TValue> GetAsync<TKey, TValue>(this IDistributedCache cache, TKey key, Func<Task<TValue>> factory, TimeSpan? expiry = null) where TValue : class
        {
            var strKey = key?.ToString() ?? string.Empty;
            // 尝试从缓存中获取值
            var cachedValue = await cache.GetStringAsync(strKey);
            if (cachedValue.IsNotNullOrWhiteSpace())
            {
                // 如果缓存值存在，则直接返回
                return cachedValue.FromJson<TValue>();
            }
            // 如果缓存值不存在，则执行 Factory 方法
            var value = await factory();
            if (value != null)
            {
                // 将结果存入缓存
                await cache.SetStringAsync(strKey, value.ToJson(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                });
            }
            return value;
        }

        /// <summary>
        /// 获取缓存值，如果缓存不存在，则执行 factory 方法，并将 factory 方法的返回值存入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">IDistributedCache实例</param>
        /// <param name="key">缓存key</param>
        /// <param name="factory">业务方法</param>
        /// <param name="chunkSize">分片大小</param>
        /// <param name="expiry">过期时间(绝对时间策略)</param>
        /// <returns></returns>
        public static async Task<T> GetCompressedAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, int chunkSize = 10000, TimeSpan? expiry = null) where T : class
        {
            var cachedValue = await cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedValue))
            {
                return DecompressAndDeserialize(cachedValue);
            }

            var result = await factory();

            if (result != null)
            {
                var serializedValue = CompressAndSerialize(result);
                var chunks = ChunkString(serializedValue);

                var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry };
                var tasks = new Task[chunks.Length];
                for (var i = 0; i < chunks.Length; i++)
                {
                    var chunkKey = GetChunkKey(key, i);
                    tasks[i] = cache.SetStringAsync(chunkKey, chunks[i], options);
                }

                await Task.WhenAll(tasks);
            }

            return result;

            string CompressAndSerialize(object obj)
            {
                var json = JsonConvert.SerializeObject(obj);
                var bytes = Encoding.UTF8.GetBytes(json);

                using var memoryStream = new MemoryStream();
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }

            T DecompressAndDeserialize(string compressedValue)
            {
                var bytes = Convert.FromBase64String(compressedValue);
                using var memoryStream = new MemoryStream(bytes);
                using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(gzipStream);
                var json = streamReader.ReadToEnd();
                return json.FromJson<T>();
            }

            string GetChunkKey(string ck, int index) => $"{ck}:chunk{index}";

            string[] ChunkString(string value)
            {
                var chunksCount = (int)Math.Ceiling((double)value.Length / chunkSize);
                var chunks = new string[chunksCount];

                for (var i = 0; i < chunksCount; i++)
                {
                    var startIndex = i * chunkSize;
                    var length = Math.Min(chunkSize, value.Length - startIndex);
                    chunks[i] = value.Substring(startIndex, length);
                }

                return chunks;
            }
        }
    }
}