using DAL.Context;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UnitOfWork : IDisposable
    {
        private LibraryContext context = new LibraryContext();
        private GenericRepository<Book> bookRepository;
        private GenericRepository<BookAssignation> bookAssignationRepository;
        private GenericRepository<AssignedPerson> assignedPersonRepository;
   
        public GenericRepository<Book> BookRepository
        {
            get
            {

                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(context);
                }
                return bookRepository;
            }
        }
        public GenericRepository<BookAssignation> BookAssignationRepository
        {
            get
            {

                if (this.bookAssignationRepository == null)
                {
                    this.bookAssignationRepository = new GenericRepository<BookAssignation>(context);
                }
                return bookAssignationRepository;
            }
        }
        public GenericRepository<AssignedPerson> AssignedPersonRepository
        {
            get
            {

                if (this.assignedPersonRepository == null)
                {
                    this.assignedPersonRepository = new GenericRepository<AssignedPerson>(context);
                }
                return assignedPersonRepository;
            }
        }
 
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
