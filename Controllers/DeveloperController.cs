using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DevHouse1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevHouse1.Controllers
{

    /// <summary>
    /// Controller for managing developers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperController"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing developer data.</param>

        public DeveloperController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all developers.
        /// </summary>


        //  Get all developers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Developer>>> GetDevelopers()
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }
            var developers = await _context.Developers.ToListAsync();
            return Ok(developers);
        }

        //  Get a single developer by ID

        /// <summary>
        /// Retrieves one developer by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     GET /api/Developer/1
        /// 
        /// **Example Response:**
        /// 
        /// ```json
        /// {
        ///   "firstname": "John",
        ///   "lastname": "Doe",
        ///   "roleId": 2,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Developer found</response>
        /// <response code="404">Developer not found</response>

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Developer>> GetDeveloper(int Id)
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }
            var developer = await _context.Developers.FindAsync(Id);
            if (developer == null)
            {
                return NotFound(new { Message = "Developer not found" });
            }
            return Ok(developer);
        }

        /// <summary>
        /// Creates a new developer.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     POST /api/Developer
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "firstname": "John",
        ///   "lastname": "Doe",
        ///   "roleId": 2,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Developer created successfully</response>
        /// <response code="400">Invalid developer data</response>

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Developer>> AddDeveloper([FromBody] Developer developer)
        {
            if (developer == null)
            {
                return BadRequest(new { Message = "Invalid developer data" });
            }

            // Ensure the related Team and Role exist
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == developer.TeamId);
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == developer.RoleId);

            if (!teamExists)
            {
                return BadRequest(new { Message = "Invalid TeamId. Team does not exist." });
            }
            if (!roleExists)
            {
                return BadRequest(new { Message = "Invalid RoleId. Role does not exist." });
            }

            await _context.Developers.AddAsync(developer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeveloper), new { Id = developer.Id }, developer);
        }

        /// <summary>
        /// Updates an existing developer.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     PUT /api/Developer/1
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "firstname": "John",
        ///   "lastname": "Doe",
        ///   "roleId": 1,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Developer updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Developer not found</response>

        [HttpPut("{Id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Developer>> UpdateDeveloper(int Id, [FromBody] Developer updatedDeveloper)
        {
            if (updatedDeveloper == null)
            {
                return BadRequest(new { Message = "Invalid request. Provide developer details." });
            }

            var existingDeveloper = await _context.Developers.FindAsync(Id);
            if (existingDeveloper == null)
            {
                return NotFound(new { Message = "Developer not found" });
            }

            // Update only provided fields
            existingDeveloper.Firstname = updatedDeveloper.Firstname ?? existingDeveloper.Firstname;
            existingDeveloper.Lastname = updatedDeveloper.Lastname ?? existingDeveloper.Lastname;
            existingDeveloper.TeamId = updatedDeveloper.TeamId;
            existingDeveloper.RoleId = updatedDeveloper.RoleId;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Developer updated successfully" });
        }

        //  Delete a developer

        /// <summary>
        /// Deletes a developer by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     DELETE /api/Developer/1
        /// 
        /// </remarks>
        /// <response code="204">Developer successfully deleted</response>
        /// <response code="404">Developer not found</response>

        [HttpDelete("{Id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDeveloper(int Id)
        {
            if (_context.Developers == null)
            {
                return NotFound();
            }

            var developer = await _context.Developers.FindAsync(Id);
            if (developer == null)
            {
                return NotFound(new { Message = "Developer not found" });
            }

            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
