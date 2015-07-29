using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainUICtrl : BaseUI
{
    public Button btnStart;
    public Button btnOptions;
    public Button btnAbout;
    public Canvas canvas;

    void Awake()
    {
        if (canvas)
        {
            canvas.worldCamera = Camera.main;
        }
        if (btnAbout)
        {
            btnAbout.onClick.AddListener(onClick);
        }
    }

    void onClick()
    {
        ViewManager.Instance.ShowView("MainOptions",new List<string>(){this.viewName},new LoadMainOptionsCallBack() );
    }

    public class LoadMainOptionsCallBack:IShowViewListener
    {
        public void Succeed(BaseUI baseUi)
        {
            throw new NotImplementedException();
        }

        public void Failed()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    public override void OnInitUI() { }

    /// <summary>
    /// UI的显示
    /// </summary>
    public override void OnShow() { }

    /// <summary>
    /// UI的隐藏
    /// </summary>
    public override void OnHide() { }

    /// <summary>
    /// UI的销毁
    /// </summary>
    public override void OnDestroy() { }
}
