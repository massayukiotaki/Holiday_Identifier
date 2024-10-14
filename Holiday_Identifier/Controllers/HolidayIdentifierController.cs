using Holiday_Identifier.Models;
using Holiday_Identifier.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Holiday_Identifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayIdentifierController : Controller
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidayIdentifierController(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        [HttpPost("IdentifyDateInfo")]
        public ActionResult<DateInfo> Post([FromBody] DateRequest request)
        {
            var dateInfo = _holidayRepository.GetDateInfo(request.Date);
            return Ok(dateInfo);
        }

        [HttpGet("GetaAllHolidaysByYear")]
        public ActionResult<List<Holiday>> GetHolidays(int year)
        {
            var holidays = _holidayRepository.GetAllHolidays(year);
            return Ok(holidays);
        }
       
        [HttpPut("UpdateHolidayDate")]
        public IActionResult UpdateHolidayRequest([FromBody] UpdateHolidayRequest request)
        {
            bool isUpdated = _holidayRepository.UpdateHolidayRequest(request.HolidayName, request.NewDate);

            if (isUpdated)
            {
                var updatedHolidayResponse = new
                {
                    Name = request.HolidayName,
                    NewDate = request.NewDate.ToString("yyyy-MM-dd")
                };

                return Ok(updatedHolidayResponse);
            }

            return NotFound("Holiday not found.");
        }

        [HttpGet("GetPagedHolidaysByYear")]
        public IActionResult GetPagedHolidaysByYear(int year, int pageNumber, int pageSize)
        {
            var holidays = _holidayRepository.GetPagedHolidaysByYear(year, pageNumber, pageSize);

            if (!holidays.Any())
            {
                return NotFound(new { Message = "Error! Holidays not found, check the year!"});
            }

            return Ok(holidays);
        }
    }

}


