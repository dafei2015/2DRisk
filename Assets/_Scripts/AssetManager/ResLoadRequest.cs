using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 资源加载类，用于加载数据
/// </summary>
public class ResLoadRequest
{
    /// <summary>
    /// 资源名称或路径
    /// </summary>
    public string assetName;

    /// <summary>
    /// 加载回调
    /// </summary>
    public List<ILoadListent> listents = new List<ILoadListent>();

    /// <summary>
    /// 加载到的资源
    /// </summary>
    public ResourceRequest request;

    /// <summary>
    ///     是否常驻内存
    /// </summary>
    public bool isKeepInMemory;

    /// <summary>
    ///     资源类型
    /// </summary>
    public Type type;

    /// <summary>
    /// 属性，加载到的资源
    /// </summary>
    public Object asset
    {
        get { return request != null && request.asset != null ? request.asset : null; }
    }

    /// <summary>
    /// 判断是否加载完成
    /// </summary>
    public bool isDone
    {
        get { return request != null && request.isDone; }
    }

    public ResLoadRequest(string assetName, bool isKeepInMemory, Type type)
    {
        this.assetName = assetName;
        this.isKeepInMemory = isKeepInMemory;
        this.type = type;
    }

    /// <summary>
    /// 添加一个回调到回调列表
    /// </summary>
    /// <param name="listent"></param>
    public void AddListent(ILoadListent listent)
    {
        if (listents == null || listents.Contains(listent)) return;
        listents.Add(listent);
    }

    /// <summary>
    /// 开始加载
    /// </summary>
    public void Load()
    {
        if (type == null)
        {
            type = typeof (GameObject);
        }
        request = Resources.LoadAsync(assetName, type);
    }
}