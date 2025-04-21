using System.Threading.Tasks;
using AppCore.Runtime;
using UnityEngine;

public class TopView : MonoBehaviour, IView
{
    [SerializeField] private Canvas canvas;
    public Canvas Canvas => canvas;
    private ViewScreen ViewScreen => ServiceLocator.Get<ViewScreen>();
    
    public static TopView Create()
    {
        var prefab = Resources.Load<TopView>("TopView");
        var instance = Instantiate(prefab);
        return instance;
    }

    public async Awaitable OpenAsync()
    {
        ViewScreen.Push(this);
        gameObject.SetActive(true);
        await Task.Yield();
    }

    public async Awaitable CloseAsync()
    {
        await Task.Yield();
        ViewScreen.Pop(this);
        Destroy(gameObject);
    }
}