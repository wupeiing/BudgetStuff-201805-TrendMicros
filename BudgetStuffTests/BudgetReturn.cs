using System;
using System.Collections.Generic;
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
            var budgets = _repo.GetBudget(startMonth, endMonth);
            if (budgets.Count == 0)
            {
                return 0;
            }

            int daysOfMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
            var days = (endTime - startTime).Days+1;

            return (decimal) budgets[0].Amount * days / daysOfMonth;
            
            
        }
    }
}
