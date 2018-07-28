using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() : base("LibraryContext")
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<AssignedPerson> AssignedPersons { get; set; }
        public DbSet<BookAssignation> BookAssignations { get; set; }
    }
}
