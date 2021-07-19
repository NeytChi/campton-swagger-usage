using System.ComponentModel.DataAnnotations;

namespace Campton.Commands
{
    /// <summary>
    /// Команда для регистрации пользователя
    /// </summary>
    public class RegistrationCommand
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
