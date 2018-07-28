using Common.UnitilyModel;
using Common.Util;
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
                CheckOutDate = model.CheckOutDate.Value,
                CheckInDate = model.CheckInDate
            };
            unitOfWork.BookAssignationRepository.Insert(bookAssignation);

            var bookEntity = unitOfWork.BookRepository.GetByID(model.BookID);
            bookEntity.CheckInCheckOut = Common.Enum.CheckInOutStatus.CheckedOut;
            unitOfWork.BookRepository.Update(bookEntity);

            unitOfWork.Save();
        }

        public void AddCheckIn(BookCheckInVIewModel model)
        {
            var bookEntity = unitOfWork.BookRepository.GetByID(model.BoookID);
            var lastBookAssignation = unitOfWork.BookAssignationRepository.GetByID(model.AssinationID);

            if(bookEntity.CheckInCheckOut == Common.Enum.CheckInOutStatus.CheckedIn)
            {
                throw new Exception("Book is already checked in.");
            }

            bookEntity.CheckInCheckOut = Common.Enum.CheckInOutStatus.CheckedIn;
            lastBookAssignation.CheckInDate = model.ActualReturnDate;

            unitOfWork.BookRepository.Update(bookEntity);
            unitOfWork.BookAssignationRepository.Update(lastBookAssignation);
            unitOfWork.Save();
        }

        public BookCheckInVIewModel GetCheckInViewModel(int bookID)
        {
            var bookEntity = unitOfWork.BookRepository.GetByID(bookID);

            var lastBookAssignation = unitOfWork.BookAssignationRepository.Get(null, null, "AssignedPerson")
                .Where(x => x.BookID == bookID)
                .OrderByDescending(x => x.CheckOutDate).FirstOrDefault();


            BookCheckInVIewModel model = new BookCheckInVIewModel()
            {
                BoookID = bookID,
                AssinationID = lastBookAssignation.ID,
                ActualReturnDate = DateTime.Now,
                AssignedPerson = new AssignedPersonViewModel()
                {
                    MobileNumber = lastBookAssignation.AssignedPerson.MobileNumber,
                    NationalID = lastBookAssignation.AssignedPerson.NationalID,
                    PersonName = lastBookAssignation.AssignedPerson.PersonName
                },
                RequiredReturnDate = lastBookAssignation.CheckOutDate,
                Penality = calculatePanelity(lastBookAssignation.CheckOutDate)
            };

            return model;
        }

        private decimal calculatePanelity(DateTime checkOutDate)
        {
            BusinessDaysCalculator businessDaysCalculator = new BusinessDaysCalculator();
            
            int exceedDays = businessDaysCalculator.GetBusinessDaysBetweenCount(checkOutDate, DateTime.Now);
            if (exceedDays > 0)
            {
                return 5 * exceedDays;
            }
            else
            {
                return decimal.Zero;
            }
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
                        CurrentAssignedPerson = bookEntity.BookAssignations.Where(x => !x.CheckInDate.HasValue).OrderByDescending(x => x.CheckInDate)
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
                                            CheckInDate = z.CheckInDate.GetValueOrDefault(),
                                            CheckOutDate = z.CheckOutDate
                                        }).ToList()
                    };

                return bookDetailViewModel;

            }
                               
            return null;
        }
    }
}
