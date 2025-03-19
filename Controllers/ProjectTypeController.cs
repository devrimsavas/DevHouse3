using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DevHouse1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevHouse1.Controllers
{

    /// <summary>
    /// Controller for managing project types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTypeController"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing project type data.</param>

        public ProjectTypeController(AppDbContext context)
        {
            _context = context;
        }

        // Get all projecttypes
        /// <summary>
        /// Retrieves all project types.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     GET /api/ProjectType
        /// 
        /// </remarks>
        /// <response code="200">Project types retrieved successfully</response>
        /// <response code="404">No project types found</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProjectType>>> GetProjectTypes()
        {
            if (_context.ProjectTypes == null)
            {
                return NotFound();
            }
            var projectTypes = await _context.ProjectTypes.ToListAsync();
            return Ok(projectTypes);
        }


        //Get a project type 
        /// <summary>
        /// Retrieves a project type by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     GET /api/ProjectType/1
        /// 
        /// **Example Response:**
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "name": "Web Development"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Project type found</response>
        /// <response code="404">Project type not found</response>




        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProjectType>> GetProjectType(int Id)
        {
            if (_context.ProjectTypes == null)
            {
                return NotFound(new { Message = "The projecttype you search is not found" });
            }
            var projectType = await _context.ProjectTypes.FindAsync(Id);
            if (projectType == null)
            {
                return NotFound(new { Message = "Project Type is not found" });
            }
            return Ok(projectType);

        }

        //add a project type 
        /// <summary>
        /// Creates a new project type.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     POST /api/ProjectType
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "name": "Web Development"
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Project type created successfully</response>
        /// <response code="400">Project type already exists</response>

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]

        public async Task<ActionResult<ProjectType>> AddProjectType([FromBody] ProjectType projectType)
        {
            var existedProjectType = await _context.ProjectTypes.FirstOrDefaultAsync(p => p.Name == projectType.Name);
            if (existedProjectType != null)
            {
                return BadRequest(new { Message = $"this project type with name {projectType.Name} already exist" });
            }
            await _context.ProjectTypes.AddAsync(projectType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjectType), new { Id = projectType.Id }, projectType);
        }

        //update a projecttype 
        /// <summary>
        /// Updates an existing project type.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     PUT /api/ProjectType/updateprojecttype/1
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "name": "Updated Project Type"
        /// }
        /// ```
        /// </remarks>
        /// <response code="204">Project type updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Project type not found</response>




        [HttpPut("updateprojecttype/{Id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectType>> UpdateProjectType(int Id, ProjectType projectType)
        {
            if (Id != projectType.Id)
            {
                return BadRequest();
            }
            _context.Update(projectType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTypeExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent();

        }

        //helper for update
        private bool ProjectTypeExists(int id)
        {
            return (_context.ProjectTypes?.Any(ProjectType => ProjectType.Id == id)).GetValueOrDefault();
        }


        //delete /id 
        /// <summary>
        /// Deletes a project type by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     DELETE /api/ProjectType/1
        /// 
        /// </remarks>
        /// <response code="204">Project type successfully deleted</response>
        /// <response code="404">Project type not found</response>

        [HttpDelete("{Id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProjectType>> DeleteProjectType(int Id)
        {
            if (_context.ProjectTypes == null)
            {
                return NotFound();
            }
            var existedProject = await _context.ProjectTypes.FindAsync(Id);
            if (existedProject is null)
            {
                return NotFound(new { Message = "The Project Type is not found" });
            }
            //delete
            _context.ProjectTypes.Remove(existedProject);
            await _context.SaveChangesAsync();
            return NoContent();
        }






    }
}
