using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    //DbContext is a class that manages the database connection and model
    //It is a bridge between the database and the model
    public class ApplicationDBContext : DbContext
    {
        //DbContextOptions is a class that is used to configure the DbContext
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        //DbSet is a class that represents a table in the database, so this DbSet represents the Stock table
        public DbSet<Stock> Stocks { get; set; }
        //this DbSet represents the Comment table
        public DbSet<Comment> Comments { get; set; }
    }
}