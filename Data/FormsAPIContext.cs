using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FormsAPI.Models;

namespace FormsAPI.Data
{
    public class FormsAPIContext : DbContext
    {
        public FormsAPIContext (DbContextOptions<FormsAPIContext> options)
            : base(options)
        {
        }

        public DbSet<FormsAPI.Models.Form> Form { get; set; } = default!;

        public DbSet<FormsAPI.Models.TypeComponent>? typeComponent { get; set; }

        public DbSet<FormsAPI.Models.Component>? Component { get; set; }

        public DbSet<FormsAPI.Models.Label>? Label { get; set; }

        public DbSet<FormsAPI.Models.LabelsToComponent>? LabelsToComponent { get; set; }

        public DbSet<FormsAPI.Models.ListInformationToScrollComponent>? ListInformationToScrollComponent { get; set; }
    }
}
