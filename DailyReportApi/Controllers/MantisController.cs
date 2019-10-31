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
    public class MantisController : ControllerBase
    {
        private readonly MyContext _myContext;

        public MantisController(MyContext context)
        {
            _myContext = context;
            if (_myContext.MantisItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                //_myContext.MantisItems.Add(
                //    new Mantis
                //    {
                //        MantisNumber = "123456",
                //        Date = DateTime.Today
                //    });
                //_myContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mantis>>> GetTodoItems()
        {
            if (_myContext.MantisItems.Any())
                return await _myContext.MantisItems.ToListAsync();
            else
                return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mantis>> GetTodoItem(int id)
        {
            var todoItem = await _myContext.MantisItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<Mantis>> PostItem(Mantis item)
        {
            if (item == null)
                return BadRequest();
            if (_myContext.MantisExist(item.Id))
                return BadRequest("Mantis id already exist!");

            _myContext.MantisItems.Add(item);
            await _myContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Mantis item)
        {
            if (item == null)
                return BadRequest();
            if (id != item.Id)
                return BadRequest();
            if (!_myContext.MantisExist(id))
                return NotFound();

            _myContext.Entry(item).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var mantis = await _myContext.MantisItems.FindAsync(id);

            if (mantis == null)
            {
                return NotFound();
            }

            _myContext.MantisItems.Remove(mantis);
            await _myContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
