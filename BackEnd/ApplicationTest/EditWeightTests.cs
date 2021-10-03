using Application.Errors;
using Application.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace ApplicationTest
{
    [TestClass]
    public class EditWeightTests : TestBase
    {
        [TestMethod]
        public void UpdatedProductsWeightShouldBePersistedInDb()
        {
            using (var context = GetDbContext())
            {
                InitProductsTable(context);
                var currentNumber = context.Products.Where(p => p.SearchWeight == 4).Count();
                var prdWithWeight4 = context.Products.Where(p=>p.SearchWeight==4).First();
                // Arrange
                var command = new EditWeight.Command()
                {
                    Id = prdWithWeight4.Id
                };
                var handler = new EditWeight.Handler(context);

                // Act
                var result = handler.Handle(command, (new CancellationTokenSource()).Token);

                // Assert
                Assert.IsTrue(context.Products.Where(p => p.SearchWeight == 4).Count() == currentNumber - 1);
                Assert.IsTrue(context.Products.Where(p => p.SearchWeight == 5).Count() == 1);
            }
        }

        [TestMethod]
        public void UpdatedProductWWithInvalidIdShouldThrowException()
        {
            using (var context = GetDbContext())
            {
                InitProductsTable(context);
                // Arrange
                var command = new EditWeight.Command()
                {
                    Id = 1000
                };
                var handler = new EditWeight.Handler(context);

                //ACT && ASSERT
                Assert.ThrowsExceptionAsync<RestException>(() => handler.Handle(command, (new CancellationTokenSource()).Token),
                    "Invalid product id!");
            }
        }

        [TestMethod]
        public void UpdatedProductWWithNegativeIdThrowException()
        {
            using (var context = GetDbContext())
            {
                InitProductsTable(context);
                // Arrange
                var command = new EditWeight.Command()
                {
                    Id = -1
                };
                var handler = new EditWeight.Handler(context);

                //ACT && ASSERT
                Assert.ThrowsExceptionAsync<RestException>(() => handler.Handle(command, (new CancellationTokenSource()).Token),
                    "Invalid product id!");
            }
        }
    }
}
