
namespace Campton.Commands
{
    /// <summary>
    /// Модель ответа в контреллерах
    /// </summary>
    public class MessageResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public MessageResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
