using System;
using System.Collections.Generic;
using System.Linq;

namespace Tababular.Internals.TableModel
{
    class Table
    {
        public Table(List<Column> columns, List<Row> rows)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (rows == null) throw new ArgumentNullException(nameof(rows));

            Rows = rows;
            //Columns = columns;
            Columns = new List<Column>();

            if(columns[2].Label == "ID")
            {
                //Se trata de un reporte de venta minorista, con los valores a hardcodear asociados
                columns[3].Label = "Importe Informe Z";
                columns[4].Label = "Importe IVA";
                columns[0].Label = "Factura nº";
                columns[0].Width = 12;
                columns[8].Label = "Tipo Factura";
                columns[7].Label = "Primer_Número_Tic";
                columns[9].Label = "Último_Número_Tic";

                //Ordenamos las columnas
                Columns.Add(columns[2]);
                Columns.Add(columns[1]);
                Columns.Add(columns[6]);
                Columns.Add(columns[3]);
                Columns.Add(columns[5]);
                Columns.Add(columns[4]);
                Columns.Add(columns[0]);
                Columns.Add(columns[8]);
                Columns.Add(columns[7]);
                Columns.Add(columns[9]);
            }
            else if(columns[6].Label == "ID")
            {

                //Se trata de un reporte de venta minorista, con los valores a hardcodear asociados
                columns[6].Label = "Id";
                columns[13].Label = "Proveedor";
                columns[1].Label = "CUIT";
                columns[7].Label = "IIBB";
                columns[5].Label = "Mes_año";
                columns[16].Label = "TIPO";
                columns[15].Label = "Suma Total";
                columns[3].Label = "DESCUENTO %";
                columns[2].Label = "DESCUENTO";
                columns[14].Label = "SUBTOTAL";
                columns[3].Label = "DESCUENTO %";
                columns[12].Label = "IVA";
                columns[10].Label = "IMPORTE IVA";
                columns[8].Label = "IIBB BSAS";
                columns[9].Label = "IIBB CABA";
                columns[11].Label = "PERCEP IVA";
                columns[0].Label = "Clasificación";
                columns[17].Label = "TOTAL";


                //Ordenamos las columnas
                Columns.Add(columns[6]);
                Columns.Add(columns[13]);
                Columns.Add(columns[1]);
                Columns.Add(columns[7]);
                Columns.Add(columns[5]);
                Columns.Add(columns[16]);
                Columns.Add(columns[4]);
                Columns.Add(columns[15]);
                Columns.Add(columns[3]);
                Columns.Add(columns[2]);
                Columns.Add(columns[14]);
                Columns.Add(columns[12]);
                Columns.Add(columns[10]);
                Columns.Add(columns[8]);
                Columns.Add(columns[9]);
                Columns.Add(columns[11]);
                Columns.Add(columns[0]);
                Columns.Add(columns[17]);


            }
            else{

                Columns = columns;
            }
                


            foreach (var column in Columns)
            {
                foreach (var row in Rows)
                {
                    var cellOrNull = row.GetCellOrNull(column);

                    if (cellOrNull == null) continue;

                    column.AdjustWidth(cellOrNull);
                }
            }

        }

        public List<Column> Columns { get; }
        public List<Row> Rows { get; }

        public bool HasCellWith(Func<Cell, bool> cellPredicate)
        {
            return Rows.SelectMany(r => r.GetAllCells()).Any(cellPredicate);
        }
    }
}