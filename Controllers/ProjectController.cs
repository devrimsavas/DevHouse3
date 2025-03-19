using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DevHouse1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevHouse1.Controllers
{

    /// <summary>
    /// Controller for managing projects.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing project data.</param>

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        //  Get all projects
        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     GET /api/Project
        /// 
        /// </remarks>
        /// <response code="200">Projects retrieved successfully</response>
        /// <response code="404">No projects found</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Project>>> GetAllProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }

        //  Get a single project by ID
        /// <summary>
        /// Retrieves a project by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     GET /api/Project/1
        /// 
        /// **Example Response:**
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "name": "Website Redesign",
        ///   "projectTypeId": 2,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Project found</response>
        /// <response code="404">Project not found</response>

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Project>> GetProject(int Id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found" });
            }
            return Ok(project);
        }

        //  Create a new project
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     POST /api/Project
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "name": "Website Redesign",
        ///   "projectTypeId": 2,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Project created successfully</response>
        /// <response code="400">Invalid project data</response>

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Project>> AddProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest(new { Message = "Invalid project data" });
            }

            //  Fetch the actual Team and ProjectType from the database
            var team = await _context.Teams.FindAsync(project.TeamId);
            var projectType = await _context.ProjectTypes.FindAsync(project.ProjectTypeId);

            if (team == null)
            {
                return BadRequest(new { Message = "Invalid TeamId. Team does not exist." });
            }
            if (projectType == null)
            {
                return BadRequest(new { Message = "Invalid ProjectTypeId. Project Type does not exist." });
            }

            //  Assign the fetched objects to the project (Prevents EF validation errors)
            project.Team = team;
            project.ProjectType = projectType;

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { Id = project.Id }, project);
        }

        //  Update an existing project
        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     PUT /api/Project/1
        /// 
        /// **Example Body:**
        /// 
        /// ```json
        /// {
        ///   "name": "Website Redesign Updated",
        ///   "projectTypeId": 2,
        ///   "teamId": 1
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Project updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Project not found</response>
        [HttpPut("{Id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Project>> UpdateProject(int Id, [FromBody] Project updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest(new { Message = "Invalid request. Provide project details." });
            }

            var existingProject = await _context.Projects.FindAsync(Id);
            if (existingProject == null)
            {
                return NotFound(new { Message = "Project not found" });
            }

            // Update only provided fields
            existingProject.Name = updatedProject.Name ?? existingProject.Name;
            existingProject.TeamId = updatedProject.TeamId;
            existingProject.ProjectTypeId = updatedProject.ProjectTypeId;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Project updated successfully" });
        }

        //  Delete a project
        /// <summary>
        /// Deletes a project by ID.
        /// </summary>
        /// <remarks>
        /// **Sample Request:**
        /// 
        ///     DELETE /api/Project/1
        /// 
        /// </remarks>
        /// <response code="204">Project successfully deleted</response>
        /// <response code="404">Project not found</response>
        [HttpDelete("{Id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteProject(int Id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found" });
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
