using Application.MapProfile;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest
{
    public class TestBase
    {
        protected IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MappingProfile)));
            var mapper = config.CreateMapper();
            return mapper;
        }

        protected DataContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContext = new DataContext(builder.Options);

            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        protected void InitProductsTable(DataContext context)
        {

            //add 9 products that start with apples and that have a search weight 0, 1, 2
            for (int i = 1; i < 10; i++)
                context.Products.Add(new Product() { Id = i, Name = $"Apples {i}", SearchWeight = i % 3 });

            //add 9 products that contains apples but not start with apples and that have a search weight 0, 1, 2
            for (int i = 1; i <= 30; i++)
                context.Products.Add(new Product() { Id = 9 + i, Name = $"New Apples {i}", SearchWeight = i % 3 });

            //add 15 products without apples word and that have a search weight 0, 1, 2, 3, 4
            for (int i = 1; i <= 15; i++)
                context.Products.Add(new Product() { Id = 39 + i, Name = $"Melon {i}", SearchWeight = i % 5 });

            context.SaveChanges();
        }
    }
}
