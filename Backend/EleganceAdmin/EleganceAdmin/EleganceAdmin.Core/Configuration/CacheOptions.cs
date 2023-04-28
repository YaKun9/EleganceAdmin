using EleganceAdmin.Core.Const;
using Furion.ConfigurableOptions;

namespace EleganceAdmin.Core.Configuration;

/// <summary>
/// 缓存配置
/// </summary>
public class CacheOptions : IConfigurableOptions
{
    /// <summary>
    /// 缓存类型 <see cref="CacheTypeConst"/>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Memory 内存缓存</item>
    /// <item>Redis Redis分布式缓存</item>
    /// </list>
    /// </remarks>
    public string CacheType { get; set; }

    /// <summary>
    /// 连接字符串,当缓存类型为Redis时有效
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 缓存前缀
    /// </summary>
    public string Prefix { get; set; }
}