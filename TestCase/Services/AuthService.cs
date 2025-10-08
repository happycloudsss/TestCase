using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TestCase.Entities;

namespace TestCase.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public (bool IsAuthenticated, User? User) ValidateCredentials(string username, string password)
        {
            // 检查是否为管理员账户
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase) && password == "admin")
            {
                // 创建一个虚拟的管理员用户
                var adminUser = new User
                {
                    Id = 0,
                    EmployeeId = "admin",
                    Name = "系统管理员",
                    Email = "admin@system.local"
                };
                return (true, adminUser);
            }

            // 查找用户（通过工号、姓名或邮箱）
            var user = _userService.GetUserByEmployeeId(username);
            
            if (user == null)
            {
                // 尝试通过姓名查找
                var usersByName = _userService.GetUsersByName(username);
                user = usersByName.FirstOrDefault();
            }
            
            if (user == null)
            {
                // 尝试通过邮箱查找
                var users = _userService.GetAllUsers();
                user = users.FirstOrDefault(u => u.Email.Equals(username, StringComparison.OrdinalIgnoreCase));
            }

            // 用户存在即认证成功（因为我们不需要密码验证）
            if (user != null)
            {
                return (true, user);
            }

            return (false, null);
        }
    }
}