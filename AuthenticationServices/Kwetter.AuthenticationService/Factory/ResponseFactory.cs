namespace Kwetter.AuthenticationService.Factory
{
    /// <summary>
    /// Factory for <see cref="Services.AuthenticationService"/> responses.
    /// </summary>
    public class ResponseFactory
    {
        /// <summary>
        /// Create a register success response with the given message.
        /// </summary>
        /// <param name="responseMessage">The message to return.</param>
        /// <returns><see cref="RegisterResponse"/>.</returns>
        public static RegisterResponse RegisterSuccessfull(string responseMessage)
        {
            return new RegisterResponse
            {
                Status = true,
            };
        }

        /// <summary>
        /// Create a register failure response with the given message.
        /// </summary>
        /// <param name="errorMessage">The message to return.</param>
        /// <returns><see cref="RegisterResponse"/>.</returns>
        public static RegisterResponse RegisterFailure(string errorMessage)
        {
            return new RegisterResponse
            {
                Status = false,
            };
        }

        /// <summary>
        /// Create a sign in success response with the generated token.
        /// </summary>
        /// <param name="jwtToken">Generated JWT token.</param>
        /// <returns><see cref="SignInResponse"/>.</returns>
        public static SignInResponse SignInSuccessfull(string jwtToken)
        {
            return new SignInResponse
            {
                Status = true,
                Token = jwtToken,
            };
        }

        /// <summary>
        /// Create a sign in failure response.
        /// </summary>
        /// <returns><see cref="SignInResponse"/>.</returns>
        public static SignInResponse SignInFailure()
        {
            return new SignInResponse
            {
                Status = false,
            };
        }
    }
}
