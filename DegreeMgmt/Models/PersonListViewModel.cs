using DegreeMgmt.Models.Entities;
using System.Collections.Generic;

namespace DegreeMgmt.Models
{
    public class PersonListViewModel
    {
        public IEnumerable<Person> Persons { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}