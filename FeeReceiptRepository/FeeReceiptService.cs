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
using iTextSharp.text.pdf.draw;



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

                        // Add a heading for SoftStacks Technologies
                        var companyHeadingParagraph = new Paragraph("SoftStacks Technologies", new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLUE)))
                        {
                            Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                            SpacingAfter = 10
                        };

                        // Create a new Paragraph with the title Fee Receipt
                        var titleParagraph = new Paragraph("Fee Receipt", new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.RED)))
                        {
                            Alignment = iTextSharp.text.Element.ALIGN_CENTER, SpacingAfter = 10
                        };

                        // Set the spacing after the paragraph
                        titleParagraph.SpacingAfter = 20;

                        // Add the title paragraph to the document
                      

                        // Add a heading for Student Fee Details
                        var headingParagraph = new Paragraph("Student Fee Details", new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK)))
                        {
                            Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                            SpacingAfter = 10
                        };

                       


                        // Open the document for writing
                        document.Open();

                        // Add the title paragraph to the document
                        document.Add(companyHeadingParagraph);
                        document.Add(titleParagraph);
                        document.Add(headingParagraph);

                        // Create a table with four columns
                        var table = new PdfPTable(5);
                        table.SetTotalWidth(new float[] { 50, 100, 70, 150, 80 }); // Adjust the widths as needed
                        table.LockedWidth = true; // Lock the width to make sure it respects the total width
                        table.TotalWidth = 550f;
                        

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

                        var headerCell4 = new PdfPCell(new Phrase("Description", boldFont)); // Corrected the variable name here
                        headerCell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                        headerCell4.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(headerCell4);

                        var headerCell5 = new PdfPCell(new Phrase("Date Paid", boldFont));
                        headerCell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                        headerCell5.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(headerCell5);

                        // Add data to the table
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.StudentId.ToString(), boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.Student?.StudentName ?? "N/A", boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.TotalFee, boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.Description ?? "N/A", boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(feeDetails.RegistrationDate.ToString(), boldFont)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

                        // Add the table to the document
                        document.Add(table);

                        // Add a line separator after the table
                        var lineSeparator = new LineSeparator(1, 100, BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -5);
                        document.Add(lineSeparator);

                        // Add a space or blank line for visual separation
                        document.Add(new Paragraph(" "));

                        // Add a prompt for the signature
                        var signaturePrompt = new Paragraph("Authorized Signature", new Font(FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK)))
                        {
                            Alignment = iTextSharp.text.Element.ALIGN_RIGHT,
                            SpacingBefore = 10
                        };

                        document.Add(signaturePrompt);

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
