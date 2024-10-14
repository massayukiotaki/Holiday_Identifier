using Holiday_Identifier.Models;

namespace Holiday_Identifier.Repositories.Interface
{
    public interface IHolidayRepository
    {
        DateInfo GetDateInfo(DateTime date);

        List<Holiday> GetAllHolidays(int year);

        bool UpdateHolidayDate(string holidayName, DateTime newDate);
    }
}
