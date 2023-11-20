using AuthVideo.Domain.AuthModels;
using AuthVideo.Domain.Entities;
using AuthVideo.Domain.InfrastructureInterfaces;
using AuthVideo.Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthVideo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public TokenController(IRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client response");
            }

            string accessToken = tokenModel.AccessToken
                ?? throw new ArgumentNullException();
            string refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentNullException();

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal?.Identity?.Name;
            var user = _userRepository.Get(username 
                ?? throw new ArgumentNullException());

            if (user is null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client response");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal?.Claims ?? throw new ArgumentNullException());
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _userRepository.UpdateEntity(user);

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
