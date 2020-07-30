using System;
using System.IO;
using Contoso.Client.Helpers;
using Contoso.Shared.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;

namespace Contoso.Client.PDF {
    /// <summary>
    /// Generate PDF Report 
    /// using UserPrint class to pass User  Model
    /// </summary>
    public class Report {

        private int _pagenumber;
        ApplicationUser _user;

        // Generate Report from  User Model 
        public Report(ApplicationUser user) {
            _user = user;
        }

        public void Generate(IJSRuntime js, int pagenumber, string filename = "user.pdf") {
            _pagenumber = pagenumber;

            js.InvokeVoidAsync(
                "saveAsFile",
                filename,
                Convert.ToBase64String(ReportPDF())
            );
        }

        public void OpenToIframe(IJSRuntime js, int pagenumber, string idiFrame) {
            _pagenumber = pagenumber;

            js.InvokeVoidAsync(
                "OpenToIframe",
                idiFrame,
                Convert.ToBase64String(ReportPDF())
            );
        }
        public void OpenIntoNewTab(IJSRuntime js, int pagenumber, string filename = "report.pdf") {
            _pagenumber = pagenumber;

            js.InvokeVoidAsync(
                "OpenIntoNewTab",
                filename,
                Convert.ToBase64String(ReportPDF())
            );
        }

        private byte[] ReportPDF() {
            var memoryStream = new MemoryStream();

            // Marge in centimeter, then I convert with .ToDpi()
            float margeLeft = 1.5f;
            float margeRight = 1.5f;
            float margeTop = 1.0f;
            float margeBottom = 1.0f;

            Document pdf = new Document(
                PageSize.A4,
                margeLeft.ToDpi(),
                margeRight.ToDpi(),
                margeTop.ToDpi(),
                margeBottom.ToDpi()
            );

            pdf.AddTitle("Blazor-PDF");
            pdf.AddAuthor("Ozzvy");
            pdf.AddCreationDate();
            pdf.AddKeywords("blazor");
            pdf.AddSubject("Create a pdf file with iText");

            PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);

            //HEADER and FOOTER
            var fontStyle = FontFactory.GetFont("Arial", 16, BaseColor.White);
            var labelHeader = new Chunk("User Info", fontStyle);
            HeaderFooter header = new HeaderFooter(new Phrase(labelHeader), false) {
                BackgroundColor = new BaseColor(48, 79, 254),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };
            //header.Border = Rectangle.NO_BORDER;
            pdf.Header = header;

            var labelFooter = new Chunk("Page", fontStyle);
            HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true) {
                Border = Rectangle.NO_BORDER,
                Alignment = Element.ALIGN_RIGHT
            };
            pdf.Footer = footer;

            pdf.Open();

            if (_pagenumber == 1)
                UserPrint.PageText(pdf, _user);

            pdf.Close();

            return memoryStream.ToArray();
        }

    }
}