using System;

/// <summary>
///     加载包
/// </summary>
public class AssetPack
{
    /// <summary>
    ///     资源
    /// </summary>
    public UnityEngine.Object asset;

    /// <summary>
    ///     是否常驻内存
    /// </summary>
    public bool isKeepInMemory;

    /// <summary>
    ///     资源类型
    /// </summary>
    public Type type;

    public AssetPack(Type type, bool isKeepInMemory)
    {
        this.type = type;
        this.isKeepInMemory = isKeepInMemory;
    }
}