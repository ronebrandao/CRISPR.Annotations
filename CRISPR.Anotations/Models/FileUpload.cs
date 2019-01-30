using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CRISPR.Anotations.Models 
{

    public class FileUpload
    {
        [Required]
        [Display(Name="Titile")]
        [StringLength(60, MinimumLength = 3)]
        public string Titile { get; set;}

        [Required]
        [Display(Name="Annotations")]
        public IFormFile Annotations { get; set; }

    }

}