public interface ICanvas
{
    void Initialize();
    void CleanUp();
    void Hide(System.Action callback = null);
    bool isActiveAndEnabled { get; }
}
