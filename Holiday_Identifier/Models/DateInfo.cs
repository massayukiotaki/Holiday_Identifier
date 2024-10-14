namespace Holiday_Identifier.Models
{
    public class DateInfo
    {
        public string DayOfWeek { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public bool IsMovableHoliday { get; set; }
        public string Date { get; set; }
    }

}
