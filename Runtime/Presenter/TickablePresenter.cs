#nullable enable

namespace AppCore.Runtime
{
    public class TickablePresenter
    {
        private IPresenter? presenter;
        private IPresenter? request;

        public void Tick()
        {
            if (request != null)
            {
                presenter?.Dispose();
                presenter = request;
                request = null!;
            }

            presenter?.Tick();
        }

        public void SetRequest(IPresenter presenter)
        {
            request = presenter;
        }
    }
}