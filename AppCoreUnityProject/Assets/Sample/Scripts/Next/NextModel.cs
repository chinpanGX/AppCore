using AppCore.Runtime;

namespace AppCore.Sample
{
    public class NextModel : IModel
    {
        public int ClickCount { get; set; } = 0;
        
        public void Initialize()
        {
            ClickCount = 0;
        }

        public void Tick()
        {
            // Update logic here
        }

        public void Dispose()
        {
            // Cleanup logic here
        }
    }
}