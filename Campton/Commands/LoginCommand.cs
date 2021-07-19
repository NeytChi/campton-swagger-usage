using System.ComponentModel.DataAnnotations;

namespace Campton.Commands
{
    /// <summary>
    /// Команда для логина пользователя
    /// </summary>
    public class LoginCommand
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
