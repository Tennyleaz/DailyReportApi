using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyReportApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly MyContext _myContext;

        public ProjectController(MyContext context)
        {
            _myContext = context;
            if (_myContext.ProjectItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                //_myContext.ProjectItems.Add(
                //    new Project
                //    {
                //        ProjectName = "test project",
                //        Version = "1.0.0"
                //    });
                //_myContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetTodoItems()
        {
            return await _myContext.ProjectItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetTodoItem(int id)
        {
            var todoItem = await _myContext.ProjectItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostItem(Project item)
        {
            if (item == null)
                return BadRequest();
            if (_myContext.ProjectExist(item.Id))
                return BadRequest("Project id already exist!");

            _myContext.ProjectItems.Add(item);
            await _myContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = item.Id }, item);
        }

        [HttpPost("TryAdd")]
        public async Task<ActionResult<Project>> TryAddProject(Project item)
        {
            if (item == null)
                return BadRequest();

            // find similar one
            var query = _myContext.ProjectItems.Where(r => r.ProjectName == item.ProjectName && r.Version == item.Version);
            int count = await query.CountAsync();
            if (count > 0)
            {
                Project existingProject = await query.FirstAsync();
                return existingProject;
            }

            // need reset id first
            item.Id = 0;
            _myContext.ProjectItems.Add(item);
            await _myContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Project item)
        {
            if (item == null)
                return BadRequest();
            if (id != item.Id)
                return BadRequest();
            if (!_myContext.ProjectExist(id))
                return NotFound();

            _myContext.Entry(item).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var project = await _myContext.ProjectItems.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _myContext.ProjectItems.Remove(project);
            await _myContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
