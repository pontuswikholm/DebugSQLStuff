using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DebugSQLStuff
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString;
            SqlConnection con;

            connectionString = @"Data Source=192.168.11.11\SQLEXPRESS; Database=HouseManufacturingData; User ID=PONTUS;Password=warcraft3;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(connectionString);

            con.Open();
            MessageBox.Show("Connection open !");

            SqlCommand command;
            SqlDataReader dataReader;
            String sql;
            String output = "";
            String ProjectName = "TopCat_2021-03 DualBoxProj";
            Guid guid = new Guid();

            sql = (@$"Select ID from ordModularProjects where ProjectName='{ ProjectName }'");
            command = new SqlCommand(sql, con);

            dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    guid = dataReader.GetGuid(0);
                }
            }
            

            output = guid.ToString();

            Console.WriteLine(output);
            MessageBox.Show(output);

            dataReader.Close();
            command.Dispose();
            con.Close();
        }
    }
}
