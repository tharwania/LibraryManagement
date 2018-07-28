using Business.BusinessProviders;
using Common.UnitilyModel;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        BooksBusinessProvider booksBusinessProvider = new BooksBusinessProvider();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;

            IEnumerable<BookViewModel> result =  
                booksBusinessProvider.GetBookListForDataTable(model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

    }
}