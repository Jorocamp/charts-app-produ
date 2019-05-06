using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using Npgsql;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using System.IO;
using Xamarin.Essentials;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using DocumentFormat.OpenXml;
using System.Linq;

namespace charts
{
    public partial class MainPage : ContentPage
    {

        string ConnectionString = "Server=ec2-107-20-185-27.compute-1.amazonaws.com; Port=5432; User Id=wdxcskrixgrlrg; Password=cf1d8afae86ffe18a9216dac407650b8d67a79d8e9d040e5404c4fa3ff8670d8; Database=d5bugk5ss3gtcc; SSL Mode=Require; Trust Server Certificate=true";
        string[,] colors = { { "#827d05", "#d3ce58", "#d8d6a6", "#0b8276", "#59c6bb", "#85b5b0", "#320d7c", "#6840b7", "#8b79af", "#303030" },
        { "#aa7211", "#e5b35e", "#968261", "#b20cac", "#c94ec5", "#cc9bca", "#119bdb", "#5890aa", "#8eb8cc", "#303030" },
        { "#0a9b0a", "#2e5e2e", "#a1c9a1", "#081191", "#434ccc", "#8d90c4", "#d80d0d", "#562b2b", "#e09191", "#303030" }};
        List<int> totalPaths = new List<int>();
        List<string> bestDays = new List<string>();
        List<string> worstDays = new List<string>();

        List<Entry> tpList = new List<Entry> ();
        List<Entry> tcList = new List<Entry> ();
        List<Entry> tiList = new List<Entry> ();
        List<Entry> dayList = new List<Entry>();
        

        int totalTP;
        int totalTI;
        int totalTC;

        List<string> generalData = new List<string>();
        List<string> tpDataA = new List<string>();
        List<string> tcDataA = new List<string>();
        List<string> tiDataA = new List<string>();
        List<string> daysData = new List<string>();




        string nombreMuestreo;
        string tipoActividad;
        string nombreProyecto;
        string desMuestreo;
        string idMuestreo;



        async void getNumberTasks(string sampling, string activity)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT count(*) FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id = "+ sampling +" AND activity_type_id = "+activity+";";

                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
               
                if(activity.Equals("1"))
                    totalTP = Int32.Parse(reader[0].ToString());
                if (activity.Equals("2"))
                    totalTC = Int32.Parse(reader[0].ToString());
                if (activity.Equals("3"))
                    totalTI = Int32.Parse(reader[0].ToString());

