using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teether.OperationalObjects;

namespace Teether.Controllers
{
    [ApiController]
    [Route("visit")]
    public class VisitController(MainDbContext dbContext, UserManager<User> userManager) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<StatusCodeResult> Create([FromBody] VisitForCreating visitForCreating)
        {
            var patient = await userManager.FindByNameAsync(visitForCreating.PatientUserName);
            var doctor = await userManager.FindByNameAsync(visitForCreating.DoctorUserName);

            dbContext.Visits.Add(new()
            {
                Date = visitForCreating.Date,
                Time = visitForCreating.Time,
                Patient = patient,
                Doctor = doctor
            });

            await dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("history/{patientUserName}/{date}")]
        public ActionResult<List<Visit>> History([FromRoute] string patientUserName, [FromRoute] DateOnly date)
        {
            return dbContext.Visits.Where(v => v.Patient.UserName == patientUserName && v.Date == date)
                            .Include(v => v.Patient)
                            .Include(v => v.Doctor)
                            .ToList();
        }

        [HttpPost("search/{doctorFirstName}/{doctorLastName}/{doctorPatronymic}/{date}/{time}")]
        public ActionResult<bool> Search([FromRoute] string doctorFirstName, [FromRoute] string doctorLastName,
                                            [FromRoute] string doctorPatronymic, [FromRoute] DateOnly date,
                                         [FromRoute] TimeOnly time)
        {
            return !dbContext.Visits.Any(v => v.Doctor.FirstName == doctorFirstName &&
                                             v.Doctor.LastName == doctorLastName &&
                                             v.Doctor.Patronymic == doctorPatronymic && v.Date == date && v.Time == time);
        }
    }
}
