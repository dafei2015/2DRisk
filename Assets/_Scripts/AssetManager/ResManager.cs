using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     资源加载管理类，全局都可以使用，所有做成单例
/// </summary>
public class ResManager : MonoBehaviour
{
    /// <summary>
    /// ResManager的实例
    /// </summary>
    private static ResManager mInstance;

    /// <summary>
    /// 获取当前处理器数量，实现异步加载
    /// </summary>
    private static int mProcessCount = 0;

    /// <summary>
    ///     所有加载完成的字典
    /// </summary>
    private Dictionary<string, AssetPack> mAssetPacksDic = new Dictionary<string, AssetPack>();

    /// <summary>
    ///     当前正在加载的队列
    /// </summary>
    private List<ResLoadRequest> mLoadList = new List<ResLoadRequest>();

    /// <summary>
    /// 等待加载的队列
    /// </summary>
    private Queue<ResLoadRequest> mWaitLoads = new Queue<ResLoadRequest>();

    /// <summary>
    /// 单例模式的应用
    /// </summary>
    public static ResManager Instance
    {
        get
        {
            if (mInstance != null) return mInstance;
            var obj = new GameObject("ResManager");
            mInstance = obj.AddComponent<ResManager>();
            mProcessCount = SystemInfo.processorCount;
            mProcessCount = mProcessCount < 1 ? 1 : mProcessCount;
            mProcessCount = mProcessCount > 8 ? 8 : mProcessCount;
            return mInstance;
        }
    }

    /// <summary>
    /// 模拟NGUI的缓存机制，可以大大降低内存的消耗
    /// </summary>
    private GameObject mGameObject;

    public GameObject CachedGameObject
    {
        get { return mGameObject ?? (mGameObject = this.gameObject); }
    }

    /// <summary>
    /// 资源管理类一直存在
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(CachedGameObject);
    }

    private void Update()
    {
        if (mLoadList.Count > 0)
        {
            ResLoadRequest ins = null;
            List<ResLoadRequest> listRemove = new List<ResLoadRequest>();
            for (int i = 0; i < mLoadList.Count; i++)
            {
                ins = mLoadList[i];
                if (ins.isDone)
                {
                    listRemove.Add(ins);
                    LoadFinish(ins);
                }
            }

            //加载完成后直接从加载列表中排除
            for (int i = 0; i < listRemove.Count; i++)
            {
                mLoadList.Remove(listRemove[i]);
            }
        }

        ResLoadRequest request = null;
        while (mLoadList.Count < mProcessCount && mWaitLoads.Count > 0)
        {
            request = mWaitLoads.Dequeue();
            mLoadList.Add(request);
            request.Load();
        }
    }

    /// <summary>
    /// 资源加载完毕
    /// </summary>
    /// <param name="request"></param>
    private void LoadFinish(ResLoadRequest request)
    {
        if (request != null)
        {
            ILoadListent listen = null;
            for (int i = 0; i < request.listents.Count; i++)
            {
                listen = request.listents[i];
                if (request.asset != null)
                {
                    listen.Succeed(request.asset);
                }
                else
                {
                    listen.Failed();
                }
            }
        }
    }

    /// <summary>
    /// 从Resource加载一个资源
    /// </summary>
    /// <param name="prefabName">资源名称</param>
    /// <param name="type">类型</param>
    /// <param name="callBack">加载完成后的回调</param>
    /// <param name="isKeepInMemory">是否常驻内存</param>
    public void LoadAssetAsync(string prefabName, Type type, ILoadListent callBack, bool isKeepInMemory = false)
    {
        LoadAsset(prefabName, type, callBack, isKeepInMemory);
    }

    private void LoadAsset(string prefabName, Type type, ILoadListent callBack, bool isKeepInMemory)
    {
        if (string.IsNullOrEmpty(prefabName))
        {
            if (callBack != null) callBack.Failed();
            return;
        }
        //如果缓存中包含此资源包则直接返回
        if (mAssetPacksDic.ContainsKey(prefabName))
        {
            if (mAssetPacksDic[prefabName].asset == null)
            {
                if (callBack != null)
                {
                    callBack.Failed();
                }
            }
            else
            {
                callBack.Succeed(mAssetPacksDic[prefabName].asset);
            }
            return; //如果找到就不在往下执行
        }

        //如果不包含，则先判断当前正在加载的列表中是否包含，如果包含则直接把回调事件加入
        for (int i = 0; i < mLoadList.Count; i++)
        {
            ResLoadRequest request = mLoadList[i];
            if (!request.assetName.Equals(prefabName)) continue;
            request.AddListent(callBack);
            return;
        }

        foreach (ResLoadRequest request in mWaitLoads)
        {
            if (!request.assetName.Equals(prefabName)) continue;
            request.AddListent(callBack);
            return;
        }

        //如果都不存在的话，则加载进等待队列
        ResLoadRequest loadRequest = new ResLoadRequest(prefabName, isKeepInMemory, type);
        loadRequest.listents.Add(callBack);
        mWaitLoads.Enqueue(loadRequest);
    }

    /// <summary>
    ///     从资源管理中释放一个资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="isRemove">是否可以释放</param>
    public void Remove(string assetName, bool isRemove = false)
    {
        if (!mAssetPacksDic.ContainsKey(assetName)) return;

        if (mAssetPacksDic[assetName].isKeepInMemory)
        {
            if (isRemove)
            {
                mAssetPacksDic[assetName] = null;
                mAssetPacksDic.Remove(assetName);
            }
        }
        else
        {
            mAssetPacksDic[assetName] = null;
            mAssetPacksDic.Remove(assetName);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    ///     清空所有资源
    /// </summary>
    public void RemoveAll()
    {
        foreach (var pair in mAssetPacksDic)
        {
            mAssetPacksDic[pair.Key] = null;
        }
        mAssetPacksDic.Clear();
        Resources.UnloadUnusedAssets();
    }
}