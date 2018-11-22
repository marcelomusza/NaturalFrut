using NaturalFrut.App_BLL;
using NaturalFrut.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace NaturalFrut.Helpers
{
    public class ExcelExportHelper
    {

        private CompraLogic compraBL;
        private VentaMinoristaLogic ventaMinoristaBL;

        public ExcelExportHelper(CompraLogic compraBL)
        {
            this.compraBL = compraBL;
        }

        public ExcelExportHelper(VentaMinoristaLogic ventaMinoristaBL)
        {
            this.ventaMinoristaBL = ventaMinoristaBL;
        }

        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                //if (showSrNo)
                //{
                //    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                //    dataColumn.SetOrdinal(0);
                //    int index = 1;
                //    foreach (DataRow item in dataTable.Rows)
                //    {
                //        item[0] = index;
                //        index++;
                //    }
                //}


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                //for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                //{
                //    if (i == 0 && showSrNo)
                //    {
                //        continue;
                //    }
                //    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                //    {
                //        workSheet.DeleteColumn(i + 1);
                //    }
                //}

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }

        public DataTable ArmarExcel(string operacion, int flagExcel)
        {


            DataTable tablaExcel = new DataTable();

            //Validamos si es un reporte para Venta Minorista o para Compra
            if (flagExcel == Constants.COMPRA_EXCEL)
            {
                var numeroCompra = operacion.Split(',');

                Compra compraInDB = new Compra();
                CompraReporte reporteTemp = new CompraReporte();
                List<CompraReporte> compraReporte = new List<CompraReporte>();

                //Armamos la lista de compras para el reporte
                foreach (var item in numeroCompra)
                {
                    if (item != string.Empty)
                    {
                        compraInDB = compraBL.GetCompraByNumeroCompra(int.Parse(item));

                        //Guardamos los datos necesarios para el reporte
                        reporteTemp = new CompraReporte();
                        reporteTemp.ID = compraInDB.NumeroCompra;
                        reporteTemp.Nombre = compraInDB.Proveedor.Nombre;
                        reporteTemp.Cuit = compraInDB.Proveedor.Cuit;
                        reporteTemp.Iibb = compraInDB.Proveedor.Iibb;
                        reporteTemp.Fecha = compraInDB.Fecha.Date.ToString("dd/MM/yyyy");
                        reporteTemp.TipoFactura = compraInDB.TipoFactura;
                        reporteTemp.Factura = compraInDB.Factura;
                        reporteTemp.SumaTotal = String.Format("{0:c}", compraInDB.SumaTotal);
                        reporteTemp.DescuentoPorc = compraInDB.DescuentoPorc;
                        reporteTemp.Descuento = String.Format("{0:c}", compraInDB.Descuento);
                        reporteTemp.Subtotal = String.Format("{0:c}", compraInDB.Subtotal);
                        reporteTemp.Iva = compraInDB.Iva;
                        reporteTemp.ImporteIva = String.Format("{0:c}", compraInDB.ImporteIva);
                        reporteTemp.ImporteIibbbsas = String.Format("{0:c}", compraInDB.ImporteIibbbsas);
                        reporteTemp.ImporteIibbcaba = String.Format("{0:c}", compraInDB.ImporteIibbcaba);
                        reporteTemp.ImportePercIva = String.Format("{0:c}", compraInDB.ImportePercIva);
                        reporteTemp.Clasificacion = compraInDB.Clasificacion.Nombre;
                        reporteTemp.Total = String.Format("{0:c}", compraInDB.Total);


                        compraReporte.Add(reporteTemp);
                    }

                }

                //Generamos el datatable correspondiente            
                tablaExcel.Columns.Add("Id", typeof(int));
                tablaExcel.Columns.Add("Proveedor", typeof(string));
                tablaExcel.Columns.Add("CUIT", typeof(string));
                tablaExcel.Columns.Add("IIBB", typeof(string));
                tablaExcel.Columns.Add("Mes Año", typeof(string));
                tablaExcel.Columns.Add("TIPO", typeof(string));
                tablaExcel.Columns.Add("Suma Total", typeof(string));
                tablaExcel.Columns.Add("Descuento %", typeof(double));
                tablaExcel.Columns.Add("Descuento", typeof(string));
                tablaExcel.Columns.Add("Subtotal", typeof(string));
                tablaExcel.Columns.Add("IVA", typeof(string));
                tablaExcel.Columns.Add("Importe IVA", typeof(string));
                tablaExcel.Columns.Add("IIBB BSAS", typeof(string));
                tablaExcel.Columns.Add("IIBB CABA", typeof(string));
                tablaExcel.Columns.Add("Percepción IVA", typeof(string));
                tablaExcel.Columns.Add("Clasificación", typeof(string));
                tablaExcel.Columns.Add("Total", typeof(string));

                foreach (var item in compraReporte)
                {
                    tablaExcel.Rows.Add(item.ID, item.Nombre, item.Cuit, item.Iibb, item.Fecha, item.TipoFactura, item.SumaTotal,
                        item.DescuentoPorc, item.Descuento, item.Subtotal, item.Iva, item.ImporteIva, item.ImporteIibbbsas,
                        item.ImporteIibbcaba, item.ImportePercIva, item.Clasificacion, item.Total);
                }

                //var info = CultureInfo.GetCultureInfo("es-AR");

                //var valor = String.Format(info, "{0:C}", 1332.05);

                // Here we add five DataRows.
                //table.Rows.Add(25, "Indocin", "David", valor);
                //table.Rows.Add(50, "Enebrel", "Sam", valor);
                //table.Rows.Add(10, "Hydralazine", "Christoff", valor);
                //table.Rows.Add(21, "Combivent", "Janet", valor);
                //table.Rows.Add(100, "Dilantin", "Melanie", valor);
                
            }
            else if(flagExcel == Constants.VENTA_MINORISTA_EXCEL)
            {
                var numeroVenta = operacion.Split(',');
                
                VentaMinorista ventaMinoristaInDB = new VentaMinorista();
                VentaMinoristaReporte reporteTemp = new VentaMinoristaReporte();
                List<VentaMinoristaReporte> ventaMinoristaReporte = new List<VentaMinoristaReporte>();

                //Armamos la lista de compras para el reporte
                foreach (var item in numeroVenta)
                {
                    if (item != string.Empty)
                    {
                        ventaMinoristaInDB = ventaMinoristaBL.GetVentaMinoristaByNumeroVenta(int.Parse(item));

                        //Guardamos los datos necesarios para el reporte
                        reporteTemp = new VentaMinoristaReporte();
                        reporteTemp.ID = ventaMinoristaInDB.NumeroVenta;
                        reporteTemp.Fecha = ventaMinoristaInDB.Fecha.Date.ToString("dd/MM/yyyy");
                        reporteTemp.Local = ventaMinoristaInDB.Local;
                        reporteTemp.Importe_Informe_Z = String.Format("{0:c}", ventaMinoristaInDB.ImporteInformeZ);
                        reporteTemp.IVA = Constants.IVA;
                        reporteTemp.Importe_IVA = String.Format("{0:c}", ventaMinoristaInDB.ImporteIva);
                        reporteTemp.Factura_N = ventaMinoristaInDB.NumFactura;
                        reporteTemp.Tipo_Factura = ventaMinoristaInDB.TipoFactura;
                        reporteTemp.Primer_Numero_Tic = ventaMinoristaInDB.PrimerNumeroTicket;
                        reporteTemp.Ultimo_Numero_Tic = ventaMinoristaInDB.UltimoNumeroTicket;



                        ventaMinoristaReporte.Add(reporteTemp);
                    }

                }

                //Generamos el datatable correspondiente            
                tablaExcel.Columns.Add("Id", typeof(int));
                tablaExcel.Columns.Add("Fecha", typeof(string));
                tablaExcel.Columns.Add("Local", typeof(string));
                tablaExcel.Columns.Add("Importe Informe Z", typeof(string));
                tablaExcel.Columns.Add("IVA", typeof(string));
                tablaExcel.Columns.Add("Importe IVA", typeof(string));
                tablaExcel.Columns.Add("Factura nº", typeof(string));
                tablaExcel.Columns.Add("Tipo Factura", typeof(string));
                tablaExcel.Columns.Add("Primer Número Ticket", typeof(string));
                tablaExcel.Columns.Add("Último Número Ticket", typeof(string));

                foreach (var item in ventaMinoristaReporte)
                {
                    tablaExcel.Rows.Add(item.ID, item.Fecha, item.Local, item.Importe_Informe_Z, item.IVA, item.Importe_IVA, item.Factura_N,
                        item.Tipo_Factura, item.Primer_Numero_Tic, item.Ultimo_Numero_Tic);
                }
                
                
            }

            return tablaExcel;


        }

       
    }
}