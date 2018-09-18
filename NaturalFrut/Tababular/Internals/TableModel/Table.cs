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
            else
                Columns = columns;


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