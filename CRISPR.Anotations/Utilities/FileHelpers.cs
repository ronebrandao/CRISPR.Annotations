using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CRISPR.Anotations.Models;

namespace CRISPR.Anotations
{
    public class FileHelpers
    {
        public static async Task<string> ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState)   
        {
            var fieldDisplayName = string.Empty;

            MemberInfo property = typeof(FileUpload).GetProperty(
                formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));

            if (property != null)
            {
                var displayAttribute = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

                if (displayAttribute != null)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
                
            }    

            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName)); 

            if (formFile.ContentType.ToLower() != "text/plain")
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) must be a text file.");
            }
            else if (formFile.Length > 1048576)
            {
                modelState.AddModelError(formFile.Name, 
                    $"The {fieldDisplayName}file ({fileName}) exceeds 1 MB.");
            }
            else {
                try {

                    string fileContents;

                    using (var reader = new StreamReader(
                        formFile.OpenReadStream(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, 
                            throwOnInvalidBytes: true), detectEncodingFromByteOrderMarks: true
                        )) 
                        {

                            fileContents = await reader.ReadToEndAsync();

                            if (fileContents.Length > 0)
                            {
                                return fileContents;
                            }
                            else
                            {
                                modelState.AddModelError(formFile.Name, 
                                    $"The {fieldDisplayName}file ({fileName}) is empty.");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        modelState.AddModelError(formFile.Name, 
                            $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                            $"Please contact the Help Desk for support. Error: {ex.Message}");
                        // Log the exception
                    }

            }

            return string.Empty;
        }     
    
    }
}