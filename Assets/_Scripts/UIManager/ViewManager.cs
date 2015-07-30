using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ViewManager:MonoBehaviour
{
    private static ViewManager mInstance;

    public static ViewManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this);
    }
    /// <summary>
    /// 将界面进行缓存
    /// </summary>
    private Dictionary<string ,BaseUI> mDicView = new Dictionary<string, BaseUI>();

    /// <summary>
    /// 添加一个界面到缓存字典中
    /// </summary>
    /// <param name="viewName">界面名字</param>
    /// <param name="view">界面</param>
    public void AddViewToDic(string viewName, BaseUI view)
    {
        if (mDicView.ContainsKey(viewName))
        {
            mDicView[viewName] = view;
        }
        else
        {
            mDicView.Add(viewName,view);
        }
    }
    /// <summary>
    /// 正在加载的列表
    /// </summary>
    private List<LoadViewCallBack> mLoadIns = new List<LoadViewCallBack>(); 
    
    /// <summary>
    /// 打开一个界面，同时关闭其他的界面
    /// </summary>
    /// <param name="viewName">需要打开的界面</param>
    /// <param name="hideList">需要关闭的界面列表</param>
    /// <param name="listener">回调函数</param>
    public void ShowView(string viewName, List<string> hideViewList , IShowViewListener listener, bool createAndShow = false )
    {
         //如果缓存中存在对应UI,则显示UI并调用对应的函数
        BaseUI baseUI = null;
        for (int i = 0; i < hideViewList.Count; i++)
        {
            if (mDicView.TryGetValue(viewName, out baseUI) && baseUI != null)
            {
                baseUI.Hide();
            }
        }
       
        if (mDicView.TryGetValue(viewName, out baseUI))
        {
            if (baseUI != null)
            {
                baseUI.Show();
                if (listener != null)
                {

                    listener.Succeed(baseUI);
                }
            }
            else
            {
                mDicView.Remove(viewName);
            }
            return;
        }

        for (int i = 0; i <mLoadIns.Count ; i++)
        {
            if (mLoadIns[i].viewName.Equals(viewName))
            {
                mLoadIns[i].AddShowViewListener(listener);
                return;
            }
        }
        LoadViewCallBack callBack = new LoadViewCallBack(viewName,LoadViewCallBack.LoadViewType.Show);
        if (createAndShow)
        {
            callBack.loadType = LoadViewCallBack.LoadViewType.Create;
        }
        callBack.AddShowViewListener(listener);
        CreateView(callBack);
    }

    /// <summary>
    /// 创建一个界面
    /// </summary>
    /// <param name="callBack"></param>
    private void CreateView(LoadViewCallBack callBack)
    {
        if (callBack == null)
        {
            return;
        }
        mLoadIns.Add(callBack);
        ResManager.Instance.LoadAssetAsync(callBack.viewName,typeof(GameObject),callBack,true);
    }

    
    /// <summary>
    /// 去除一个当前正在加载的界面
    /// </summary>
    /// <param name="loadViewCallBack"></param>
    public void RemoveLoadIns(LoadViewCallBack loadViewCallBack)
    {
        if (mLoadIns.Contains(loadViewCallBack))
        {
            mLoadIns.Remove(loadViewCallBack);
        }
    }

    /// <summary>
    /// 隐藏view
    /// </summary>
    /// <param name="viewName"></param>
    public void HideView(string viewName)
    {
        for (int i = 0; i < mLoadIns.Count; i++)
        {
            if (mLoadIns[i].viewName.Equals(viewName))
            {
                mLoadIns[i].loadType = LoadViewCallBack.LoadViewType.Create;
                return;
            }
        }
        BaseUI baseUI = null;
        if (mDicView.TryGetValue(viewName, out baseUI))
        {
            if (baseUI != null)
            {
                baseUI.Hide();
            }
            else if (baseUI == null)
            {
                mDicView.Remove(viewName);
            }
        }
    }

    /// <summary>
    /// 销毁view
    /// </summary>
    /// <param name="viewName">界面名称</param>
    public void DestroyView(string viewName)
    {
        BaseUI baseUI = null;
        if (mDicView.TryGetValue(viewName, out baseUI))
        {
            if (baseUI != null)
            {
                Destroy(baseUI.CacheGameObject);
                mDicView.Remove(viewName);
            }
        }
    }
}