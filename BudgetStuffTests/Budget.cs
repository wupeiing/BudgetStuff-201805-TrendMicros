using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetStuffTests
{
    public class Budget
    {

        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int getDailyAmount()
        {
            int daysOfBudgetMonth = DateTime.DaysInMonth(Convert.ToInt32(YearMonth.Substring(0, 4)), Convert.ToInt32(YearMonth.Substring(4)) );
            return Amount / daysOfBudgetMonth;
        }
    }
}
