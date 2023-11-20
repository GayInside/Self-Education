using AuthVideo.Domain.AuthModels;
using AuthVideo.Domain.Entities;
using AuthVideo.Domain.EntitiesModels;
using AuthVideo.Domain.InfrastructureInterfaces;
using AuthVideo.Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public HomeController(IRepository<User> userRepository, ITokenService tokenService, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] SignInModel? reqUser)
        {
            if (reqUser is null)
            {
                return BadRequest("Invalid client response");
            }

            var user = _userRepository.Get(reqUser.Login ?? throw new ArgumentNullException());

            if (user is null ||
                user.PasswordHash != _passwordService.GetHashedPassword(reqUser.Password ?? throw new ArgumentNullException()))
            {
                return Unauthorized("Incorrect login or password");
            }

            string role = (user.Role == UserRole.Default) ? "user" : "admin";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, user.Login ?? throw new ArgumentNullException())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _userRepository.UpdateEntity(user);

            return Ok(
                new TokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            );
        }
    }
}
