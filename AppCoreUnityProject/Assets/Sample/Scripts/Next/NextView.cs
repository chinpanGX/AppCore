using System.Threading.Tasks;
using AppCore.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace AppCore.Sample
{
    public class NextView : MonoBehaviour, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button addButton;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button backButton;
        
        public Button AddButton => addButton;
        public Button BackButton => backButton;
        
        public Canvas Canvas => canvas;
        private ViewScreen viewScreen;
        private AsyncOperationHandle<GameObject> handle;
        
        public static async Task<NextView> CreateAsync(ViewScreen viewScreen)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("NextView");
            await handle.Task;
            var instance = Instantiate(handle.Result);
            var comp = instance.GetComponent<NextView>();
            comp.Construct(viewScreen, handle);
            return comp;
        }
        
        private void Construct(ViewScreen viewScreen, AsyncOperationHandle<GameObject> asyncOperationHandle)
        {
            this.viewScreen = viewScreen;
            handle = asyncOperationHandle;
        }
        
        public async Awaitable OpenAsync()
        {
            viewScreen.Push(this);
            gameObject.SetActive(true);
            await Task.Yield();
        }
        
        public async Awaitable CloseAsync()
        {
            viewScreen.Pop(this);
            Destroy(gameObject);
            await Task.Yield();
        }
        
        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
        
        private void OnDestroy()
        {
            handle.Release();
        }
    }
}