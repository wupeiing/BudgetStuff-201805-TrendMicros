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
            if (IsBudgetEmpty(budgets))
            {
                return 0;
            }

            var monthes = GetPeriod(startTime, endTime);
            var budgetDict = CollectBudget(budgets);

            var totalAmount = 0;


            for (int i = 0; i < monthes; i++)
            {
                var validDays = GetValidDays(startTime, endTime, i, monthes);

                var ym = startTime.ToString("yyyyMM");

                if (budgetDict.ContainsKey(ym))
                {
                    totalAmount = CollectAmount(budgetDict, ym, totalAmount, validDays);
                }

                startTime = startTime.AddDays(validDays);

            }
            return totalAmount;            
        }

        private static int CollectAmount(Dictionary<string, Budget> budgetDict, string ym, int totalAmount, int validDays)
        {
            var budget = budgetDict[ym];
            totalAmount += budget.getDailyAmount() * validDays;
            return totalAmount;
        }

        private static int GetValidDays(DateTime startTime, DateTime endTime, int i, int monthes)
        {
            var validDays = 0;
            if (i == monthes - 1)
            {
                validDays = endTime.Day - startTime.Day + 1;
            }
            else
            {
                validDays = DateTime.DaysInMonth(startTime.Year, startTime.Month) - startTime.Day + 1;
            }

            return validDays;
        }

        private static Dictionary<string, Budget> CollectBudget(List<Budget> budgets)
        {
//budget to Dict
            Dictionary<String, Budget> budgetDict = new Dictionary<string, Budget>();
            foreach (Budget budget in budgets)
            {
                budgetDict[budget.YearMonth] = budget;
            }

            return budgetDict;
        }

        private static int GetPeriod(DateTime startTime, DateTime endTime)
        {
            return (endTime.Year * 12 + endTime.Month) - (startTime.Year * 12 + startTime.Month) + 1;
        }

        private static bool IsBudgetEmpty(List<Budget> budgets)
        {
            return budgets.Count == 0;
        }
    }

    public class InvalidException: Exception
    {

    }
}
