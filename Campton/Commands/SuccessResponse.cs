namespace Campton.Commands
{
    /// <summary>
    /// Модель ответа на запрос в контроллере
    /// </summary>
    public class SuccessResponse
    {
        public bool Success { get; set; }
        public SuccessResponse(bool success)
        {
            Success = success;
        }
    }
}
