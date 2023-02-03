using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ASP.Models
{
    public class DoctorViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="Plese provide the doctor name")]
        [Display(Name ="Doctor Name")]
        public string DoctorName { get; set; }
        [Required(ErrorMessage ="Plese provide the experience")]
        public int? Experience { get; set; }
        [Required(ErrorMessage ="Plese provide the doctor Qualification")]
        public string Qualification { get; set; }
        [Required(ErrorMessage = "Plese provide the specialization")]
        [Display(Name = "Specialization")]
        public int? SpecializationId { get; set; }

        public string SpecializationName {get; set;} 


        public List<SpecializationViewModel> Specializations { get; set; }
    }
}
