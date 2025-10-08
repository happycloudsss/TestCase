using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TestCase.Entities;

namespace TestCase.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        
        public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookie = httpContext.Request.Cookies["auth_user"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    try
                    {
                        var userJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookie));
                        var user = JsonSerializer.Deserialize<User>(userJson);
                        
                        if (user != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Name),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("EmployeeId", user.EmployeeId)
                            };

                            // 如果是管理员，添加管理员角色
                            if (user.EmployeeId == "admin")
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            }
                            else
                            {
                                claims.Add(new Claim(ClaimTypes.Role, "User"));
                            }

                            var identity = new ClaimsIdentity(claims, "apiauth");
                            var userPrincipal = new ClaimsPrincipal(identity);
                            return new AuthenticationState(userPrincipal);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 如果解析失败，返回未认证状态
                        // 记录异常但不抛出，避免影响应用程序运行
                        Console.WriteLine($"Error deserializing user from cookie: {ex.Message}");
                    }
                }
            }

            return new AuthenticationState(_anonymous);
        }

        public async Task Login(User user)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var userJson = JsonSerializer.Serialize(user);
                var userBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(userJson));
                
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = false, // 在生产环境中应设置为true（部署到HTTPS时）
                    SameSite = SameSiteMode.Strict
                };
                
                httpContext.Response.Cookies.Append("auth_user", userBase64, cookieOptions);
            }
            
            await NotifyAuthenticationStateChangedAsync();
        }

        public async Task Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Delete("auth_user");
            }
            
            await NotifyAuthenticationStateChangedAsync();
        }

        public async Task NotifyAuthenticationStateChangedAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
    }
}