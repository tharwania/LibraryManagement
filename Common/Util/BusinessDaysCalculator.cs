using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class BusinessDaysCalculator
    {
        public int GetBusinessDaysBetweenCount(DateTime startDate, DateTime endDate)
        {

            int totalDays = (endDate - startDate).Days;

            if(totalDays <= 0)
            {
                return 0;
            }

            int businessDays = 0;
            for(int day = 0; day < totalDays; day++)
            {
               if(startDate.AddDays(day).DayOfWeek != DayOfWeek.Sunday &&
                    startDate.AddDays(day).DayOfWeek != DayOfWeek.Saturday)
                {
                    businessDays++;
                }
            }

            return businessDays;
        }

        public DateTime GetDateAfterBusinessDays(DateTime dateTime, int count)
        {
            int businessDays = 0;
            int daysPast = 0;
            while(businessDays != count)
            {
                if (dateTime.AddDays(daysPast).DayOfWeek != DayOfWeek.Sunday &&
                    dateTime.AddDays(daysPast).DayOfWeek != DayOfWeek.Saturday)
                {
                    businessDays++;
                }

                daysPast++;
            }

            return dateTime.AddDays(daysPast);
        }
    }
}
