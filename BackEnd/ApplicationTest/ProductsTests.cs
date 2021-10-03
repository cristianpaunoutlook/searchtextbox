using Application.Errors;
using Application.Products;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTest
{
    [TestClass]
    public class ProductsTests : TestBase
    {
        [TestMethod]
        public async Task ShouldReturnAllProductsInDbInEachCateg()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                var command = new List.Query() { SearchedText = "app" };
                var handler = new List.Handler(context, GetMapper());

                //ACT
                var products = await handler.Handle(command, (new CancellationTokenSource()).Token);

                //assert top 5 by weight + top 5 by start name + top 20 contains text
                Assert.IsTrue(products.Count == 30);
                //top 5 by weight 
                Assert.IsTrue(products.Where(p => p.SearchWeight == 4).ToList().Count == 3);
                Assert.IsTrue(products.Where(p => p.SearchWeight == 3).ToList().Count == 2);
                //top 5 by start name
                Assert.IsTrue(products.Where(p => p.Name.ToLower().StartsWith("app")).ToList().Count == 5);
                //top 20 by contains searched text not start with searched text and hasn't search weight > 2
                Assert.IsTrue(products.Where(p => p.Name.ToLower().Contains("app") && !p.Name.ToLower().StartsWith("app") && p.SearchWeight < 3).ToList().Count == 20);
            }
        }

        [TestMethod]
        public async Task ShouldReturnAllProductsInDbInEachCateg2()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                var command = new List.Query() { SearchedText = "apples 1" };
                var handler = new List.Handler(context, GetMapper());

                //ACT
                var products = await handler.Handle(command, (new CancellationTokenSource()).Token);

                //assert top 5 by weight + top 5 by start name + top 20 contains text => 5 by weight + 1 by start text + 11 by contains
                Assert.IsTrue(products.Count == 17);
                //top 5 by weight 
                Assert.IsTrue(products.Where(p => p.SearchWeight == 4).ToList().Count == 3);
                Assert.IsTrue(products.Where(p => p.SearchWeight == 3).ToList().Count == 2);
                //top 5 by start name --> only one product start with apples 1
                Assert.IsTrue(products.Where(p => p.Name.ToLower().StartsWith("apples 1")).ToList().Count == 1);
                //top 20 by contains searched text not start with searched text and hasn't search weight > 2 ==> new apples 1, new apples 10, new apples 11 ... new apples 19, 
                Assert.IsTrue(products.Where(p => p.Name.ToLower().Contains("apples 1") && !p.Name.ToLower().StartsWith("apples 1") && p.SearchWeight < 3).ToList().Count == 11);
            }
        }

        [TestMethod]
        public async Task ShouldReturnTopProductsByWeightWhithoutSearchedText()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                var command = new List.Query() { SearchedText = "aaaa" };
                var handler = new List.Handler(context, GetMapper());

                //ACT
                var products = await handler.Handle(command, (new CancellationTokenSource()).Token);

                //assert top 5 by weight + top 5 by start name + top 20 contains text - only 5 products by weight without containg text
                Assert.IsTrue(products.Count == 5);
                //top 5 by weight 
                Assert.IsTrue(products.Where(p => p.SearchWeight == 4).ToList().Count == 3);
                Assert.IsTrue(products.Where(p => p.SearchWeight == 3).ToList().Count == 2);
                //top 5 by start name - no products start with aaaa
                Assert.IsTrue(products.Where(p => p.Name.ToLower().StartsWith("aaaa")).ToList().Count == 0);
                //top 20 by contains searched text not start with searched text and hasn't search weight > 2 - no products contains aaaa
                Assert.IsTrue(products.Where(p => p.Name.ToLower().Contains("aaaa") && !p.Name.ToLower().StartsWith("aaaa") && p.SearchWeight < 3).ToList().Count == 0);
            }
        }

        [TestMethod]
        public async Task IfNoWeightReturnOnlyProductsByTextCriteria()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                //remove search weight value for all products
                context.Products.ToList().ForEach(p => p.SearchWeight = 0);
                context.SaveChanges();
                var command = new List.Query() { SearchedText = "app" };
                var handler = new List.Handler(context, GetMapper());

                //ACT
                var products = await handler.Handle(command, (new CancellationTokenSource()).Token);

                //assert top 5 by weight + top 5 by start name + top 20 contains text - only 5 products by weight without containg text
                Assert.IsTrue(products.Count == 25);
                //top 5 by weight 
                Assert.IsTrue(products.Where(p => p.SearchWeight > 0).ToList().Count == 0);
                //top 5 by start name 
                Assert.IsTrue(products.Where(p => p.Name.ToLower().StartsWith("app")).ToList().Count == 5);
                //top 20 by contains searched text not start with searched text 
                Assert.IsTrue(products.Where(p => p.Name.ToLower().Contains("app") && !p.Name.ToLower().StartsWith("app")).ToList().Count == 20);
            }
        }

        [TestMethod]
        public void ShouldThrowExceptionIfSearchTextLessThen3Chars()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                var command = new List.Query() { SearchedText = "ap" };
                var handler = new List.Handler(context, GetMapper());

                //ACT && ASSERT
                Assert.ThrowsExceptionAsync<RestException>(() => handler.Handle(command, (new CancellationTokenSource()).Token),
                    "Search by 3 ore more chars please!");
            }
        }

        [TestMethod]
        public async Task IfNoWeightReturnAndSearchCriteriaReturnEmptyList()
        {
            using (var context = GetDbContext())
            {
                //ARRANGE
                InitProductsTable(context);
                //remove search weight value for all products
                context.Products.ToList().ForEach(p => p.SearchWeight = 0);
                context.SaveChanges();
                var command = new List.Query() { SearchedText = "aaa" };
                var handler = new List.Handler(context, GetMapper());

                //ACT
                var products = await handler.Handle(command, (new CancellationTokenSource()).Token);

                //assert top 5 by weight + top 5 by start name + top 20 contains text - 0 products -- no weight and no containg text
                Assert.IsTrue(products.Count == 0);
            }
        }
    }
}
