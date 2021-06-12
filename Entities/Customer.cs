using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        [Required (ErrorMessage = "SSN is Required")]
        [RegularExpression (@"^\d{10}|\d{3}-\d{2}-\d{5}$", ErrorMessage ="Invalid Social Security Number")]
        public string SSN { get; set; }
    }
}
