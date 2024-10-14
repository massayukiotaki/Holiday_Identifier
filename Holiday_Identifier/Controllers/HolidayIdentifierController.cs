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

        [HttpPost]
        public ActionResult<DateInfo> Post([FromBody] DateTime date)
        {
            var dateInfo = _holidayRepository.GetDateInfo(date);
            return Ok(dateInfo);
        }

        [HttpGet("GetaAllHolidaysByYear{year}")]
        public ActionResult<List<Holiday>> GetHolidays(int year)
        {
            var holidays = _holidayRepository.GetAllHolidays(year);
            return Ok(holidays);
        }



       
        [HttpPut("UpdateHolidayDate")]
        public IActionResult UpdateHolidayDate([FromBody] UpdateHolidayRequest request)
        {
            bool isUpdated = _holidayRepository.UpdateHolidayDate(request.HolidayName, request.NewDate);

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
        
        public class UpdateHolidayRequest
        {
            public string HolidayName { get; set; }
            public DateTime NewDate { get; set; }
        }
    }

}


