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

namespace CRISPR.Anotations
{
    public static class FileHelpers
    {
        public static MemoryStream ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState, Annotation annotation)
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

                    byte[] byteLista1 = StringToByteArray(lista1.ToString());
                    byte[] byteLista2 = StringToByteArray(lista2.ToString());


                    var ms = new MemoryStream();

                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        var ZipArchiveEntry = archive.CreateEntry("CRISPR.fasta", CompressionLevel.Fastest);
                        using (var zipStream = ZipArchiveEntry.Open()) zipStream.Write(byteLista1, 0, byteLista1.Length);

                        ZipArchiveEntry = archive.CreateEntry("Espacador.fasta", CompressionLevel.Fastest);
                        using (var zipStream = ZipArchiveEntry.Open()) zipStream.Write(byteLista2, 0, byteLista2.Length);
                    }

                    return ms;

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

        private static byte[] StringToByteArray(string str)
        {
            var encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

    }
}