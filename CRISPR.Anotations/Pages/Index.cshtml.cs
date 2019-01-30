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
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var filePath = "filepathname";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await FileUpload.Annotations.CopyToAsync(fileStream);
            }

            return RedirectToPage(".Index");
        }

    }
}
