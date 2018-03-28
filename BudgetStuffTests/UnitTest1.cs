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
            BudgetShouldBe(0, actualResult);
        }

        [TestMethod]
        public void FullMonth()
        {
            GivenBudget(new List<Budget> { new Budget{Amount = 3000, YearMonth = "201704"}});
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017,4,1), new DateTime(2017,4,30));
            BudgetShouldBe(3000, actualResult);
        }


        [TestMethod]
        public void PartofOneMonth()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 1), new DateTime(2017, 4, 7));
            BudgetShouldBe(700, actualResult);
        }

        [TestMethod]
        public void QueryCrossMonth()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 1), new DateTime(2017, 5, 5));
            BudgetShouldBe(3000, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InvalidDuration()
        {
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 1), new DateTime(2017, 3, 7));
        }

        private static void BudgetShouldBe(decimal expected, decimal actualResult)
        {
            Assert.AreEqual(expected, actualResult);
        }

        

        private void GivenBudget(List<Budget> budgets)
        {
            _repository.GetBudget("", "").ReturnsForAnyArgs(budgets);
        }
    }
}