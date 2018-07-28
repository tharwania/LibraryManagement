using Common.UnitilyModel;
using Common.ViewModel;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessProviders
{

    public class BooksBusinessProvider
    {
        private UnitOfWork unitOfWork;

        public BooksBusinessProvider()
        {
            unitOfWork = new UnitOfWork();
        }

        public IList<BookViewModel> GetBookListForDataTable(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<BookViewModel>();
            }
            return result;
        }

        public void AddBookAssignment(BookAssinationViewModel model)
        {
            BookAssignation bookAssignation = new BookAssignation()
            {
                BookID = model.BookID,
                AssignedPerson = new AssignedPerson()
                {
                    MobileNumber = model.AssignedPerson.MobileNumber,
                    NationalID = model.AssignedPerson.NationalID,
                    PersonName = model.AssignedPerson.PersonName
                },
                CheckOutDate = model.CheckOutDate,
                CheckInDate = model.CheckInDate
            };
            unitOfWork.BookAssignationRepository.Insert(bookAssignation);

            var bookEntity = unitOfWork.BookRepository.GetByID(model.BookID);
            bookEntity.CheckInCheckOut = Common.Enum.CheckInOutStatus.CheckedOut;
            unitOfWork.BookRepository.Update(bookEntity);

            unitOfWork.Save();
        }

        public List<BookViewModel> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            var query = unitOfWork.BookRepository
                .Get(x => string.IsNullOrEmpty(searchBy) ||
                                            (searchBy.ToLower().Contains(x.BookTitle.ToLower()) ||
                                            searchBy.ToLower().Contains(x.ISBN.ToLower())));
            if (sortDir) {
                switch (sortBy)
                {
                    case "ISBN":
                        query = query.OrderBy(x => x.BookTitle);
                        break;
                    case "PublishYear":
                        query = query.OrderBy(x => x.BookTitle);
                        break;
                    case "CoverPrice":
                        query = query.OrderBy(x => x.BookTitle);
                        break;
                    case "CheckInOutStatus":
                        query = query.OrderBy(x => x.BookTitle);
                        break;
                    default:
                        query = query.OrderBy(x => x.BookTitle);
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case "ISBN":
                        query = query.OrderByDescending(x => x.BookTitle);
                        break;
                    case "PublishYear":
                        query = query.OrderByDescending(x => x.BookTitle);
                        break;
                    case "CoverPrice":
                        query = query.OrderByDescending(x => x.BookTitle);
                        break;
                    case "CheckInOutStatus":
                        query = query.OrderByDescending(x => x.BookTitle);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.BookTitle);
                        break;
                }
            }
            var result = query.Skip(skip).Take(take)
                .Select(m => new BookViewModel
                 {
                     BookID = m.ID,
                     BookTitle = m.BookTitle,
                     ISBN = m.ISBN,
                     CheckInOutStatus = m.CheckInCheckOut.HasValue ? m.CheckInCheckOut.ToString() : "None",
                     CoverPrice = m.CoverPrice,
                     PublishYear = m.PublishDate.Year
                 })
                 .ToList();

            filteredResultsCount = unitOfWork.BookRepository.Get(x => string.IsNullOrEmpty(searchBy) ||
                                            (searchBy.ToLower().Contains(x.BookTitle.ToLower()) ||
                                            searchBy.ToLower().Contains(x.ISBN.ToLower()))).Count();
            totalResultsCount = unitOfWork.BookRepository.Get().Count();

            return result;


        }

        public BookDetailViewModel GetBookDetail(int BookID)
        {
            var bookEntity = unitOfWork.BookRepository
                                .Get(null, null, "BookAssignations,BookAssignations.AssignedPerson").Where(x => x.ID == BookID)
                                .SingleOrDefault();

            if(bookEntity != null)
            {
                BookDetailViewModel bookDetailViewModel =
                    new BookDetailViewModel()
                    {
                        BookID = bookEntity.ID,
                        BookTitle = bookEntity.BookTitle,
                        ISBN = bookEntity.ISBN,
                        CheckInOutStatus = bookEntity.CheckInCheckOut.HasValue ? bookEntity.CheckInCheckOut.ToString() : "None",
                        CoverPrice = bookEntity.CoverPrice,
                        PublishYear = bookEntity.PublishDate.Year,
                        CurrentAssignedPerson = bookEntity.BookAssignations.Where(x => !x.CheckOutDate.HasValue).OrderByDescending(x => x.CheckInDate)
                                        .Select(y => new AssignedPersonViewModel
                                        {
                                            MobileNumber = y.AssignedPerson.MobileNumber,
                                            NationalID = y.AssignedPerson.NationalID,
                                            PersonName = y.AssignedPerson.PersonName
                                        }).SingleOrDefault(),
                        BookAssignationHistory = bookEntity.BookAssignations.OrderByDescending(x => x.CheckInDate)
                                        .Select(z => new BookAssinationViewModel()
                                        {
                                            AssignedPerson = new AssignedPersonViewModel
                                            {
                                                MobileNumber = z.AssignedPerson.MobileNumber,
                                                NationalID = z.AssignedPerson.NationalID,
                                                PersonName = z.AssignedPerson.PersonName
                                            },
                                            CheckInDate = z.CheckInDate,
                                            CheckOutDate = z.CheckOutDate
                                        }).ToList()
                    };

                return bookDetailViewModel;

            }
                               
            return null;
        }
    }
}
