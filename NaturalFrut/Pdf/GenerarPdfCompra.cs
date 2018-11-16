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
    public class GenerarPdfCompra : Page
    {
        public readonly CompraLogic compraBL;
        private readonly ProductoXCompraLogic productoxCompraBL;

        public GenerarPdfCompra(CompraLogic CompraLogic,
            ProductoXCompraLogic ProductoXCompraLogic)
        {
            compraBL = CompraLogic;

            productoxCompraBL = ProductoXCompraLogic;
        }
        public void CrearPdf(int id)
        {

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    var compra = compraBL.GetCompraById(id);
                    CompraViewModel viewModel = new CompraViewModel(compra)
                    {
                        // Clientes = clienteBL.GetClienteById(venta.ClienteID),
                        ProductoXCompra = productoxCompraBL.GetProductoXCompraByIdCompra(compra.ID)

                    };

                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("<br />");
                   

                    //Generar contenido del body de la tabla
                    sb.Append("<table style='font-size:8px;' border = '0' ;>");



                    bool fondoColor = true;
                    foreach (var prod in viewModel.ProductoXCompra)
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

                        sb.Append("<td width='70%'>" + prod.Producto.Nombre + "</td>");
                        sb.Append("<td width='10%' align = 'center'>" + prod.Cantidad + "</td>");
                        sb.Append("<td width='10%' align = 'center'>" + prod.PrecioUnitario + "</td>");
                        sb.Append("<td width='10%' align = 'center'>" + prod.Total + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr><td align = 'center' colspan = '25'>Total: </td>");
                    sb.Append("<td>$"+compra.TotalGastos+"</td>");
                    sb.Append("</tr>");
                    sb.Append("</tr></table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 100,25);

                    //Instanciamos Componentes para el Header
                    PDFHeaderCompra pageHeader = new PDFHeaderCompra();
                    pageHeader.Compra = viewModel;
                   

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);

                    writer.PageEvent = pageHeader;
                 
                    //Donde la magia sucede
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();


                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=NotaPedido_" + compra.NumeroCompra + ".pdf");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(pdfDoc);
                    HttpContext.Current.Response.End();
                }
            }

        }



    }

    public class PDFHeaderCompra : PdfPageEventHelper
    {

        public CompraViewModel Compra { get; set; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            int pageN = writer.PageNumber;
            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
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

               

                //headerTable.SetWidths(new float[] { 70,10,10,10 });             


                PdfPCell cell;


                cell = new PdfPCell();

                cell.Colspan = 12;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("NATURAL FRUT", titleFont));
                cell.Colspan = 8;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Orden de Compra", subTitleFont));
                cell.Colspan = 4;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);             

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Colspan = 12;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);               

                cell = new PdfPCell(new Phrase("Sebastián Alejandro Genebrier  CUIT:20-32763767-4", boldTableFont));
                cell.Colspan = 8;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("N°: " + Compra.NumeroCompra, boldTableFont));
                cell.Colspan = 4;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);                

                cell = new PdfPCell(new Phrase("Proveedor: " + Compra.ProveedorObj.Nombre, boldTableFont));
                cell.Colspan = 8;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Fecha: " + Compra.Fecha, boldTableFont));
                cell.Colspan = 4;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);
                

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.Colspan = 12;
                cell.Border = 0;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                headerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cantidad", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Importe Unitario", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                


                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 20, writer.DirectContent);



            }
            else
            {


                // var headerTable2 = new PdfPTable(12);
                headerTable.HorizontalAlignment = 0;
                headerTable.SpacingBefore = 0;
                headerTable.SpacingAfter = 0;
                headerTable.DefaultCell.Border = 5;
                headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]
               // headerTable.SetWidths(new float[] { 70,10,10,10 });

                PdfPCell cell;         

                cell = new PdfPCell(new Phrase("Producto", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cantidad", boldTableFont));
                cell.BorderWidth = 0;
                headerTable.AddCell(cell);
                cell.FixedHeight = 25;
                cell = new PdfPCell(new Phrase("Importe Unitario", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                cell = new PdfPCell(new Phrase("Importe Total", boldTableFont));
                cell.BorderWidth = 0;
                cell.FixedHeight = 25;
                headerTable.AddCell(cell);
                

                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 20, writer.DirectContent);



            }

            // 

        }


    }

 


}