using UnityEngine;
using UnityEngine.EventSystems;

public class CommonUtil
{
    /// <summary>
    /// 获取一个UI事件系统
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static EventTrigger Get(GameObject go)
    {
        EventTrigger eventTriger = go.GetComponent<EventTrigger>();
        if (eventTriger == null) eventTriger = go.AddComponent<EventTrigger>();
        return eventTriger;
    }
}