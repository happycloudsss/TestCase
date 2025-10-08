using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TestCase.Entities;

namespace TestCase.Services
{
    public class AuthRedirectService
    {
        public static bool IsUserAuthenticated(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookie = httpContext.Request.Cookies["auth_user"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    try
                    {
                        var userJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookie));
                        var user = JsonSerializer.Deserialize<User>(userJson);
                        return user != null;
                    }
                    catch (Exception ex)
                    {
                        // 如果解析失败，认为用户未认证
                        // 记录异常但不抛出，避免影响应用程序运行
                        Console.WriteLine($"Error deserializing user from cookie: {ex.Message}");
                    }
                }
            }
            return false;
        }
    }
}