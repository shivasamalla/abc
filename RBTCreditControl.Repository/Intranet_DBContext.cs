using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTCreditControl.Repository
{
    public class Emp_Data
    {
        [Key]
        public string emp_code { get; set; }
        public string emp_name { get; set; }
        public string emp_status { get; set; }
    }

    public class Intranet_DBContext : DbContext
    {
        public Intranet_DBContext(DbContextOptions<Intranet_DBContext> options) : base(options)
        {

        }

       public DbSet<Emp_Data> Emp_Data { get; set; }
    }
}
