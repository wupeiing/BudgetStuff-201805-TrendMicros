using System;
using System.Collections.Generic;

namespace BudgetStuffTests
{
    public interface IRepository<T>
    {
        List<Budget> GetBudget(string startTime, string endTime);
    }
}