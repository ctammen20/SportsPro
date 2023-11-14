using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SportsPro.Models
{
    public class Registration
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ProductID { get; set; }
        [ValidateNever]

        public Customer Customer { get; set; }
        [ValidateNever]

        public Product Product { get; set; }
    }
}
