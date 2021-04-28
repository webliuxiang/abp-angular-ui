using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Account;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public class AuthorizeApi : IAuthorizeApi
    {
        private readonly HttpClient _httpClient;

        public AuthorizeApi(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("IAuthorizeApi");
            _httpClient.BaseAddress = new Uri("http://localhost:6096/");
        }

        public async Task Login(LoginViewModel loginModel)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(loginParameters), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsJsonAsync("api/Authorize/Login", loginModel);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            result.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/Authorize/Logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterViewModel registerParameters)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(registerParameters), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsJsonAsync("api/Authorize/Register", registerParameters);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task<UserListDto> GetUserInfo()
        {
            var result = await _httpClient.GetFromJsonAsync<UserListDto>("api/Authorize/UserInfo");
            return result;
        }
    }
}
