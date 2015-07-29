using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载界面回调
/// </summary>
public class LoadViewCallBack:ILoadListent
{
    
    public enum LoadViewType { Create,Show}

    public LoadViewType loadType = LoadViewType.Show;
    public string viewName;
    public List<IShowViewListener> showViewListeners = new List<IShowViewListener>();

    public LoadViewCallBack(string viewName , LoadViewType type)
    {
        this.viewName = viewName;
        this.loadType = type;
    }

    public void AddShowViewListener(IShowViewListener listener)
    {
        if (!showViewListeners.Contains(listener))
        {
            showViewListeners.Add(listener);
        }
    }
    public void Succeed(Object asset)
    {
        GameObject target = GameObject.Instantiate(asset) as GameObject;
        target.name = asset.name;
        target.gameObject.SetActive(false);
        BaseUI view = target.GetComponent<BaseUI>();
        if (view != null && loadType == LoadViewType.Show)
        {
            view.InitUI();
            view.Show();
        }
        else if (view != null && loadType == LoadViewType.Create)
        {
            view.InitUI();
        }
        view.viewName = viewName; //修改名字

        for (int i = 0; i < showViewListeners.Count; i++)
        {
            if (showViewListeners[i] != null)
            {
                showViewListeners[i].Succeed(view);
            }
        }
        ViewManager.Instance.RemoveLoadIns(this);
        ViewManager.Instance.AddViewToDic(viewName, view);
    }

    public void Failed()
    {
        ViewManager.Instance.RemoveLoadIns(this);
    }
}