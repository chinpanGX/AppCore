using AppCore.Runtime;
using UnityEngine;

namespace AppCore.Sample
{
    public class TopPresenter : IPresenter
    {
        private readonly IDirector director;
        private readonly TopModel model;
        private readonly TopView view;
        
        public TopPresenter(IDirector director, TopModel model, TopView view)
        {
            this.director = director;
            this.model = model;
            this.view = view;
            
            InitializeAsync();
        }
        
        private async void InitializeAsync()
        {
            view.Button.onClick.AddListener(() => _ = OnButtonClick());
            model.Initialize();
            
            view.SetTitle(model.Title);

            await view.OpenAsync();
        }
        
        public void Tick()
        {
            model.Tick();
        }
        
        public void Dispose()
        {
            view.Button.onClick.RemoveListener(() => _ = OnButtonClick());
            model.Dispose();
        }
        
        private async Awaitable OnButtonClick()
        {
            Debug.Log("Button Clicked!");
            await view.CloseAsync();
            await director.SwitchPresenterAsync("NextScreen");
        }
    }

}