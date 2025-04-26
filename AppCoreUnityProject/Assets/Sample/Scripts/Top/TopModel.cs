using AppCore.Runtime;
using UnityEngine;

namespace AppCore.Sample
{
    public class TopModel : IModel
    {
        public string Title { get; private set; }
        
        public void Initialize()
        {
            Title = "Top Screen";
        }
        
        public void Tick()
        {
            Debug.Log("Model Ticking...");
        }
        
        public void Dispose()
        {
            Debug.Log("Model Disposing...");
        }
    }
}