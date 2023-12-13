using AutoMapper;
using Forum_Application_API.Dto;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Forum_Application_API.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class CommentController : Controller
    {

        private readonly IThreadInterface _threadInterface;
        private readonly IUserInterface _userInterface;
        private readonly ICommentInterface _commentInterface;
        private readonly IMapper _mapper;
        public CommentController(IThreadInterface threadInterface, IUserInterface userInterface, ICommentInterface commentInterface, IMapper mapper)
        {
            _commentInterface = commentInterface;
            _threadInterface = threadInterface;
            _userInterface = userInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        public IActionResult GetComments()
        {
            var comments = _mapper.Map<List<Comment>>(_commentInterface.GetComments());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(comments);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]

        public IActionResult GetCommentsByUserId(int userId)
        {
            if (!_userInterface.UserExists(userId))
            {
                return NotFound();
            }

            var comments = _mapper.Map<List<Comment>>(_commentInterface.GetCommentsByUser(userId));
            var updatedComments = comments.Select(comment =>
            {
                comment.User = _userInterface.GetUser(comment.UserId);
                return comment;
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(updatedComments);
        }


        [HttpGet("/thread/{threadId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentsByThreadId(int threadId)
        {
            if (!_threadInterface.ThreadExists(threadId))
            {
                return NotFound();
            }

            var comments = _mapper.Map<List<Comment>>(_commentInterface.GetCommentsByThread(threadId));
            var updatedComments = comments.Select(comment =>
            {
                comment.User = _userInterface.GetUser(comment.UserId);
                return comment;
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(updatedComments);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateThreadComment([FromQuery] int userId, [FromQuery] int threadId, [FromBody] CommentDto commentCreate)
        {

            if (commentCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentMap = _mapper.Map<Comment>(commentCreate);
            commentMap.CreatedDate = DateTime.UtcNow;

            var user = _userInterface.GetUser(userId);

            var thread = _threadInterface.GetThread(threadId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            if (thread == null)
            {
                ModelState.AddModelError("", "Thread does not exists");
                return StatusCode(422, ModelState);
            }

            commentMap.User = _userInterface.GetUser(userId);
            commentMap.Thread = _threadInterface.GetThread(threadId);

            if (!_commentInterface.CreateComment(commentMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating Comment");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        //UPDATE
        [HttpPut("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateComment(int commentId, [FromQuery] int threadId, [FromQuery] int userId, [FromBody] CommentDto updatedComment)
        {
            if (updatedComment == null) return BadRequest(ModelState);

            if (commentId != updatedComment.Id) return BadRequest(ModelState);

            //Deleted threads will delete all comments in it already
            var user = _userInterface.GetUser(userId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            if (!_commentInterface.CommentExists(commentId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var commentMap = _mapper.Map<Comment>(updatedComment);
            commentMap.UserId = userId;
            commentMap.ThreadId = threadId;
            commentMap.CreatedDate = DateTime.UtcNow;

            if (!_commentInterface.UpdateComment(commentMap))
            {
                ModelState.AddModelError("", "Something went wrong when updating Comment");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated");
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(int commentId, [FromQuery] int userId)
        {
            if (!_commentInterface.CommentExists(commentId))
            {
                return NotFound();
            }

            var user = _userInterface.GetUser(userId);

            if (user == null)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            var commentToDelete = _commentInterface.GetComment(commentId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_commentInterface.DeleteComment(commentToDelete))
            {
                ModelState.AddModelError("", "Somethiing went wrong with deleting Comments");
            }

            return Ok("Successfully Removed");

        }
    }
}
