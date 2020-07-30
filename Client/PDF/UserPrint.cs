using System;
using Contoso.Client.Helpers;
using Contoso.Shared.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Contoso.Client.PDF {
    public class UserPrint {

        public static void PageText(Document pdf, ApplicationUser User) {
            Font _fontStyle = FontFactory.GetFont("Tahoma", 8f, Font.ITALIC);

            var Name = new Paragraph("User Name:", new Font(Font.HELVETICA, 20, Font.BOLD));
            Name.SpacingAfter = 10f;

            pdf.Add(Name);
            var userName = new Paragraph(User.UserName, new Font(Font.HELVETICA, 15, Font.NORMAL));
            userName.SpacingBefore = 5f;
            userName.SetAlignment("LEFT");

            pdf.Add(userName);

            var Email = new Paragraph("Email:", new Font(Font.HELVETICA, 20, Font.BOLD));
            Email.SpacingAfter = 5f;

            pdf.Add(Email);
            var userEmail = new Paragraph(User.Email, new Font(Font.HELVETICA, 15, Font.NORMAL));
            userEmail.SpacingBefore = 5f;
            userEmail.SetAlignment("LEFT");
            pdf.Add(userEmail);

            // Create and add a Paragraph

            float margeborder = 1.5f;
            float widhtColumn = 8.5f;
            float space = 1.0f;

            MultiColumnText columns = new MultiColumnText();
            columns.AddSimpleColumn(margeborder.ToDpi(),
                pdf.PageSize.Width - margeborder.ToDpi() - space.ToDpi() - widhtColumn.ToDpi());
            columns.AddSimpleColumn(margeborder.ToDpi() + widhtColumn.ToDpi() + space.ToDpi(),
                pdf.PageSize.Width - margeborder.ToDpi());

            pdf.Add(columns);
        }
    }
}