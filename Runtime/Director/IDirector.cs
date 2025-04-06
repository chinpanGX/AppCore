using UnityEngine;

namespace AppCore.Runtime
{
    public interface IDirector
    {
        Awaitable PushAsync(string key);
    }
}