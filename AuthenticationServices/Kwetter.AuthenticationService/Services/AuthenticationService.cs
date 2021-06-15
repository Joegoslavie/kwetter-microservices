namespace Kwetter.AuthenticationService.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Kwetter.AuthenticationService.Persistence.Entity;
    using Kwetter.Messaging.Interfaces;
    using Kwetter.Messaging.Interfaces.Tweet;
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
        private readonly IProfileEvent profileEvent;

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
            IProfileEvent profileEvent)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.logger = logger;
            this.profileEvent = profileEvent;
        }

        /// <summary>
        /// Tries to sign in a user based on the given username and password.
        /// </summary>
        /// <param name="request">Sign in request.</param>
        /// <param name="context">Server context.</param>
        /// <returns><see cref="SignInResponse"/> that contains the status of the request and
        /// if succesfull, returns a generated JWT token.
        /// </returns>
        public override async Task<AuthenticationResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            var user = await this.userManager.FindByNameAsync(request.Username).ConfigureAwait(false);
            if (user == null)
            {
                var metadata = new Metadata
                {
                    { "Username", request.Username },
                };

                throw new RpcException(new Status(StatusCode.NotFound, "Username not found"), metadata);
            }

            if (!await this.userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Username and/or password incorrect"));
            }

            string generatedToken = this.GenerateToken(user.UserName, user.Id);
            if (string.IsNullOrEmpty(generatedToken))
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to generate token"));
            }

            return new AuthenticationResponse
            {
                Status = true,
                Account = new AccountResponse
                {
                    Id = user.Id,
                    Token = generatedToken,
                    Email = user.Email,
                    Username = user.UserName,
                },
            };
        }

        /// <summary>
        /// Tries to register an account in the Microsoft identity tables.
        /// </summary>
        /// <param name="request">Register request.</param>
        /// <param name="context">Server context.</param>
        /// <returns>The <see cref="ServerCallContext"/> with the status of the request.</returns>
        public override async Task<AuthenticationResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var user = await this.userManager.FindByNameAsync(request.Username).ConfigureAwait(false);
            if (user != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Username already exists."));
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
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.Errors.ToString()));
            }

            this.profileEvent.Invoke(newUser.Id, newUser.UserName, newUser.UserName, "default.jpg");
            return new AuthenticationResponse
            {
                Status = true,
                Account = new AccountResponse
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    Username = newUser.UserName,
                    Token = this.GenerateToken(newUser.UserName, newUser.Id),
                },
            };
        }

        private string GenerateToken(string username, int id)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Sid, id.ToString()),
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
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
