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
        public void BudgetCrossMonth()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" } , new Budget { Amount = 31, YearMonth = "201705" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 1), new DateTime(2017, 4, 5));
            BudgetShouldBe(500, actualResult);
        }

        [TestMethod]
        public void BudgetAndQueryCrossMonth()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" }, new Budget { Amount = 31, YearMonth = "201707" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 16), new DateTime(2017, 7, 15));
            BudgetShouldBe(1515, actualResult);
        }

        [TestMethod]
        public void QueryCrossMonthWithPartOfOneMonth()
        {
            GivenBudget(new List<Budget> {new Budget { Amount = 31, YearMonth = "201705" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 4, 3), new DateTime(2017, 5, 15));
            BudgetShouldBe(15, actualResult);
        }

        [TestMethod]
        public void BudgetWithFebInLeapYear()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 5800, YearMonth = "201602" }});
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2016, 2, 28), new DateTime(2016, 3, 1));
            BudgetShouldBe(400, actualResult);
        }

        [TestMethod]
        public void BudgetNotWithinQueryDay()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" }, new Budget { Amount = 31, YearMonth = "201705" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 1, 3), new DateTime(2017, 3, 5));
            BudgetShouldBe(0, actualResult);
        }

        [TestMethod]
        public void QueryDurationAfterBudget()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" }, new Budget { Amount = 31, YearMonth = "201705" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 6, 1), new DateTime(2017, 7, 30));
            BudgetShouldBe(0, actualResult);
        }

        [TestMethod]
        public void QueryIncludeBudgetWithFullMonth()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 3000, YearMonth = "201704" }, new Budget { Amount = 31, YearMonth = "201705" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2017, 2, 3), new DateTime(2017, 6, 30));
            BudgetShouldBe(3031, actualResult);
        }

        [TestMethod]
        public void QueryAndBudgetCrossYear()
        {
            GivenBudget(new List<Budget> { new Budget { Amount = 62, YearMonth = "201607" }, new Budget { Amount = 6200, YearMonth = "201707" }, new Budget { Amount = 620, YearMonth = "201807" } });
            var actualResult = _budgetReturn.BudgetResult(new DateTime(2016, 7, 1), new DateTime(2018, 7, 1));
            BudgetShouldBe(6282, actualResult);
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