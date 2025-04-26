using UnityEngine;

namespace AppCore.Runtime
{
    public interface IView
    {
        Canvas Canvas { get; }
        Awaitable OpenAsync();
        Awaitable CloseAsync();
    }
}