using UnityEngine;

public class BaseUI:MonoBehaviour
{
    private Transform mCacheTransform;
    private GameObject mCacheGameObject;
    public Canvas uiCanvas;
    public string viewName = string.Empty; //当前界面名称

    #region 组件缓存

    public Transform CacheTransform
    {
        get
        {
            if (mCacheTransform != null) return mCacheTransform;
            mCacheTransform = this.transform;
            return mCacheTransform;
        }
    }

    public GameObject CacheGameObject
    {
        get
        {
            if (mCacheGameObject != null) return mCacheGameObject;
            mCacheGameObject = this.gameObject;
            return mCacheGameObject;
        }
    }

    #endregion


    public void InitUI()
    {
        if (uiCanvas != null)
        {
            uiCanvas.worldCamera = Camera.main;
        }
        OnInitUI();
    }

    public void Show()
    {
        this.mCacheGameObject.SetActive(true);
        OnShow();
    }

    public void Hide()
    {
        this.mCacheGameObject.SetActive(false);
        OnHide();
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    public virtual void OnInitUI() { }

    /// <summary>
    /// UI的显示
    /// </summary>
    public virtual void OnShow() { }

    /// <summary>
    /// UI的隐藏
    /// </summary>
    public virtual void OnHide() { }

    /// <summary>
    /// UI的销毁
    /// </summary>
    public virtual void OnDestroy() { }

}