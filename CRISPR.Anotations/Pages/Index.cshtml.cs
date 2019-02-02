using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CRISPR.Anotations.Models;
using CRISPR.Anotations.Utilities;


namespace CRISPR.Anotations.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
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
            var annotations = FileHelpers.ProcessFormFile(HttpContext.Request.Form.Files[0], ModelState, Annotation);

            return File(annotations, "application/octet-stream", "Annotations.zip");
        }

    }
}
