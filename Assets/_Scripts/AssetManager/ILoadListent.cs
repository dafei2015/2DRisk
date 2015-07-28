/// <summary>
/// 资源加载完成后的回调
/// </summary>
public interface ILoadListent
{
    /// <summary>
    /// 加载成功
    /// </summary>
    /// <param name="asset">获得加载的资源</param>
    void Succeed(UnityEngine.Object asset);

    /// <summary>
    /// 加载失败
    /// </summary>
    void Failed();
}