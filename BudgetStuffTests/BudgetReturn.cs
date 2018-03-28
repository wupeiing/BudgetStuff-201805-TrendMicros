using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetStuffTests
{
    public class BudgetReturn
    {
        private readonly IRepository<Budget> _repo;

        public BudgetReturn(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal BudgetResult(DateTime startTime, DateTime endTime)
        {
            var startMonth = startTime.ToString("yyyyMM");
            var endMonth = endTime.ToString("yyyyMM");
            if (startTime > endTime)
            {
                throw new InvalidException();
            }
            var budgets = _repo.GetBudget(startMonth, endMonth);
            if (budgets.Count == 0)
            {
                return 0;
            }

            var monthes = (endTime.Year * 12 + endTime.Month) - (startTime.Year * 12 + startTime.Month) + 1;

            //budget to Dict
            Dictionary<String,Budget> budgetDict = new Dictionary<string, Budget>();
            foreach(Budget budget in budgets)
            {
                budgetDict[budget.YearMonth] = budget;
            }
            var rtn = 0;
            if (monthes == 1)
            {
                //return startTime.Day;
                var validDays = endTime.Day - startTime.Day + 1;
                var ym = startTime.ToString("yyyyMM");
                if (budgetDict.ContainsKey(ym))
                {
                    var budget = budgetDict[ym];
                    rtn += budget.getDailyAmount() * validDays;
                }

                return rtn;
            }

            for (int i = 0; i <= monthes; ++i)
            {
                var validDays = 0;
                if (i == monthes )
                {
                    validDays = DateTime.DaysInMonth(endTime.Year, endTime.Month) - startTime.Day + 1;

                }
                else
                {
                    validDays = DateTime.DaysInMonth(startTime.Year, startTime.Month) - startTime.Day + 1;
                    
                    
                }
                var ym = startTime.ToString("yyyyMM");
        
                if (budgetDict.ContainsKey(ym))

                {
                    var budget = budgetDict[ym];
                    rtn += budget.getDailyAmount() * validDays;
                }

                if (i != monthes)
                {
                    startTime = startTime.AddDays(validDays);
                }

            }



            //var budgetDate = DateTime.ParseExact(budgets[0].YearMonth+"01", "yyyyMMdd", null);
            //int daysOfBudgetMonth = DateTime.DaysInMonth(budgetDate.Year, budgetDate.Month);
            //var days = (endTime - startTime).Days+1;
            
       

            return rtn;
            
            
        }
    }

    public class InvalidException: Exception
    {

    }
}
