using System;

namespace AppCore.Runtime
{
    public interface IModel : IDisposable
    {
        void Initialize();
        void Tick();
    }
}
