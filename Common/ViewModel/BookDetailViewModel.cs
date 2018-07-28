using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class BookDetailViewModel : BookViewModel
    {

        public AssignedPersonViewModel CurrentAssignedPerson { get; set; }
        public List<BookAssinationViewModel> BookAssignationHistory { get; set; }
    }
}
