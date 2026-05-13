using Microsoft.AspNetCore.Mvc;
using University.Api.Application;

namespace University.Api.Controllers;

[ApiController]
[Route("api/students")]
public sealed class StudentsController(StudentEnrollmentService enrollmentService) : ControllerBase
{
    [HttpPost("credits")]
    public ActionResult<int> GetTotalCredits([FromBody] List<Course> courses)
    {
        var total = courses.Sum(x => x.Credits); 
        return Ok(total);
    }
}