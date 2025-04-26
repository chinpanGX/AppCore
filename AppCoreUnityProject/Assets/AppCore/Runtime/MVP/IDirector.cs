using UnityEngine;

namespace AppCore.Runtime
{
    public interface IDirector
    {
        Awaitable SwitchPresenterAsync(string key);
    }

}