using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CRISPR.Anotations.Models;


namespace CRISPR.Anotations.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        [BindProperty]
        public Annotation Annotation { get; set; }
        
        public IActionResult OnPost()
        {

            var annotations = FileHelpers.ProcessFormFile(FileUpload.Annotations, ModelState, Annotation);
            annotations.Position = 0;

            return File(annotations, "application/octet-stream", "Annotations");
        }

    }
}
