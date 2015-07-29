/// <summary>
/// 成功打开界面后的回调函数
/// </summary>
public interface IShowViewListener
{
    void Succeed(BaseUI baseUi);
    void Failed();
}