                connection.Close();
            }
            catch (Exception ex)
            {
                titulo.Text ="Error de conexión";
            }


        }



        async void getDataCharts(string muestreo, string activity) {
            try
            {

                createTasksTableHeader("1");
                createTasksTableHeader("2");
                createTasksTableHeader("3");
                int t1 = 1, t2 = 1, t3 = 1;
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();


                int indxColor = Int32.Parse(activity);
                indxColor--;

                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT nombre, count(*) FROM (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id = " + muestreo + " AND activity_type_id = "+ activity +"  group by nombre order by count desc;";

                NpgsqlDataReader reader = command.ExecuteReader();

                int counter = 0;
                int othersSum = 0;
                while (reader.Read())
                {

                    if (counter < 9)
                    {
                        Entry temp = new Entry(Int32.Parse(reader[1].ToString()));
                        temp.Label = reader[0].ToString();
                        temp.Color = SKColor.Parse(colors[indxColor, counter]);
                        int num = Int32.Parse(reader[1].ToString());
                        if (activity.Equals("1"))
                        {

                            temp.ValueLabel = Math.Round(((double)num * 100.0 / (double)totalTP), 2).ToString() + "%";
                            addTaskTable(temp.Label, num.ToString(), temp.ValueLabel, "1", t1);
                            t1++;
                            tpList.Add(temp);
                        }
                        if (activity.Equals("2"))
                        {
                            temp.ValueLabel = Math.Round(((double)num * 100.0 / (double)totalTC), 2).ToString() + "%";
                            addTaskTable(temp.Label, num.ToString(), temp.ValueLabel, "2", t2);
                            t2++;
                            tcList.Add(temp);
                        }
                        if (activity.Equals("3"))
                        {
                            temp.ValueLabel = Math.Round(((double)num * 100.0 / (double)totalTI), 2).ToString() + "%";
                            addTaskTable(temp.Label, num.ToString(), temp.ValueLabel, "3", t3);
                            t3++;
                            tiList.Add(temp);
                        }
                        counter++;


                    }
                    else
                    {
                        othersSum += Int32.Parse(reader[1].ToString());
                        if (activity.Equals("1"))
                        {
                            int num = Int32.Parse(reader[1].ToString());
                            addTaskTable(reader[0].ToString(), reader[1].ToString(), Math.Round(((double)num * 100.0 / (double)totalTP), 2).ToString() + "%", "1", t1);
                            t2++;

                        }
                        if (activity.Equals("2"))
                        {
                            int num = Int32.Parse(reader[1].ToString());
                            addTaskTable(reader[0].ToString(), reader[1].ToString(), Math.Round(((double)num * 100.0 / (double)totalTC), 2).ToString() + "%", "2", t2);
                            t2++;

                        }
                        if (activity.Equals("3"))
                        {
                            int num = Int32.Parse(reader[1].ToString());
                            addTaskTable(reader[0].ToString(), reader[1].ToString(), Math.Round(((double)num * 100.0 / (double)totalTI), 2).ToString() + "%", "3", t3);
                            t3++;

                        }


                    }

                    }
                    Entry other = new Entry(othersSum);
                    other.Label = "Otros";
                    other.Color = SKColor.Parse(colors[indxColor, counter]);
                    if (activity.Equals("1"))
                    {

                        other.ValueLabel = Math.Round(((double)othersSum * 100.0 / (double)totalTP), 2).ToString() + "%";
                        tpList.Add(other);
                    }
                    if (activity.Equals("2"))
                    {
                        other.ValueLabel = Math.Round(((double)othersSum * 100.0 / (double)totalTC), 2).ToString() + "%";
                        tcList.Add(other);
                    }
                    if (activity.Equals("3"))
                    {
                        other.ValueLabel = Math.Round(((double)othersSum * 100.0 / (double)totalTI), 2).ToString() + "%";
                        tiList.Add(other);
                    }
                


                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



        }

        List<Entry> generalChart()
        {
            int total = totalTC + totalTI + totalTP;
            string vlTP = Math.Round(((double)totalTP * 100.0 / (double)total), 2).ToString() + "%";
            string vlTC = Math.Round(((double)totalTC * 100.0 / (double)total), 2).ToString() + "%";
            string vlTI = Math.Round(((double)totalTI * 100.0 / (double)total), 2).ToString() + "%";
            string[] param = { "Tareas productivas (TP)", totalTP.ToString(), vlTP, "Tareas contributivas (TC)", totalTC.ToString(), vlTC, "Tareas improductivas (TI)", totalTI.ToString(), vlTI, "Total", total.ToString(), "100%" };
            generalData = param.OfType<string>().ToList();
           
            createGeneralTableHeader();
            createGeneralTable(param);
            return new List<Entry> {
                 new Entry(totalTP)
                 {
                     Label = "Productivas",
                     ValueLabel = vlTP,
                     Color = SKColor.Parse("#4edb5c")
                 },
                  new Entry(totalTC)
                 {
                     Label = "Colaborativas",
                     ValueLabel = vlTC,
                     Color = SKColor.Parse("#4e60db")
                 },
                   new Entry(totalTI)
                 {
                     Label = "No productivas",
                     ValueLabel = vlTI,
                     Color = SKColor.Parse("#db645c")
                 },



            };
        }


        void createGeneralTableHeader() {

            //Headers


            var h1 = new Label
            {
                Text = "Tarea",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h2 = new Label
            {
                Text = "Total",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h3 = new Label
            {
                Text = "Porcentaje",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            gridPG.Children.Add(h1, 0, 0);
            gridPG.Children.Add(h2, 1, 0);
            gridPG.Children.Add(h3, 2, 0);


        }

        void createGeneralTable(string[] labels)
        {


          
            int cont = 0;
            for (int i = 1; i <= 4; i++) {
                
                for (int j = 0; j <= 2; j++)
                {

                    var temp = new Label {
                        Text = labels[cont],
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    cont++;
                    gridPG.Children.Add(temp, j, i);
                }
            }


        }

        void createTasksTableHeader(string task)
        {

            //Headers


            var h1 = new Label
            {
                Text = "Tarea",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h2 = new Label
            {
                Text = "Total",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h3 = new Label
            {
                Text = "Porcentaje",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            if (task.Equals("1")) {


                gridTP.Children.Add(h1, 0, 0);
                gridTP.Children.Add(h2, 1, 0);
                gridTP.Children.Add(h3, 2, 0);


            }
            if (task.Equals("2"))
            {

                gridTC.Children.Add(h1, 0, 0);
                gridTC.Children.Add(h2, 1, 0);
                gridTC.Children.Add(h3, 2, 0);

            }
            if (task.Equals("3"))
            {

                gridTI.Children.Add(h1, 0, 0);
                gridTI.Children.Add(h2, 1, 0);
                gridTI.Children.Add(h3, 2, 0);

            }


        }

        void addTaskTable(string tarea, string total, string porcentaje,string task, int row)
        {

            //Headers


            var h1 = new Label
            {
                Text = tarea,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h2 = new Label
            {
                Text = total,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var h3 = new Label
            {
                Text = porcentaje,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            if (task.Equals("1"))
            {

                tpDataA.Add(tarea);
                tpDataA.Add(total);
                tpDataA.Add(porcentaje);
                gridTP.Children.Add(h1, 0, row);
                gridTP.Children.Add(h2, 1, row);
                gridTP.Children.Add(h3, 2, row);


            }
            if (task.Equals("2"))
            {
                tcDataA.Add(tarea);
                tcDataA.Add(total);
                tcDataA.Add(porcentaje);
                gridTC.Children.Add(h1, 0, row);
                gridTC.Children.Add(h2, 1, row);
                gridTC.Children.Add(h3, 2, row);

            }
            if (task.Equals("3"))
            {

                tiDataA.Add(tarea);
                tiDataA.Add(total);
                tiDataA.Add(porcentaje);
                gridTI.Children.Add(h1, 0, row);
                gridTI.Children.Add(h2, 1, row);
                gridTI.Children.Add(h3, 2, row);

            }


        }

        //string date, string pro, string impro, string obs, string actPro, string actImpro
        void addDateTable(string[] data, int row) {
           for(int i = 0; i < 6; i++)
            {
                daysData.Add(data[i]);
                var temp = new Label
                {
                    Text = data[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                gridD.Children.Add(temp, i, row);
            }
        }



        async void getPathsxDay(string sampling) {

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();


                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT fecha, count(*) FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id =  " + sampling + " group by fecha order by fecha asc;";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    totalPaths.Add(Int32.Parse(reader[1].ToString()));

                }

                connection.Close();
            }
            catch(Exception e)
            {
                titulo.Text = "Error de conexión";
            }

        }

        async void fillBest(string sampling)
        {

            try { 
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();


            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT fecha, nombre, count FROM (SELECT fecha, nombre, count(nombre), ROW_NUMBER() OVER (PARTITION BY fecha  ORDER BY count(nombre) DESC) num  FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id = " + sampling +" AND activity_type_id = 1 group by fecha, nombre) act WHERE num = 1;";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bestDays.Add(reader[1].ToString());

            }

            connection.Close();
            }
            catch (Exception ex)
            {
                titulo.Text = "Error de conexión";
            }

        }

        async void fillWorst(string sampling)
        {
            try { 
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();


            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT fecha, nombre, count FROM (SELECT fecha, nombre, count(nombre), ROW_NUMBER() OVER (PARTITION BY fecha  ORDER BY count(nombre) DESC) num  FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id = "+sampling+" AND NOT activity_type_id = 1 group by fecha, nombre) act WHERE num = 1 ORDER BY fecha asc;";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                worstDays.Add(reader[1].ToString());

            }

            connection.Close();
            }
            catch (Exception ex)
            {
                titulo.Text = "Error de conexión";
            }

        }

        async void fillPathsxDays(string sampling)
        {
            try { 
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            fillBest(sampling);
            fillWorst(sampling);
            string[] header = { "Fecha", "TP", "TI", "Observaciones", "TP Destacada", "TI Destacada" };
            addDateTable(header, 0);


            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT fecha, count(*) FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id =  " + sampling + " AND activity_type_id = 1 group by fecha order by fecha asc;";
            
            NpgsqlDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {

                Entry temp = new Entry(Int32.Parse(reader[1].ToString()));
                string lab = reader[0].ToString();
                var indx = lab.IndexOf(" ");
                temp.Label = lab.Substring(0, indx);
                temp.Color = SKColor.Parse("#1155c1");
                int tc = Int32.Parse(reader[1].ToString());
                int ti = totalPaths[count] - tc;
                temp.ValueLabel = Math.Round(((double)tc * 100.0 / (double)totalPaths[count]), 2).ToString() + "%";
                string tiTot = Math.Round(((double)ti * 100.0 / (double)totalPaths[count]), 2).ToString() + "%";
                dayList.Add(temp);
                string[] row = {  temp.Label, temp.ValueLabel , tiTot, totalPaths[count].ToString(), bestDays[count], worstDays[count] };
                addDateTable(row, count + 1);

                count++;


            }
            connection.Close();
        }
            catch (Exception ex)
            {
                titulo.Text = "Error de conexión";
            }

}
        async void getNames(string sampling)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();


                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from (select n.id,n.samp,n.tamp,p.nombre, n.descripcion from ( select s.id, s.nombre as samp, t.nombre as tamp, s.project_id, s.descripcion from samplings s inner join sampling_types t on t.id = s.sampling_type_id) n inner join projects p on p.id=n.project_id) f where id = "+sampling+";";

                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();

                nombreMuestreo = reader[1].ToString();
                tipoActividad = reader[2].ToString();
                nombreProyecto = reader[3].ToString();
                desMuestreo = reader[4].ToString();

                connection.Close();
            }
            catch (Exception ex)
            {
                titulo.Text = "Error de conexión";
            }

        }



        void IniData(string sampling) {
            getNumberTasks(sampling, "1");
            getNumberTasks(sampling, "2");
            getNumberTasks(sampling, "3");
            getDataCharts(sampling, "1");
            getDataCharts(sampling, "2");
            getDataCharts(sampling, "3");
            getPathsxDay(sampling);
            fillPathsxDays(sampling);
            getNames(sampling);
            idMuestreo = sampling;

            titulo.Text = nombreMuestreo + " (" + tipoActividad + ")";


        }



        public async Task SendEmail(string subject, string body)
        {
            try
            {
                
                var message = new EmailMessage
                {
                        Subject = subject,
                        Body = body,
                        //To = recipients,
                        //Cc = ccRecipients,
                        //Bcc = bccRecipients
                };



                    message.Attachments.Add(new EmailAttachment(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), idMuestreo + "_" + tipoActividad + ".xlsx")));

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                Console.WriteLine(fbsEx);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                // Some other exception occurred
            }
        }



        //GenerarFilas para EXCEL

        private string GetExcelColumnName(uint columnNumber)
        {
            uint dividend = columnNumber;
            string columnName = String.Empty;
            uint modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (uint)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private Cell InsertCell(uint rowIndex, uint columnIndex, Worksheet worksheet, string value)
        {
            Row row = null;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            // Check if the worksheet contains a row with the specified row index.
            row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // Convert column index to column name for cell reference.
            var columnName = GetExcelColumnName(columnIndex);
            var cellReference = columnName + rowIndex;      // e.g. A1

            // Check if the row contains a cell with the specified column name.
            var cell = row.Elements<Cell>()
                       .FirstOrDefault(c => c.CellReference.Value == cellReference);
            if (cell == null)
            {
                if (int.TryParse(value, out int n))
                {
                    cell = new Cell() { CellReference = cellReference, CellValue = new CellValue(value), DataType = CellValues.Number };
                }
                else if (value[value.Length - 1] == '%') {
                    value = value.Substring(0, value.Length - 1);
                    double result;
                    result = Convert.ToDouble(value);
                    result = result / 100.00;
                    value = result.ToString();
                    cell = new Cell() { CellReference = cellReference, CellValue = new CellValue(value), DataType = CellValues.Number };
                }
                else
                {
                    cell = new Cell() { CellReference = cellReference, CellValue = new CellValue(value), DataType = CellValues.String };
                }
                if (row.ChildElements.Count < columnIndex)
                    row.AppendChild(cell);
                else
                    row.InsertAt(cell, (int)columnIndex);
            }

            return cell;
        }

        //Excel Chart




        //Create table
        private void createTable(Worksheet wrk, uint fr, uint fc, uint totC, List<string> values) {

            uint cont = 0;
            uint ront = 0;
            for(int i = 0; i < values.Count; i++)
            {
                if(cont >= totC)
                {
                    cont = 0;
                    ront++;
                }
                InsertCell(fr+ront, fc+cont, wrk, values[i]);
                cont++;
            }


        }



        async void OnButtonClicked(object sender, EventArgs args)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), idMuestreo + "_" + tipoActividad + ".xlsx"), SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Consolidacion del muestreo"
                };

                //Proyect Data
                InsertCell(1, 1, worksheetPart.Worksheet, "Datos del Muestreo");
                InsertCell(2, 1, worksheetPart.Worksheet, "Nombre del Proyecto: ");
                InsertCell(3, 1, worksheetPart.Worksheet, "Nombre del Muestreo: ");
                InsertCell(4, 1, worksheetPart.Worksheet, "Actividad: ");
                InsertCell(5, 1, worksheetPart.Worksheet, "ID Muestreo: ");
                InsertCell(6, 1, worksheetPart.Worksheet, "Descripcion: ");

                InsertCell(2, 2, worksheetPart.Worksheet, nombreProyecto);
                InsertCell(3, 2, worksheetPart.Worksheet, nombreMuestreo);
                InsertCell(4, 2, worksheetPart.Worksheet, tipoActividad);
                InsertCell(5, 2, worksheetPart.Worksheet, idMuestreo);
                InsertCell(6, 2, worksheetPart.Worksheet, desMuestreo);

                //Name of tables
                InsertCell(1, 4, worksheetPart.Worksheet, "Porcentaje general de tareas");
                InsertCell(1, 8, worksheetPart.Worksheet, "Porcentaje para tareas productivas");
                InsertCell(1, 12, worksheetPart.Worksheet, "Porcentaje para tareas contributivas");
                InsertCell(1, 16, worksheetPart.Worksheet, "Porcentaje para tareas no productivas");
                InsertCell(1, 20, worksheetPart.Worksheet, "Productividad por día");


                //General table (Header and then data)
                InsertCell(2, 4, worksheetPart.Worksheet, "Tarea");
                InsertCell(2, 5, worksheetPart.Worksheet, "Total");
                InsertCell(2, 6, worksheetPart.Worksheet, "Porcentaje");
                createTable(worksheetPart.Worksheet, 3, 4, 3, generalData);

                //TP table (Header and then data)
                InsertCell(2, 8, worksheetPart.Worksheet, "Tarea");
                InsertCell(2, 9, worksheetPart.Worksheet, "Total");
                InsertCell(2, 10, worksheetPart.Worksheet, "Porcentaje");
                createTable(worksheetPart.Worksheet, 3, 8, 3, tpDataA);
                //TC table (Header and then data)
                InsertCell(2, 12, worksheetPart.Worksheet, "Tarea");
                InsertCell(2, 13, worksheetPart.Worksheet, "Total");
                InsertCell(2, 14, worksheetPart.Worksheet, "Porcentaje");
                createTable(worksheetPart.Worksheet, 3, 12, 3, tcDataA);
                //TI table (Header and then data)
                InsertCell(2, 16, worksheetPart.Worksheet, "Tarea");
                InsertCell(2, 17, worksheetPart.Worksheet, "Total");
                InsertCell(2, 18, worksheetPart.Worksheet, "Porcentaje");
                createTable(worksheetPart.Worksheet, 3, 16, 3, tiDataA);
                //Days table (Header and then data)
                createTable(worksheetPart.Worksheet, 2,20, 6, daysData);

                //General Chart
                DrawingsPart drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
                worksheetPart.Worksheet.Append(new DocumentFormat.OpenXml.Spreadsheet.Drawing() { Id = worksheetPart.GetIdOfPart(drawingsPart) });
                ExcelCharts.CreatePieChart(drawingsPart, "'Consolidacion del muestreo'!$D$3:$D$5", "'Consolidacion del muestreo'!$E$3:$E$5",8, true, "General");
                ExcelCharts.CreatePieChart(drawingsPart, "'Consolidacion del muestreo'!$H$3:$H$"+(2+ tpDataA.Count/3).ToString(), "'Consolidacion del muestreo'!$I$3:$I$"+(2 + tpDataA.Count / 3).ToString(), 24, false, "TP");
                ExcelCharts.CreatePieChart(drawingsPart, "'Consolidacion del muestreo'!$L$3:$L$" + (2 + tcDataA.Count / 3).ToString(), "'Consolidacion del muestreo'!$M$3:$M$" + (2 + tcDataA.Count / 3).ToString(), 40, false, "TC");
                ExcelCharts.CreatePieChart(drawingsPart, "'Consolidacion del muestreo'!$P$3:$P$" + (2 + tiDataA.Count / 3).ToString(), "'Consolidacion del muestreo'!$Q$3:$Q$" + (2 + tiDataA.Count / 3).ToString(), 56, false, "TI");
                ExcelCharts.CreateLineChart(drawingsPart, "'Consolidacion del muestreo'!$T$3:$T$" + (2 + (daysData.Count - 6) / 6).ToString(), "'Consolidacion del muestreo'!$U$3:$U$" + (2 + (daysData.Count - 6) / 6).ToString(), 72, false, "Productividad x Dia");

                sheets.Append(sheet);
                
                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();


            }


           // string pathN = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), idMuestreo + "_" + tipoActividad + ".xlsx");
           // ExcelCharts.CreatePieChart(pathN, 3, "'Consolidacion del muestreo'!$D$3:$D$5", "'Consolidacion del muestreo'!$E$3:$E$5", 8, 2);


            ExperimentalFeatures.Enable(ExperimentalFeatures.EmailAttachments);

           SendEmail("MAC - Proyecto ("+nombreProyecto+") - Datos del muestreo ("+nombreMuestreo+")", "Mensaje enviado usando el app móvil de Muestreo de Actividades Constructivas (MAC) \n Nombre del Proyecto: " + nombreProyecto + "\n Nombre del Muestreo: " + nombreMuestreo  + "\n ID muestreo: " + idMuestreo + "\n Tipo Actividad: " + tipoActividad + "\n Descripcion: " + desMuestreo);
        }


        public MainPage()
        {
            
            InitializeComponent();
            BindingContext = this;

            IniData("24");
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);

            Chart1.Chart = new DonutChart() { Entries = generalChart() };
            Chart2.Chart = new DonutChart() { Entries = tpList,
                HoleRadius = 0.5f,
            };
            Chart3.Chart = new DonutChart()
            {
                Entries = tcList,
                HoleRadius = 0.5f,
            };
            Chart4.Chart = new DonutChart()
            {
                Entries = tiList,
                HoleRadius = 0.5f,
            };
            Chart5.Chart = new LineChart()
            {
                Entries = dayList,
                
            };
        }
    }
}
