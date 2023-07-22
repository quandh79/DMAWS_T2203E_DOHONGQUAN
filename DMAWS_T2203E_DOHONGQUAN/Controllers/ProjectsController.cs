using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMAWS_T2203E_DOHONGQUAN.Models;

namespace DMAWS_T2203E_DOHONGQUAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProjectsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
          if (_context.Projects == null)
          {
              return Problem("Entity set 'DataContext.Projects'  is null.");
          }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }

        [HttpGet("search")]
        public IActionResult Search(string projectName, bool? inProgress = null)
        {
            IQueryable<Project> query = _context.Projects;

            if (!string.IsNullOrEmpty(projectName))
            {
                query = query.Where(p => p.ProjectName.Contains(projectName));
            }

            if (inProgress.HasValue)
            {
                DateTime currentDate = DateTime.Now;
                if (inProgress.Value)
                {
                    query = query.Where(p => p.ProjectEndDate == null || p.ProjectEndDate > currentDate);
                }
                else
                {
                    query = query.Where(p => p.ProjectEndDate != null && p.ProjectEndDate <= currentDate);
                }
            }

            var projects = query.ToList();
            return Ok(projects);
        }

        [HttpGet("{id}")]
public IActionResult GetProjectDetails(int id)
{
    var project = _context.Projects
        .Include(p => p.ProjectEmployees)
        .ThenInclude(pe => pe.Employee)
        .FirstOrDefault(p => p.ProjectId == id);

    if (project == null)
    {
        return NotFound();
    }

    return Ok(project);
}
    }
}
