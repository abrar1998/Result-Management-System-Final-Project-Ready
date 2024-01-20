using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.CodeAnalysis;
using RMS.Models;
using System.Reflection.Metadata;
using AngleSharp.Dom;
using RMS.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Properties;
using iText.IO.Font.Constants;



namespace RMS.FeeReceiptRepository
{
    public class FeeReceiptService : IFeeReceiptService
    {
        private readonly AccountContext context;

        public FeeReceiptService(AccountContext context)
        {
            this.context = context;
        }

        ///////////////////
        
        //creating table for receipt with the help of below code///// this method is fast

        public byte[] GenerateReceipt(FeeDetails feeDetails)
        {
            // Fetch the student details from the database using the StudentId
            var student = context.Students.FirstOrDefault(s => s.StudentId == feeDetails.StudentId);

            // Create a MemoryStream to store the PDF content
            using (MemoryStream ms = new MemoryStream())
            {
                // Create a PdfWriter, PdfDocument, and Document for PDF generation
                using (var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4))
                {
                    using (var writer = PdfWriter.GetInstance(document, ms))
                    {
                        // Set up fonts and colors
                        var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);

                        // Create a new Paragraph with the title
                        var titleParagraph = new Paragraph("Fee Receipt", new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.RED)))
                        {
                            Alignment = iTextSharp.text.Element.ALIGN_CENTER
                        };

                        // Set the spacing after the paragraph
                        titleParagraph.SpacingAfter = 20;

                        // Open the document for writing
                        document.Open();

                        // Add the title paragraph to the document
                        document.Add(titleParagraph);

                        // Add student information
                        document.Add(new Paragraph($"Student Id: {feeDetails.StudentId}", boldFont));

                        if (student != null)
                        {
                            document.Add(new Paragraph($"Student Name: {student.StudentName}", boldFont));
                        }


                        // Create a table with three columns
                        var table = new PdfPTable(3);

                        // Set up table headers
                        var headerCell1 = new PdfPCell(new Phrase("Id", boldFont));
                        headerCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                        headerCell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(headerCell1);

                        var headerCell2 = new PdfPCell(new Phrase("Name", boldFont));
                        headerCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                        headerCell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(headerCell2);

                        var headerCell3 = new PdfPCell(new Phrase("Fee", boldFont));
                        headerCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                        headerCell3.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(headerCell3);

                        // Add data to the table
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.StudentId.ToString(), boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.Student?.StudentName ?? "N/A", boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.TotalFee, boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

                        // Add the table to the document
                        document.Add(table);


                        // Add a thank you message
                        document.Add(new Paragraph("Thank you for your payment!", boldFont));

                        // Close the document
                        document.Close();

                        // Return the byte array representing the PDF content
                        return ms.ToArray();
                    }
                }
            }
        }




        //////////

       


        ////////////

        //below code will generate only simple receipt but takes time , slow 

        /* public byte[] GenerateReceipt(FeeDetails feeDetails)
         {
             var name = context.Students.Where(s => s.StudentId == feeDetails.StudentId).FirstOrDefault();
             using (MemoryStream ms = new MemoryStream())
             {

                 iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(ms);
                 iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(writer);
                 iText.Layout.Document document = new iText.Layout.Document(pdfDocument, iText.Kernel.Geom.PageSize.A4);


                 // Add content to the PDF document based on feeDetails
                 document.Add(new global::iText.Layout.Element.Paragraph($"Student Id: {feeDetails.StudentId}"));
                 document.Add(new global::iText.Layout.Element.Paragraph($"Student Name: {name.StudentName}"));
                 document.Add(new global::iText.Layout.Element.Paragraph($"Amount Paid: {feeDetails.TotalFee}"));
                 document.Add(new global::iText.Layout.Element.Paragraph($"Date Paid: {feeDetails.RegistrationDate}"));
                 document.Add(new global::iText.Layout.Element.Paragraph($"Student Id: {feeDetails.Description}"));

                 // Save the document to the MemoryStream
                 document.Close();

                 return ms.ToArray();
             }

         }
         */

    }

}
