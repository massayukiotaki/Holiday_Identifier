using Holiday_Identifier.Models;

namespace Holiday_Identifier.Repositories.Interface
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly List<Holiday> _fixedHolidays;

        public HolidayRepository()
        {

            _fixedHolidays = new List<Holiday>
            {
                new Holiday { Name = "Ano Novo", Date = new DateTime(DateTime.Now.Year, 1, 1) },
                new Holiday { Name = "Tiradentes", Date = new DateTime(DateTime.Now.Year, 4, 21) },
                new Holiday { Name = "Dia do Trabalho", Date = new DateTime(DateTime.Now.Year, 5, 1) },
                new Holiday { Name = "Independência do Brasil", Date = new DateTime(DateTime.Now.Year, 9, 7) },
                new Holiday { Name = "Nossa Senhora Aparecida", Date = new DateTime(DateTime.Now.Year, 10, 12) },
                new Holiday { Name = "Finados", Date = new DateTime(DateTime.Now.Year, 11, 2) },
                new Holiday { Name = "Proclamação da República", Date = new DateTime(DateTime.Now.Year, 11, 15) },
                new Holiday { Name = "Natal", Date = new DateTime(DateTime.Now.Year, 12, 25) }
            };
        }


        public DateInfo GetDateInfo(DateTime date)
        {
            var dateInfo = new DateInfo
            {
                DayOfWeek = date.DayOfWeek.ToString(),
                IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
                Date = date.ToString("yyyy-MM-dd")
            };

           
            var fixedHoliday = _fixedHolidays.FirstOrDefault(h => h.Date.Month == date.Month && h.Date.Day == date.Day);
            if (fixedHoliday != null)
            {
                dateInfo.IsHoliday = true;
                dateInfo.HolidayName = fixedHoliday.Name;
                dateInfo.IsMovableHoliday = false;
            }

 
            var movableHoliday = GetMovableHoliday(date);
            if (movableHoliday != null)
            {
                dateInfo.IsHoliday = true;
                dateInfo.HolidayName = movableHoliday.Name;
                dateInfo.IsMovableHoliday = true;
            }

            return dateInfo;
        }

        
        private Holiday GetMovableHoliday(DateTime date)
        {
            var easter = GetEaster(date.Year);
            if (date.Date == easter.Date)
            {
                return new Holiday { Name = "Páscoa", Date = easter };
            }

            
            var goodFriday = easter.AddDays(-2);
            if (date.Date == goodFriday.Date)
            {
                return new Holiday { Name = "Sexta-feira Santa", Date = goodFriday };
            }

            
            var carnival = easter.AddDays(-47);
            if (date.Date == carnival.Date)
            {
                return new Holiday { Name = "Carnaval", Date = carnival };
            }

        
            var corpusChristi = easter.AddDays(60);
            if (date.Date == corpusChristi.Date)
            {
                return new Holiday { Name = "Corpus Christi", Date = corpusChristi };
            }

            return null;
        }

     
        private DateTime GetEaster(int year)
        {
            int month = 3;
            int g = year % 19;
            int c = year / 100;
            int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            int i = h - (h / 28) * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));
            int day = i - ((year + (year / 4) + i + 2 - c + (c / 4)) % 7) + 28;

            if (day > 31)
            {
                month = 4;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        
        public List<Holiday> GetAllHolidays(int year)
        {
            
            var holidays = _fixedHolidays.Select(h => new Holiday
            {
                Name = h.Name,
                Date = new DateTime(year, h.Date.Month, h.Date.Day)
            }).ToList();

            
            holidays.Add(new Holiday { Name = "Páscoa", Date = GetEaster(year) });
            holidays.Add(new Holiday { Name = "Sexta-feira Santa", Date = GetEaster(year).AddDays(-2) });
            holidays.Add(new Holiday { Name = "Carnaval", Date = GetEaster(year).AddDays(-47) });
            holidays.Add(new Holiday { Name = "Corpus Christi", Date = GetEaster(year).AddDays(60) });

            return holidays;
        }

        
        public bool UpdateHolidayRequest(string holidayName, DateTime newDate)
        {
            var holiday = _fixedHolidays.FirstOrDefault(h => h.Name.Equals(holidayName, StringComparison.OrdinalIgnoreCase));
            if (holiday != null)
            {
                
                holiday.Date = newDate;
                return true; 
            }
            return false; 
        }

        public IEnumerable<Holiday> GetPagedHolidaysByYear(int year, int pageNumber, int pageSize)
        {

            // Cria uma lista de feriados com o ano especificado
            var holidays = _fixedHolidays.Select(h => new Holiday
             {
               Name = h.Name,
               Date = new DateTime(year, h.Date.Month, h.Date.Day)
             }).ToList();

             // Adiciona feriados móveis
             holidays.Add(new Holiday { Name = "Páscoa", Date = GetEaster(year) });
             holidays.Add(new Holiday { Name = "Sexta-feira Santa", Date = GetEaster(year).AddDays(-2) });
             holidays.Add(new Holiday { Name = "Carnaval", Date = GetEaster(year).AddDays(-47) });
             holidays.Add(new Holiday { Name = "Corpus Christi", Date = GetEaster(year).AddDays(60) });

             
             return holidays.OrderBy(h => h.Date)
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize);

        }
    }
}
