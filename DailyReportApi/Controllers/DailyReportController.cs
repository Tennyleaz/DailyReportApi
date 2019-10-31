using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DailyReportApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyReportApi.Controllers
{
    // For this sample, the controller class name is TodoController, so the controller name is "todo".
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportController : ControllerBase
    {
        private readonly MyContext _myContext;

        public DailyReportController(MyContext context)
        {
            _myContext = context;
            if (_myContext.DailyReportItems.Count() == 0)
            {
                // Create a new DailyReport if collection is empty,
                // which means you can't delete all DailyReports.
                //_myContext.DailyReportItems.Add(
                //    new DailyReport
                //    {
                //        Date = DateTime.Today,
                //        Message = "This is a test report.",
                //        ProjectId = 1
                //    });
                //_myContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyReport>>> GetDailyReports()
        {
            return await _myContext.DailyReportItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DailyReport>> GetDailyReport(int id)
        {
            var DailyReport = await _myContext.DailyReportItems.FindAsync(id);

            if (DailyReport == null)
                return NotFound();

            return DailyReport;
        }

        [HttpGet("{startDate}/{endDate}")]
        public async Task<ActionResult<List<DailyReport>>> GetDailyReportByDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                return BadRequest();

            var query = _myContext.DailyReportItems.Where(x => x.Date >= startDate && x.Date <= endDate);
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DailyReport>> PostItem(DailyReport item)
        {
            if (item == null)
                return BadRequest();
            if (!_myContext.ProjectExist(item.ProjectId))
                return NotFound();
            if (_myContext.DailyReportExist(item.Id))
                return BadRequest("DailyReport id already exist!");

            _myContext.DailyReportItems.Add(item);
            await _myContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDailyReport), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, DailyReport item)
        {
            if (item == null)
                return BadRequest();
            if (id != item.Id)
                return BadRequest();
            if (!_myContext.DailyReportExist(item.Id))
                return NotFound();
            if (!_myContext.ProjectExist(item.ProjectId))
                return NotFound();

            _myContext.Entry(item).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();

            return NoContent();
        }

        /*[HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<DailyReport> patchBody)
        {
            if (patchBody == null)
                return BadRequest();

            if (!_myContext.DailyReportExist(id))
                return NotFound();

            throw new NotImplementedException();
        }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var DailyReport = await _myContext.DailyReportItems.FindAsync(id);

            if (DailyReport == null)
            {
                return NotFound();
            }

            _myContext.DailyReportItems.Remove(DailyReport);
            await _myContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
