using Holiday_Identifier.Models;

namespace Holiday_Identifier.Repositories.Interface
{
    public interface IHolidayRepository
    {
        DateInfo GetDateInfo(DateTime date);

        List<Holiday> GetAllHolidays(int year);

        bool UpdateHolidayRequest(string holidayName, DateTime newDate);

        IEnumerable<Holiday> GetPagedHolidaysByYear(int year, int pageNumber, int pageSize);
    }
}
