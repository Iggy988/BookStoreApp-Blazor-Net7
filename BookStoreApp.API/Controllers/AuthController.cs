using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        _logger.LogInformation($"Registration Attempt for {userDto.Email}");
        var user = _mapper.Map<ApiUser>(userDto);
        user.UserName = userDto.Email;
        var result = await _userManager.CreateAsync(user, userDto.Password);

        try
        {
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "User");
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }

    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
        _logger.LogInformation($"Login Attempt for {userDto.Email}");

        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (user is null || passwordValid == false)
            {
                return NotFound();
            }
            return Accepted();

            
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Something went wrong in the {nameof(Login)}");
            return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
        }
    }
}
