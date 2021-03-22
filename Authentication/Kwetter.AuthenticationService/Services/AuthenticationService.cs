namespace Kwetter.AuthenticationService.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    // https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-5-with-jwt-and-swagger/

    /// <summary>
    /// Service class that contains functions related to authentication.
    /// </summary>
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        /// <summary>
        /// Logger interface for the authentication service.
        /// </summary>
        private readonly ILogger<AuthenticationService> logger;

        /// <summary>
        /// The dotnet user manager class.
        /// </summary>
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="userManager">Injected user manager.</param>
        /// <param name="configuration">Injected configuration.</param>
        /// <param name="logger">Injected logger.</param>
        public AuthenticationService(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthenticationService> logger)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Tries to sign in a user based on the given username and password.
        /// </summary>
        /// <param name="request">Sign in request.</param>
        /// <param name="context">Server context.</param>
        /// <returns><see cref="SignInResponse"/> that contains the status of the request and
        /// if succesfull, returns a generated JWT token.
        /// </returns>
        public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            var user = await this.userManager.FindByNameAsync(request.Username).ConfigureAwait(false);
            if (user != null && await this.userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: this.configuration["JWT:ValidIssuer"],
                    audience: this.configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                // return token.
                return new SignInResponse
                {
                    Status = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };
            }

            await Task.Delay(10);
            return null;
        }

        /// <summary>
        /// Tries to register
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            await Task.Delay(10);
            return null;
        }
    }
}
