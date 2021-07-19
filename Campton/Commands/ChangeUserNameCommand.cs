using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Campton.Commands
{
    /// <summary>
    /// Команда для изменения имя пользователя
    /// </summary>
    public class ChangeUserNameCommand
    {
        [Required]
        public string Hash { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
