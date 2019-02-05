using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CRISPR.Anotations.Models;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using CRISPR.Anotations.Helpers.Zipping;

namespace CRISPR.Anotations.Helpers
{
    public static class FileHelpers
    {
        public static Stream ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState, Annotation annotation)
        {

            if (formFile.Length > 0)
            {
                var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

                try
                {
                    SheetHelper sheetHelper = new SheetHelper(formFile.FileName, annotation, formFile.OpenReadStream());
                    sheetHelper.ReadSheet();

                    List<ZipItem> items = new List<ZipItem>();

                    items.Add(new ZipItem("CRISPR.fasta", sheetHelper.CRISPR.ToString(), Encoding.UTF8));
                    items.Add(new ZipItem("Espacador.fasta", sheetHelper.Spacer.ToString(), Encoding.UTF8));

                    return Zipper.Zip(items);
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name,
                      $"The file ({fileName}) upload failed. " +
                      $"Please contact the Help Desk for support. Error: {ex.Message}");
                }

            }

            return null;
        }

    }
}