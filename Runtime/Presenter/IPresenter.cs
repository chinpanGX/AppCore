using System;

namespace AppCore.Runtime
{
    public interface IPresenter : IDisposable
    {
        void Tick();
    }
}