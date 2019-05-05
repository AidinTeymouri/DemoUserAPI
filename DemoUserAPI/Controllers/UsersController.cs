using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoUserAPI.Models;

namespace DemoUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Database context
        private readonly DataContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UsersController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of users
        /// GET: api/Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.Include(x => x.Address);
        }

        /// <summary>
        /// Returns a specific user by ID
        /// GET: api/Users/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] long id)
        {
            // Find user
            var user = _context.Users.Include(x => x.Address).SingleOrDefault(x => x.ID == id);

            // If user does not exist
            if (user == null)
                return NotFound();

            // Return user
            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user
        /// PUT: api/Users/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] long id, [FromBody] User user)
        {
            // Ensure ID on user object matches ID from route
            if (id != user.ID)
                return BadRequest();

            // Mark the user object as modified
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // Perform any database changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If user to update does not exist
                if (!UserExists(id))
                    return NotFound();

                throw;
            }

            // Return successful response
            return NoContent();
        }

        /// <summary>
        /// Creates a new user
        /// POST: api/Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            // Add user to collection
            _context.Users.Add(user);

            // Update database
            await _context.SaveChangesAsync();

            // Get created user
            var createdUser = _context.Users.Include(x => x.Address).SingleOrDefault(x => x.ID == user.ID);

            // Return created user 
            return Ok(createdUser);
        }

        /// <summary>
        /// Deletes an existing user
        /// DELETE: api/Users/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            // Find user to delete
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Delete user
            _context.Users.Remove(user);

            // Update database
            await _context.SaveChangesAsync();

            // Return deleted user
            return Ok(user);
        }

        /// <summary>
        /// Check if a user already exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}