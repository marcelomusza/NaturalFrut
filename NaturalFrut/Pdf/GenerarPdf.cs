using iTextSharp.text.pdf;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.Data;
using System.Text;
using System.Web.UI;


namespace NaturalFrut.Pdf
{
    public class GenerarPdf : Page
    {
        public readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly ProductoXVentaLogic productoxVentaBL;

        public GenerarPdf(VentaMayoristaLogic VentaMayoristaLogic,
            ProductoXVentaLogic ProductoXVentaLogic)
        {
            ventaMayoristaBL = VentaMayoristaLogic;

            productoxVentaBL = ProductoXVentaLogic;
        }
        public void CrearPdf(int id)
        {

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    var venta = ventaMayoristaBL.GetVentaMayoristaById(id);
                    VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(venta)
                    {
                        // Clientes = clienteBL.GetClienteById(venta.ClienteID),
                        ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID)

                    };

                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");

                    //Generar contenido del body de la tabla
                    sb.Append("<table style='font-size:10px;' border = '0' ;>");



                    bool fondoColor = true;
                    foreach (var prod in viewModel.ProductoXVenta)
                    {

                        if (fondoColor)
                        {
                            sb.Append("<tr bgcolor='#e1e3e8'>");
                            fondoColor = false;
                        }
                        else
                        {
                            sb.Append("<tr>");
                            fondoColor = true;
                        }

                        sb.Append("<td width='25%'>" + prod.Producto.Nombre + "</td>");
                        //sb.Append("<td>pasas c/cobertura chocolate negrox 1kg</td>");
                        sb.Append("<td width='5%'>" + prod.Cantidad + "</td>");
                        sb.Append("<td width='5%'>" + prod.TipoDeUnidad.Nombre + "</td>");
                        sb.Append("<td width='5%'>" + prod.Descuento + "</td>");
                        sb.Append("<td width='5%'>" + prod.Importe + "</td>");
                        sb.Append("<td width='5%'>" + prod.Total + "</td>");
                        sb.Append("<td width='25%'>" + prod.Producto.Nombre + "</td>");
                        sb.Append("<td width='5%'>" + prod.Cantidad + "</td>");
                        sb.Append("<td width='5%'>" + prod.TipoDeUnidad.Nombre + "</td>");
                        sb.Append("<td width='5%'>" + prod.Descuento + "</td>");
                        sb.Append("<td width='5%'>" + prod.Importe + "</td>");
                        sb.Append("<td width='5%'>" + prod.Total + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr><td align = 'right' colspan = '5'>Total: </td>");
                    sb.Append("<td>$22232</td>");
                    sb.Append("<tr><td align = 'right' colspan = '5'>Total: </td>");
                    sb.Append("<td>$22232</td>");
                    sb.Append("</tr></table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 36, 36, 80, 36);

                    //Instanciamos Componentes para el Header
                    PDFHeader pageHeader = new PDFHeader();
                    pageHeader.VentaMayorista = viewModel;

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);

                    writer.PageEvent = pageHeader;
                    writer.PageEvent = new PDFFooter();

                    //Donde la magia sucede
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();


                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=NotaPedido_" + venta.NumeroVenta + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    HttpContext.Current.Response.End();
                }
            }

        }



    }

    public class PDFHeader : PdfPageEventHelper
    {

        public VentaMayoristaViewModel VentaMayorista { get; set; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            int pageN = writer.PageNumber;
            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
            var headerTable = new PdfPTable(12);

            if (pageN == 1)
            {

                headerTable.HorizontalAlignment = 0;
                headerTable.SpacingBefore = 0;
                headerTable.SpacingAfter = 0;
                headerTable.DefaultCell.Border = 5;
                headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]

                headerTable.SetWidths(new float[] { 25, 5, 5, 5, 5, 5, 25, 5, 5, 5, 5, 5 });


                PdfPCell cell;


                cell = new PdfPCell();

                cell.Colspan = 12;
                cell.Border = 0;

                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nota de Pedido", titleFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("No Valido como Factura", subTitleFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Nota de Pedido", titleFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("No Valido como Factura", subTitleFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Colspan = 6;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Duplicado", boldTableFont));
                cell.Colspan = 6;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("N Venta: " + VentaMayorista.NumeroVenta, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cliente: " + VentaMayorista.Cliente, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("N Venta: " + VentaMayorista.NumeroVenta, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cliente: " + VentaMayorista.Cliente, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Vendedor: " + VentaMayorista.Vendedor, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Direccion: " + VentaMayorista.ClienteObj.Direccion, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Vendedor: " + VentaMayorista.Vendedor, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Direccion: " + VentaMayorista.ClienteObj.Direccion, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Fecha: " + VentaMayorista.Fecha, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Telefono: " + VentaMayorista.ClienteObj.TelefonoNegocio, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Fecha: " + VentaMayorista.Fecha, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Telefono: " + VentaMayorista.ClienteObj.TelefonoNegocio, boldTableFont));
                cell.Colspan = 3;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Colspan = 12;
                cell.Border = 0;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cant.", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Unidad", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Desc.", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 15;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cant.", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Unidad", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Desc.", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);

                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);



            }
            else
            {


                // var headerTable2 = new PdfPTable(12);
                headerTable.HorizontalAlignment = 0;
                headerTable.SpacingBefore = 0;
                headerTable.SpacingAfter = 0;
                headerTable.DefaultCell.Border = 5;
                headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]

                PdfPCell cell;

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cant.", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Unidad", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Desc.", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cant.", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Unidad", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Desc.", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);

                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 20, writer.DirectContent);



            }

            // 

        }


    }

    public class PDFFooter : PdfPageEventHelper
    {
        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(12);
            // PdfPTable tabFot = new PdfPTable(12);
            tabFot.HorizontalAlignment = 0;
            tabFot.SpacingBefore = 10;
            tabFot.SpacingAfter = 10;
            tabFot.DefaultCell.Border = 5;

            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);

            PdfPCell cell;

            cell = new PdfPCell();
            cell.Colspan = 12;
            cell.BorderWidth = 0;
            tabFot.AddCell(cell);


            tabFot.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            cell = new PdfPCell(new Phrase("Haga su pedido al 4709-6832//15-2415-0520 o 15-5429-6101", boldTableFont));
            cell.BorderWidth = 0;
            cell.Colspan = 6;
            tabFot.AddCell(cell);

            cell = new PdfPCell(new Phrase("Haga su pedido al 4709-6832//15-2415-0520 o 15-5429-6101", boldTableFont));
            cell.BorderWidth = 0;
            cell.Colspan = 6;
            tabFot.AddCell(cell);

            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
        }
    }


}