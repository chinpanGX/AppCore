using System.Threading.Tasks;
using AppCore.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace AppCore.Sample
{
    public class TopView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button button;
        public Canvas Canvas => canvas;
        public Button Button => button;

        private ViewScreen viewScreen;
        private AsyncOperationHandle<GameObject> handle;

        public static async Task<TopView> CreateAsync(ViewScreen viewScreen)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("TopView");
            await handle.Task;
            var instance = Instantiate(handle.Result);
            var comp = instance.GetComponent<TopView>();
            comp.Construct(viewScreen, handle);
            return comp;
        }

        private void Construct(ViewScreen viewScreen, AsyncOperationHandle<GameObject> handle)
        {
            this.viewScreen = viewScreen;
            this.handle = handle;
        }

        public async Awaitable OpenAsync()
        {
            viewScreen.Push(this);
            gameObject.SetActive(true);
            await Task.Yield();
        }

        public async Awaitable CloseAsync()
        {
            await Task.Yield();
            viewScreen.Pop(this);
            Destroy(gameObject);
        }

        public void SetTitle(string title)
        {
            this.title.text = title;
        }

        private void OnDestroy()
        {
            handle.Release();
        }
    }

}