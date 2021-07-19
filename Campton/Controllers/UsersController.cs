using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Campton.Commands;
using Campton.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Campton.Controllers
{
    /// <summary>
    /// Контреллер для работы с пользователем и его профайлом.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        public Context _context;
        public ProfileCondition _profileCondition = new();
        public ILogger<UsersController> _logger;

        /// <summary>
        /// Создаем контроллер в котором будем использовать базу данных и логгирование
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public UsersController(Context context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Метод для регистрации пользователя.
        /// <remarks> Результат : { "success": true } </remarks>
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Registration(RegistrationCommand command)
        {
            var message = string.Empty;

            // Поиск пользователя по Емайлу
            if (_context.Users.FirstOrDefault(u => u.Email == command.Email) != null)
            {
                // Возврат ошибки 500 если был уже найден такой пользователь
                message = "This email is already exists.";
                _logger.LogWarning(message);
                return StatusCode(500, new MessageResponse(false, message));
            }
            // Создаем пользователя и возвращаем результат 200 ОК
            var user = new User
            {
                Email = command.Email,
                Name = command.Name,
                Password = _profileCondition.HashPassword(command.Password), // Хешируем пароль чтобы в БД хранить только сам хеш пароля
                Hash = _profileCondition.CreateHash(100), // Создаем хеш по которому будем определять пользователя
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), // Сохраняем время создания пользователя в формате Unix
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            _logger.LogInformation("Create new user, id ->" + user.Id);
            return Ok(new SuccessResponse(true));
        }
        /// <summary>
        /// Метод для логина пользователя. 
        /// <remarks> Результат :
        /// {
        ///   "id": 1,
        ///   "email": "email@email",
        ///   "name": "1string",
        ///   "lastLoginAt": 1626422451,
        ///   "createdAt": 1626422390,
        ///   "hash": "1234"
        /// }
        /// </remarks>
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Login(LoginCommand command)
        {
            string message; User user;
            // Поиск пользователя по емайлу
            if ((user = _context.Users.FirstOrDefault(u => u.Email == command.Email)) != null)
            {
                // Проверяем пользователя пароль
                if (_profileCondition.VerifyHashedPassword(user.Password, command.Password))
                {
                    // Обновляем дату последнего входа и возвращаем ответ с данными пользователя
                    user.LastLoginAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    _logger.LogInformation($"Login user, id -> {user.Id}.");
                    return Ok(new LoginDto(user));
                }
                message = "Wrong password.";
            }
            else
            {
                message = "No user with such email.";
            }

            _logger.LogWarning(message);
            var response = new MessageResponse(false, message);
            return StatusCode(500, response);
        }
        /// <summary>
        /// Метод для изменения имени пользователя
        /// <remarks> Результат : { "success": true }
        /// </remarks>
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ChangeUserName(ChangeUserNameCommand command)
        {
            var message = ""; User user;

            // Поиск пользователя по хешу, иначе ошибка что невозможно определить пользователя
            if ((user = _context.Users.FirstOrDefault(u => u.Hash == command.Hash)) == null)
            {
                message = "Unknown auth hash.";
                _logger.LogWarning(message);
                return StatusCode(500, new MessageResponse(false, message));
            }
            user.Name = command.UserName;
            _context.Update(user);
            _context.SaveChanges();
            _logger.LogInformation($"Change user name, id -> {user.Id}.");
            return Ok(new SuccessResponse(true));
        }
    }
}
