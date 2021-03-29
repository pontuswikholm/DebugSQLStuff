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

            Single DimTopBottom = 72;
            Single DimMainStuds = 44;
            Single DimSecondaryStuds = 72;
            Single DimStandardSheets = 1200;
            Single LengthSpecialFirstAndLastSheets = 839;
            Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT = 2;
            Single ExtraStud1Xpos = 3310;
            Single ExtraStud2Xpos = 5683;
            bool EW1GableHasAWindow = true;
            bool EW1GableHasExtraStuds = true;

            //Board info needed to create a Board!
            Guid BoardTypeID = Guid.NewGuid();
            Guid ParentID = Guid.NewGuid();
            String Description;
            Single Length;
            int RotationX;
            int RotationY;
            int RotationZ;
            Single PositionX;
            Single PositionY;
            Single PositionZ;


            //General rules for EW1 Gable --> Each and every stud is of secondary dimmension except for the first two studs (standard) and the closest studs to the SUBELEMENT is the secondary type.



            //**** Exception will be that one stud in the real data from TopHat in EW1 Gable does not apply to the each and every rule. ****//

            // 1. Create first main stud                                                            [44]
            //      1.1 Xpos --> 0
            // 2. Create second main stud                                                           [44]
            //      2.1 Xpos --> 1.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 3. Create final stud att end of LengthSpecialFirstAndLastSheets                      [72]
            //      3.1 Xpos --> LengthSpecialFirstAndLastSheets - DimThisStud/2
            // 4. Create first stud under standard sheets                                           [44]
            //      4.1 XPos --> 3.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 5. Create next stud                                                                  [72]
            //      5.1 XPos --> 4.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 6. Create next stud                                                                  [44]    <---------- Exception mentioned above
            //      6.1 XPos --> 5.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 7. Create next stud                                                                  [72]   
            //      7.1 XPos --> 6.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 8. Create next stud                                                                  [44]   
            //      8.1 XPos --> 7.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 9. Create next stud                                                                  [72]   
            //      9.1 XPos --> 8.1 Xpos + DimLastStud/2 + (3 * DimStandardSheets/2) - DimThisStud/2
            // 10. Create next stud                                                                 [44]   
            //      10.1 XPos --> 9.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 11. Create next stud                                                                 [72]   
            //      11.1 XPos --> 10.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 12. Create next stud                                                                 [44]   
            //      12.1 XPos --> 11.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 13. Create next stud                                                                 [72]   
            //      13.1 XPos --> 12.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 14. Create next stud                                                                 [44]   
            //      14.1 XPos --> 13.1 Xpos + DimLastStud/2 + (LengthSpecialFirstAndLastSheets - DimStandardSheets/2 - DimThisStud)
            // 15. Create next stud                                                                 [44]   
            //      15.1 XPos --> 14.1 Xpos + DimLastStud/2 + DimStandardSheets/2 - DimThisStud/2
            // 16. Create extra stud
            //      16.1 XPos --> ExtraStud1Xpos
            // 17. Create extra stud
            //      17.1 XPos --> ExtraStud2Xpos

            while (true)
            {

            }

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
