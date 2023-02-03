using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ASP.Models
{
    public class SpecializationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plese provide the specialization")]
        [Display(Name = "Specialization")]
        public string SpecializationName { get; set; }
    }
}
