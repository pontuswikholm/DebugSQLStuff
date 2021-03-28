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
            //----Variable Definitions----

            //Define a connection to the SQL DB
            SqlConnection con;
            con = new SqlConnection(@"Data Source=192.168.11.11\SQLEXPRESS; Database=HouseManufacturingData; User ID=PONTUS;Password=warcraft3;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            //SQL Miscellaneous
            SqlCommand command;
            SqlDataReader dataReader;
            SqlDataAdapter adapter = new SqlDataAdapter();
            String output = "";
            String sqlGetProjId;
            String sqlCreateNewProject;
            String sqlCreateModule;

            //Query Input
            String ProjectName = "TopHat_2021-03 DualBoxProj";
            String CustomerName = "TopHat";
            String ModuleName = "M1FF";
            DateTime ModuleCreationDate = DateTime.Now;
            Single BoundsLength = 9168;
            Single BoundsWidth = 4472;      //Kalle hur skall jag hantera decimaler efter kommatecken? (skall vara .5 på bredden...)
            Single BoundsHight =2484;

            //Query Output
            Guid guidProjId = new Guid();

            //----Process Code----

            //Open connection to DB
            con.Open();

            //Get ProjectID
            sqlGetProjId = (@$"Select ID from ordModularProjects where ProjectName='{ ProjectName }'");
            command = new SqlCommand(sqlGetProjId, con);

            dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    guidProjId = dataReader.GetGuid(0);
                }
            }
            output = guidProjId.ToString();

            //Write out string REMOVE LATER
            Console.WriteLine(output);

            //Close this session
            dataReader.Close();
            command.Dispose();
            con.Close();

            //Create new project if not exist
            if (guidProjId == Guid.Empty)
            {
                con.Open();
                sqlCreateNewProject = (@$"INSERT INTO ordModularProjects (ProjectName,CustomerName) VALUES ('{ ProjectName }', '{ CustomerName }')");
                command = new SqlCommand(sqlCreateNewProject, con);

                adapter.InsertCommand = new SqlCommand(sqlCreateNewProject, con);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
                adapter.Dispose();
                con.Close();
            }

            //Get ProjectID again..
            sqlGetProjId = (@$"Select ID from ordModularProjects where ProjectName='{ ProjectName }'");
            con.Open();
            command = new SqlCommand(sqlGetProjId, con);
            dataReader = command.ExecuteReader();
            

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    guidProjId = dataReader.GetGuid(0);
                }
            }
            output = guidProjId.ToString();
            command.Dispose();
            adapter.Dispose();
            con.Close();

            //CreateModule

            sqlCreateModule = (@$"INSERT INTO matModules (ProjectID, Label, CreationDate, BoundsLength, BoundsWidth, BoundsHight) VALUES ('{ guidProjId }', '{ ModuleName }', '{ ModuleCreationDate }', { BoundsLength }, { BoundsWidth }, { BoundsHight })");
            con.Open();
            command = new SqlCommand(sqlCreateModule, con);           
            adapter.InsertCommand = new SqlCommand(sqlCreateModule, con);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            adapter.Dispose();

            con.Close();
        }
    }
}
