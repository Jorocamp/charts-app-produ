using Microcharts;
using SkiaSharp;
using Microcharts.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using Npgsql;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
namespace charts
{
    public partial class MainPage : ContentPage
    {

        string ConnectionString = "Server=ec2-107-20-185-27.compute-1.amazonaws.com; Port=5432; User Id=wdxcskrixgrlrg; Password=cf1d8afae86ffe18a9216dac407650b8d67a79d8e9d040e5404c4fa3ff8670d8; Database=d5bugk5ss3gtcc; SSL Mode=Require; Trust Server Certificate=true";
        string[,] colors = { { "#4edb5c", "#18c929", "#03a012", "#18ce79", "#2b6331", "#21c432", "#b3e0b8", "#698c6d", "#414f43", "#7f7f7f" },
        { "#4e60db", "#001291", "#000947", "#8796ff", "#4d558e", "#292d47", "#bfc7fc", "#666a84", "#8f3fb5", "#7f7f7f" },
        { "#db645c", "#8c413c", "#421e1c", "#ffb5b5", "#ff0d00", "#b70900", "#f74238", "#c1837f", "#ff7338", "#7f7f7f" }};
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
                Console.WriteLine(ex.ToString());
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
            string[] param = { "Tareas productivas (TP)", totalTP.ToString(), vlTP, "Tareas contributivas (TC)", totalTC.ToString(), vlTC, "Tareas improductivas (TP)", totalTI.ToString(), vlTI, "Total", total.ToString(), "100%" };
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


                gridTP.Children.Add(h1, 0, row);
                gridTP.Children.Add(h2, 1, row);
                gridTP.Children.Add(h3, 2, row);


            }
            if (task.Equals("2"))
            {

                gridTC.Children.Add(h1, 0, row);
                gridTC.Children.Add(h2, 1, row);
                gridTC.Children.Add(h3, 2, row);

            }
            if (task.Equals("3"))
            {

                gridTI.Children.Add(h1, 0, row);
                gridTI.Children.Add(h2, 1, row);
                gridTI.Children.Add(h3, 2, row);

            }


        }

        //string date, string pro, string impro, string obs, string actPro, string actImpro
        void addDateTable(string[] data, int row) {
           for(int i = 0; i < 6; i++)
            {
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
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();


            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT fecha, count(*) FROM  (operator_registers o INNER JOIN paths p ON p.id = o.path_id) b INNER JOIN activities a ON b.activity_id = a.id  WHERE sampling_id =  " + sampling +" group by fecha order by fecha asc;";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                totalPaths.Add(Int32.Parse(reader[1].ToString()));

            }

            connection.Close();

        }

        async void fillBest(string sampling)
        {
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

        async void fillWorst(string sampling)
        {
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

        async void fillPathsxDays(string sampling)
        {
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


        void IniData(string sampling) {
            getNumberTasks(sampling, "1");
            getNumberTasks(sampling, "2");
            getNumberTasks(sampling, "3");
            getDataCharts(sampling, "1");
            getDataCharts(sampling, "2");
            getDataCharts(sampling, "3");
            getPathsxDay(sampling);
            fillPathsxDays(sampling);


        }




        public MainPage()
        {






            InitializeComponent();
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
