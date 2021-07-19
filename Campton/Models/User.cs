using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campton.Models
{
    /// <summary>
    /// Модель пользователя с его данными
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Hash { get; set; }
        public long CreatedAt { get; set; }
        public long LastLoginAt { get; set; }
        public int? RecoveryCode { get; set; }
        public bool Deleted { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
    }
}
