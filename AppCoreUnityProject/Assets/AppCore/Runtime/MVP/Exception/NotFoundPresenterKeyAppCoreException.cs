namespace AppCore.Runtime
{
    public class NotFoundPresenterKeyAppCoreException : System.Exception
    {
        public NotFoundPresenterKeyAppCoreException(string key) : base($"Presenter with key '{key}' not found.")
        {
        }
    }
}