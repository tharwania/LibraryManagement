using Business.BusinessProviders;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        BooksBusinessProvider bookBusinessProvider = new BooksBusinessProvider();
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var booModel = bookBusinessProvider.GetBookDetail(id);

            return View(booModel);
        }
       
        public ActionResult CheckOut(int id)
        {
            BookAssinationViewModel model = new BookAssinationViewModel();
            model.BookID = id;
            model.AssignedPerson = new AssignedPersonViewModel();
            model.CheckInDate = DateTime.Now;
            model.CheckOutDate = DateTime.Now.AddDays(15);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(BookAssinationViewModel model)
        {
            bookBusinessProvider.AddBookAssignment(model);

            return RedirectToAction("Index", "Home");
        }
    }
}