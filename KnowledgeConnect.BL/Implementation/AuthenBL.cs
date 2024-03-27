using Database.Services;
using KnowledgeConnect.Model;
using KnowledgeConnect.Model.Authen;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.BL
{
    public class AuthenBL : BaseBL, IAuthenBL
    {
        private IServiceScopeFactory _serviceScopeFactory;

        private IConfiguration _configuration;
        public AuthenBL(IConfiguration configuration, IDatabaseService databaseService, IServiceScopeFactory serviceScopeFactory) : base(configuration ,databaseService)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }


        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse> Login(LoginRequest loginRequest)
        {
            var response = new ServiceResponse();
            if (String.IsNullOrEmpty(loginRequest.UserName) || String.IsNullOrEmpty(loginRequest.Password))
            {
                response.OnError();
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userBL = scope.ServiceProvider.GetService<IUserBL>();
                    var userCredential = ((await userBL.GetPagingAsync(typeof(User), new PagingRequest()
                    {
                        PageIndex = 1,
                        PageSize = 1,
                        Filter = $"[[\"UserName\", \"=\", \"{loginRequest.UserName}\"], \"and\", [\"Password\", \"=\", \"{HashPassword(loginRequest.Password)}\"]]"
                    })).PageData as List<UserCredential>).FirstOrDefault();

                    if (userCredential == null)
                    {
                        response.OnError(userMessage: "User name or password is invalid");
                    }
                    else
                    {
                        var user = await userBL.GetByIDAsync(typeof(User), userCredential.UserID.ToString());
                        var token = BuildToken(_configuration["Jwt:Key"], "", null, user);
                        response.OnSuccess(new { Token = token });

                    }

                }
                response.Success = true;
            }
            return response;
            throw new NotImplementedException();
        }

        private string BuildToken(string? v1, string v2, object value, object user)
        {
            //TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hàm băm password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert password string to byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute hash value of the password
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                // Convert hashed bytes to string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2")); // Convert byte to hexadecimal string
                }
                return builder.ToString();
            }
        }

        public Task<ServiceResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetInfoFromToken(string? v)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
