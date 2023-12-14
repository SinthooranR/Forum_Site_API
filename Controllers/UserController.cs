using AutoMapper;
using Forum_Application_API.Dto;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Methods;
using Forum_Application_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Forum_Application_API.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserInterface _userInterface;
        private readonly IThreadInterface _threadInterface;
        private readonly ICommentInterface _commentInterface;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        public UserController(IUserInterface userInterface, IThreadInterface threadInterface, ICommentInterface commentInterface, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, JwtGenerator jwtGenerator)
        {
            _userInterface = userInterface;
            _threadInterface = threadInterface;
            _commentInterface = commentInterface;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

       /* [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<User>>(_userInterface.GetUsers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }*/

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]

        //THIS RETURNS A SINGLE ITEM
        public IActionResult GetUser(int userId)
        {
            if (!_userInterface.UserExists(userId))
            {
                return NotFound();
            }

            var user = _mapper.Map<User>(_userInterface.GetUser(userId));
            var threads = _threadInterface.GetThreadsByUser(userId);
            var comments = _commentInterface.GetCommentsByThread(userId);
            var newUser = _mapper.Map<SecureUserDto>(user);
            newUser.Threads = threads.Select(thread => _mapper.Map<UserForumDto>(thread)).ToList();
            newUser.Comments = comments.Select(comment => _mapper.Map<UserCommentDto>(comment)).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(newUser);
        }


        /*[HttpGet("{userId}/threads")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult GetThreadsByUser(int userId)
        {
            if (!_userInterface.UserExists(userId))
            {
                return NotFound();
            }

            var threads = _mapper.Map<List<ThreadDto>>(_userInterface.GetThreadsByUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(threads);
        }*/

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest(ModelState);
            }

        var user = _userInterface.GetUsers()
            .FirstOrDefault(u => u.Email != null && u.Email.ToUpper() == userCreate.Email.ToUpper());

            var userIDExists = _userInterface.GetUsers().Where(u => u.Id == userCreate.Id).FirstOrDefault();

            if (user != null || userIDExists != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userMap = new User
            {
                UserName = userCreate.Email, 
                Email = userCreate.Email,
                FirstName = userCreate.FirstName,
                LastName = userCreate.LastName,
                CreatedDate = DateTime.UtcNow
        };
        
            var result = await _userManager.CreateAsync(userMap, userCreate.Password);

            if (result.Succeeded)
            {
                return Ok("Successfully Created");
            }

            else
            {
                return BadRequest(result.Errors);
            }

            /*if (!_userInterface.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating User");
                return StatusCode(500, ModelState);
            }*/


        }


        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto userEntered)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userEntered.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userEntered.Password))
            {
                return Unauthorized();
            }


            /*
            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = _jwtGenerator.GenerateToken(user);
            return Ok(new { Token = token });
            */

            var token = _jwtGenerator.GenerateToken(user);

            // Set HttpOnly cookie
            Response.Cookies.Append("token", token, new CookieOptions
            {
                SameSite = SameSiteMode.None, // or SameSiteMode.Strict
                Secure = true,
                Path = "/",
            });

            return Ok("Logged In Successfully");
        }



        //UPDATE
        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updatedUser)
        {
            var userIdString = userId.ToString();
            var user = await _userManager.FindByIdAsync(userIdString);

            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.CreatedDate = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok("Successfully Updated");
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            else
            {
                ModelState.AddModelError("", "No such User");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int userId)
        {
            if (!_userInterface.UserExists(userId))
            {
                return NotFound();
            }

            var userToDelete = _userInterface.GetUser(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userInterface.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Somethiing went wrong with deleting User");
            }

            return NoContent();
        }
    }
}
