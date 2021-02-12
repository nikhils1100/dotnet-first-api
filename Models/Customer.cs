using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_NwDb.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        [MinLength(0, ErrorMessage ="Enter Valid Phone Number")]
        [MaxLength(10,ErrorMessage ="Enter Valid Phone Number")]
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
