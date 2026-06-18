using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TmsApi.Data;
using Microsoft.EntityFrameworkCore;
namespace RegistrarController;
[ApiController]
[Route("api/registrar")]
public class RegistrarController(TmsDbContext context) : ControllerBase
{

[HttpGet("active-gpa-gt-3")]
public async Task<IActionResult> ActiveGpaGT3()
    {
        var count = await context.Students.Where(s => s.IsActive && s.GPA >= 3.0m).CountAsync();
        return Ok(count);
    }
[HttpGet("mostenrollmentsdescending")]
public async Task<IActionResult> MostEnrollmentsDescending()
    {
        var list = await context.Courses
.Select(c => new
{
c.Title,
EnrollmentCount = c.Enrollments.Count
})
.OrderByDescending(x => x.EnrollmentCount)
.ToListAsync();

return Ok(list);
    }
[HttpGet("avg-gpa-percourse")]
public async Task<IActionResult> AvgGpaPerCourse()
    {
        var list = await context.Enrollments
.GroupBy(e => e.Course.Title)
.Select(g => new
{
Course = g.Key,
AverageGPA = g.Average(e => e.Student.GPA)
})
.ToListAsync();

return Ok(list);
    }

[HttpGet("zero-enrollments")]
public async Task<IActionResult> ZeroEnrollments()
    {
        var list = await context.Students
.Where(s => !s.Enrollments.Any())
.Select(s => s.Name)
.ToListAsync();
return Ok(list);
    }
}

