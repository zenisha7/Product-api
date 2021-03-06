﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Product.Models;
using System;


namespace Product.Data
{
    public class ProductDbContext: DbContext
    {
       
        public DbSet<ProductDto> Products { get; set; }
        public DbSet<ResellHistory> ResellHistories { get; set; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductDto>(x =>
            {
                x.Property(y => y.ID).IsRequired();
                x.Property(y => y.ProductID).IsRequired();
                x.Property(y => y.StockLvl).IsRequired();
                x.Property(y => y.ResellPrice).IsRequired();
            });

            modelBuilder.Entity<ResellHistory>(x =>
            {
                x.Property(y => y.ID).IsRequired();
                x.Property(y => y.ProductID).IsRequired();
                x.Property(y => y.ResellPrice).IsRequired();
                x.Property(y => y.DateTime).IsRequired();
            });
        }
    }
}
