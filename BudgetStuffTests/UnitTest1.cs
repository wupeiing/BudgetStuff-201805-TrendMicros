using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetStuffTests
{

    [TestClass]
    public class UnitTest1
    {
        private BudgetReturn _budgetReturn;
        private IRepository<Budget> _repository= Substitute.For<IRepository<Budget>>();

        [TestInitializeAttribute]
        public void TestInit()
        {
            _budgetReturn = new BudgetReturn(_repository);

        }

        [TestMethod]
        public void NoBudget()
        {
            GivenBudget(new List<Budget>());
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 1, 1), new DateTime(2018, 1, 1));
            Assert.AreEqual(0, actualResult);
        }

        [TestMethod]
        public void FullMonth()
        {
            GivenBudget(new List<Budget> { new Budget{Amount = 3000, YearMonth = "201704"}});
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017,4,1), new DateTime(2017,4,30));
            Assert.AreEqual(3000, actualResult);
        }


        [TestMethod]
        public void PartofOneMonth()
        {
            decimal expected = 700;
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 1), new DateTime(2017, 4, 7));
            Assert.AreEqual(expected, actualResult);
        }


        private void GivenBudget(List<Budget> budgets)
        {
            _repository.GetBudget("", "").ReturnsForAnyArgs(budgets);
        }
    }
}