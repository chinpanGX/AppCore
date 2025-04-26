using AppCore.Runtime;
using UnityEngine;

namespace AppCore.Sample
{
    public class NextPresenter : IPresenter
    {
        private readonly IDirector director;
        private readonly NextModel model;
        private readonly NextView view;
        
        public NextPresenter(IDirector director, NextModel model, NextView view)
        {
            this.director = director;
            this.model = model;
            this.view = view;
            
            InitializeAsync();
        }
        
        private async void InitializeAsync()
        {
            view.AddButton.onClick.AddListener(OnButtonClick);
            view.BackButton.onClick.AddListener(() => _ = OnBackButtonClick());
            model.Initialize();
            
            view.SetButtonText($"{model.ClickCount}");
            
            await view.OpenAsync();
        }
        
        public void Tick()
        {
            model.Tick();
        }

        public void Dispose()
        {
            view.AddButton.onClick.RemoveListener(OnButtonClick);
            view.BackButton.onClick.RemoveListener(() => _ = OnBackButtonClick());
        }
        
        private void OnButtonClick()
        {
            model.ClickCount++;
            view.SetButtonText($"{model.ClickCount}");
        }
        
        private async Awaitable OnBackButtonClick()
        {
            await view.CloseAsync();
            await director.SwitchPresenterAsync("TopScreen");
        }
    }
}