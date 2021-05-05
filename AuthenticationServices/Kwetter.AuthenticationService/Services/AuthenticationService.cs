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
    using Kwetter.AuthenticationService.Factory;
    using Kwetter.AuthenticationService.Persistence.Entity;
    using Kwetter.Messaging.Events;
    using Kwetter.Messaging.Interfaces;
    using Microservice.AuthGRPCService;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// Service class that contains functions related to authentication.
    /// </summary>
    public class AuthenticationService : AuthGRPCService.AuthGRPCServiceBase
    {
        /// <summary>
        /// Logger interface for the authentication service.
        /// </summary>
        private readonly ILogger<AuthenticationService> logger;

        /// <summary>
        /// The dotnet user manager class.
        /// </summary>
        private readonly UserManager<KwetterUserEntity<int>> userManager;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProfileEvent createProfileEvent;

        /// <summary>
        /// Interface for reading the configuration file.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="userManager">Injected user manager.</param>
        /// <param name="configuration">Injected configuration.</param>
        /// <param name="logger">Injected logger.</param>
        public AuthenticationService(
            UserManager<KwetterUserEntity<int>> userManager,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger,
            NewProfileEvent profileEvent)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.logger = logger;
            this.createProfileEvent = profileEvent;
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
                return ResponseFactory.SignInSuccessfull(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return ResponseFactory.SignInFailure();
        }

        /// <summary>
        /// Tries to register an account in the Microsoft identity tables.
        /// </summary>
        /// <param name="request">Register request.</param>
        /// <param name="context">Server context.</param>
        /// <returns>The <see cref="ServerCallContext"/> with the status of the request.</returns>
        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var user = await this.userManager.FindByNameAsync(request.Username).ConfigureAwait(false);
            if (user != null)
            {
                return ResponseFactory.RegisterFailure("Username already exists!");
            }

            var newUser = new KwetterUserEntity<int>
            {
                UserName = request.Username,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await this.userManager.CreateAsync(newUser, request.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return ResponseFactory.RegisterFailure("Failed to create user!");
            }

            this.createProfileEvent.Invoke(newUser.Id, newUser.UserName, string.Empty);
            return ResponseFactory.RegisterSuccessfull("Registration succesfull");
        }
    }
}
