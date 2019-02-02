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

namespace CRISPR.Anotations.Utilities
{
    public static class FileHelpers
    {
        public static Stream ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState, Annotation annotation)
        {

            if (formFile.Length > 0)
            {
                var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));
                ISheet sheet = null;
                StringBuilder lista1 = new StringBuilder();
                StringBuilder lista2 = new StringBuilder();

                try
                {
                    Stream stream = formFile.OpenReadStream();

                    if (Path.GetExtension(formFile.FileName) == ".xlsx")
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);

                    }
                    else if (Path.GetExtension(formFile.FileName) == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0);
                    }

                    IRow headerRow = sheet.GetRow(0);

                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        lista1.AppendLine($">CRISPR{annotation.CRISPRName}_{annotation.MicroorganismName}_Rep{i}");
                        lista1.AppendLine(row.GetCell(1).ToString());

                        lista2.AppendLine($">Espacador{annotation.Spacer}_{annotation.MicroorganismName}_Rep{i}");
                        lista2.AppendLine(row.GetCell(2).ToString());
                    }

                    List<ZipItem> items = new List<ZipItem>();

                    items.Add(new ZipItem("CRISPR.fasta", lista1.ToString(), UTF8Encoding.UTF8));
                    items.Add(new ZipItem("Espacador.fast", lista2.ToString(), UTF8Encoding.UTF8));

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