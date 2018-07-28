using Business.BusinessProviders;
using Common.Util;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bookModel = bookBusinessProvider.GetBookDetail(id.Value);

            if (bookModel == null)
            {
                return HttpNotFound();
            }


            return View(bookModel);
        }

        public ActionResult CheckOut(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            BookAssinationViewModel model = new BookAssinationViewModel();
            model.BookID = id.Value;
            model.AssignedPerson = new AssignedPersonViewModel();
            model.CheckInDate = DateTime.Now;

            BusinessDaysCalculator businessDaysCalculator = new BusinessDaysCalculator();
            model.CheckOutDate = businessDaysCalculator.GetDateAfterBusinessDays(DateTime.Now, 15);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(BookAssinationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookBusinessProvider.AddBookAssignment(model);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to check-out book");
            }

            return View(model);
        }

        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookCheckInVIewModel checkin = bookBusinessProvider.GetCheckInViewModel(id.Value);
            if (checkin == null)
            {
                return HttpNotFound();
            }

            return View(checkin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(BookCheckInVIewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookBusinessProvider.AddCheckIn(model);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to check-in the book");
            }

            
            return CheckIn(model.BoookID);
        }
    }
}