using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Persistence
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (context.Products.Count() > 0)
            {
                return;
            }
            //add 9 products that start with apples and that have a search weight 0, 1, 2
            for (int i = 1; i < 10; i++)
                context.Products.Add(new Product() { Name = $"Apples {i}", SearchWeight = i % 3, Price = i * 10 });

            //add 9 products that contains apples but not start with apples and that have a search weight 0, 1, 2
            for (int i = 1; i <= 30; i++)
                context.Products.Add(new Product() { Name = $"New Apples {i}", SearchWeight = i % 3, Price = i * 10 });

            //add 15 products without apples word and that have a search weight 0, 1, 2, 3, 4
            for (int i = 1; i <= 15; i++)
                context.Products.Add(new Product() { Name = $"Melon {i}", SearchWeight = i % 5, Price = i * 10 });

            context.SaveChanges();
        }
    }
}
