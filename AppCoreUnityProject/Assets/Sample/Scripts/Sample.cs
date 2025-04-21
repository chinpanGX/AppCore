using System.Threading.Tasks;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private async void Start()
    {
        var topView = TopView.Create();
        await topView.OpenAsync();
        await Task.Delay(2000);
        await topView.CloseAsync();
    }
}