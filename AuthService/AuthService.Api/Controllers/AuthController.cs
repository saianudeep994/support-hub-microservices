using AuthService.Application.DTOs;
using AuthService.Application.Services.Abstrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userservice;
        public AuthController(ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _userservice = userService;
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userservice.GetAllUsers();
            return Ok(users);
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthService.Application.DTOs.LoginDTO loginDTO)
        {
            var user = _userservice.LoginUser(loginDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized("Invalid email or password.");
        }
        [HttpGet("Getuserbymail")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userservice.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found.");
        }

        [HttpPost("signup")]
        public IActionResult Register([FromBody] SignUpDTO signUpDTO)
        {
            var user = _userservice.RegisterUser(signUpDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("User registration failed.");
        }
        [HttpGet("getuserbyid")]
        public IActionResult GetUserById(int id) 
        {
            return Ok("Anudeep");
        }
        [HttpGet("getuserbyids")]
        public IActionResult GetUsersByIds([FromQuery] int[] ids)
        {
            GetNameDTO p1 = new();
            List<GetNameDTO> people = [
                    new GetNameDTO
                    { Id = 1, Name = "Ada" },
                    new GetNameDTO
                    { Id = 2, Name = "Anudeep" }
                ];


            return Ok(people);
        }
    }
}
