namespace GreenCorner.MVC.Services.Interface
{
    public interface IChatService
    {
        Task<string> GetGeminiResponseAsync(string message);
    }
}
