using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using CRISPR.Anotations.Models;

namespace CRISPR.Anotations.Helpers
{
    public class SheetHelper
    {

        public StringBuilder CRISPR { get; set; } 
        public StringBuilder Spacer { get; set; }

        string FileName;
        Annotation Annotation;
        Stream Stream;

        public SheetHelper(string fileName, Annotation annotation, Stream stream)
        {
            CRISPR = new StringBuilder(); 
            Spacer = new StringBuilder(); 

            FileName = fileName;
            Annotation = annotation;
            Stream = stream;
        }

        public void ReadSheet()
        {
            ISheet sheet = null;

            if (Path.GetExtension(FileName) == ".xlsx")
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(Stream);
                sheet = hssfwb.GetSheetAt(0);

            }
            else if (Path.GetExtension(FileName) == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(Stream); //This will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0);
            }

            CreateLists(sheet);
        }

        private void CreateLists(ISheet sheet)
        {
            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                CRISPR.AppendLine($">CRISPR{Annotation.CRISPRName}_{Annotation.MicroorganismName}_Rep{i}");
                CRISPR.AppendLine(row.GetCell(1).ToString());

                Spacer.AppendLine($">Espacador{Annotation.Spacer}_{Annotation.MicroorganismName}_Rep{i}");
                Spacer.AppendLine(row.GetCell(2).ToString());
            }
        }

    }
}
