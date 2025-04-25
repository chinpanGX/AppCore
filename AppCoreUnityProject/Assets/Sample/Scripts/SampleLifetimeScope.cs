using AppCore.Runtime;
using AppCore.Sample;
using UnityEngine;

public class SampleDirector : MonoBehaviour, IDirector
{
    private ViewScreen viewScreen;
    private TickablePresenter tickablePresenter;
    
    private async void Start()
    {
        viewScreen = ServiceLocator.Get<ViewScreen>();
        tickablePresenter = new TickablePresenter();
        
        await SwitchPresenterAsync("TopScreen");
    }
    
    private void Update()
    {
        tickablePresenter.Tick();
    }
    
    public async Awaitable SwitchPresenterAsync(string key)
    {
        IPresenter requestPresenter = key switch
        {
            "TopScreen" => new TopPresenter(this, new TopModel(), await TopView.CreateAsync(viewScreen)),
            "NextScreen" => new NextPresenter(this, new NextModel(), await NextView.CreateAsync(viewScreen)),
            _ => throw new NotFoundPresenterKeyAppCoreException(key)
        };
        tickablePresenter.SetRequest(requestPresenter);
    }
}