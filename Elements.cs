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

namespace ModuleDataHandeler
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

	public class Main
    {

		SQLConnectionMethods sql = new SQLConnectionMethods();
		ModularProject modularProject = new ModularProject();
		

		public Main()
        {
			Module module = new Module();
		
			module.Elements.Add(new EW3FRONT());
			module.Elements.Add(new EW4BACK());
			module.Elements.Add(new EW1GABLE());
			module.Elements.Add(new EW2PARTY());

			//Prepare Data for Module
			PrepareData(module);

			//Create Module

			//Create Elements

			//Create Boards

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

		}

		public Module PrepareData(Module module)
        {

			modularProject.ProjectName = "TopHat_2021-03 DualBoxProj";
			modularProject.CustomerName = "TopHat";
			module.BoundsLength = 9168;
			module.BoundsWidth = 4472.5F;      //Hur skall vi hantera decimaler efter kommatecken? (skall vara .5 på bredden men verkar strula med sql-insert..)
			module.BoundsHight = 2484;
			module.Label = "M1FF";
			module.CreationDate = DateTime.Now;

			BoardType boardType = new BoardType();
			


			foreach (var e in module.Elements)
            {

				e.ModuleID = module.ID;
				e.ElementTypeID = Guid.Parse("664b2f1c-34f8-4e11-aa56-187b5abcc21a");
				e.ParentID = module.ID;
				
				switch (e)
                {
					case EW1GABLE:
						e.Label = "EW1GABLE";
						e.Description = "External LongWall Wallno-->1 Name-->Gable";
						e.BoundsLength = module.BoundsLength - (e.DimBoardsWidth*2);
						e.BoundsHight = module.BoundsHight;
						e.BoundsWidth = e.DimBoardsWidth;
						
						boardType.Width = 145;
						// Add studs (Loop as many times as studs can be fit into a element length)
						for (int studs = 0; studs < (e.BoundsLength / 600); studs++)
						{
							e.Boards.Add(new Board { 
								ID = Guid.NewGuid()
								, BoardTypeID = boardType.ID
								, ParentID = e.ID
								, Description = "Stud"
								, Length = e.BoundsHight - (e.DimTopBottom * 2)
								, RoatationX = 0, RoatationY = 0, RoatationZ = -90
								, PositionX = (studs * 600) + e.DimMainStuds, PositionY = e.DimTopBottom, PositionZ = 0 };

						}

						// Add top and bottom plate

						break;
					case EW2PARTY:
						e.Label = "EW2PARTY";
						e.Description = "External LongWall Wallno-->2 Name-->Party";
						e.BoundsLength = module.BoundsLength - (e.DimBoardsWidth * 2);
						e.BoundsHight = module.BoundsHight;
						e.BoundsWidth = e.DimBoardsWidth;
						break;
					case EW3FRONT:
						e.Label = "EW3FRONT";
						e.Description = "External ShortWall Wallno-->3 Name-->Front";
						e.BoundsLength = module.BoundsWidth;
						e.BoundsHight = module.BoundsHight;
						e.BoundsWidth = e.DimBoardsWidth;
						break;
					case EW4BACK:
						e.Label = "EW4BACK";
						e.Description = "External ShortWall Wallno-->4 Name-->Back";
						e.BoundsLength = module.BoundsWidth;
						e.BoundsHight = module.BoundsHight;
						e.BoundsWidth = e.DimBoardsWidth;
						break;


				}



			}

			return module;
        }

		public Module InserAllModuleDataIntoSQL(Module module)
        {
			//Get Project Id		
			modularProject.ID = sql.GetProjectID(modularProject.ProjectName);

			//Create new project if not exist
			if (modularProject.ID == Guid.Empty)
			{
				sql.CreateModularProject(modularProject.ProjectName, modularProject.CustomerName);
				modularProject.ID = sql.GetProjectID(modularProject.ProjectName);
			}

			module.ProjectID = modularProject.ID;

			sql.CreateModule(module.ProjectID, module.Label, module.CreationDate, module.BoundsLength, module.BoundsWidth, module.BoundsHight);

			module.ID = sql.GetModuleID(module.Label);

			return module;
		}

	}

    public class EW1GABLE : IElement //External Wall 1 Gable
	{
		public Guid ID { get; set; }						//Dont fill
		public Guid ModuleID { get; set; }
		public Guid ElementTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
		public Single ZPosMeters { get; set; }
		public List<Board> Boards { get; set; }
		public List<IElement> IElements { get; set; }
		public Single DimTopBottom { get; set; }
		public Single DimMainStuds { get; set; }
		public Single DimSecondaryStuds { get; set; }
		public Single DimBoardsWidth { get; set; }
		public Single DimStandardSheets { get; set; }
		public Single LengthSpecialFirstAndLastSheets { get; set; }
		public Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT { get; set; }
		public Single ExtraStud1Xpos { get; set; }
		public Single ExtraStud2Xpos { get; set; }
		public bool EW1GableHasAWindow { get; set; }
		public bool EW1GableHasExtraStuds { get; set; }

		public EW1GABLE()
			{
			ID = Guid.NewGuid();
			DimTopBottom = 72;
			DimMainStuds = 44;
			DimSecondaryStuds = 72;
			DimBoardsWidth = 145;
			DimStandardSheets = 1200;
			LengthSpecialFirstAndLastSheets = 839;
			NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT = 2;
			ExtraStud1Xpos = 3310;
			ExtraStud2Xpos = 5683;
			EW1GableHasAWindow = true;
			EW1GableHasExtraStuds = true;
		}
	}

	public class EW2PARTY : IElement //External Wall 1 Gable
	{
		public Guid ID { get; set; }                        //Dont fill
		public Guid ModuleID { get; set; }
		public Guid ElementTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
		public Single ZPosMeters { get; set; }
		public List<Board> Boards { get; set; }
		public List<IElement> IElements { get; set; }
		public Single DimTopBottom { get; set; }
		public Single DimMainStuds { get; set; }
		public Single DimSecondaryStuds { get; set; }
		public Single DimBoardsWidth { get; set; }
		public Single DimStandardSheets { get; set; }
		public Single LengthSpecialFirstAndLastSheets { get; set; }
		public Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT { get; set; }
		public Single ExtraStud1Xpos { get; set; }
		public Single ExtraStud2Xpos { get; set; }
		public bool EW1GableHasAWindow { get; set; }
		public bool EW1GableHasExtraStuds { get; set; }

		public EW2PARTY()
		{
			DimTopBottom = 72;
			DimMainStuds = 44;
			DimSecondaryStuds = 72;
			DimBoardsWidth = 145;
			DimStandardSheets = 1200;
			LengthSpecialFirstAndLastSheets = 839;
			NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT = 2;
			ExtraStud1Xpos = 3310;
			ExtraStud2Xpos = 5683;
			EW1GableHasAWindow = true;
			EW1GableHasExtraStuds = true;
		}
	}

	public class EW3FRONT : IElement //External Wall 1 Gable
	{
		public Guid ID { get; set; }                        //Dont fill
		public Guid ModuleID { get; set; }
		public Guid ElementTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
		public Single ZPosMeters { get; set; }
		public List<Board> Boards { get; set; }
		public List<IElement> IElements { get; set; }
		public Single DimTopBottom { get; set; }
		public Single DimMainStuds { get; set; }
		public Single DimSecondaryStuds { get; set; }
		public Single DimBoardsWidth { get; set; }
		public Single DimStandardSheets { get; set; }
		public Single LengthSpecialFirstAndLastSheets { get; set; }
		public Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT { get; set; }
		public Single ExtraStud1Xpos { get; set; }
		public Single ExtraStud2Xpos { get; set; }
		public bool EW1GableHasAWindow { get; set; }
		public bool EW1GableHasExtraStuds { get; set; }

		public EW3FRONT()
		{
			DimTopBottom = 72;
			DimMainStuds = 44;
			DimSecondaryStuds = 72;
			DimBoardsWidth = 145;
			DimStandardSheets = 1200;
			LengthSpecialFirstAndLastSheets = 839;
			NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT = 2;
			ExtraStud1Xpos = 3310;
			ExtraStud2Xpos = 5683;
			EW1GableHasAWindow = true;
			EW1GableHasExtraStuds = true;
		}
	}

	public class EW4BACK : IElement //External Wall 1 Gable
	{
		public Guid ID { get; set; }                        //Dont fill
		public Guid ModuleID { get; set; }
		public Guid ElementTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
		public Single ZPosMeters { get; set; }
		public List<Board> Boards { get; set; }
		public List<IElement> IElements { get; set; }
		public Single DimTopBottom { get; set; }
		public Single DimMainStuds { get; set; }
		public Single DimSecondaryStuds { get; set; }
		public Single DimBoardsWidth { get; set; }
		public Single DimStandardSheets { get; set; }
		public Single LengthSpecialFirstAndLastSheets { get; set; }
		public Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT { get; set; }
		public Single ExtraStud1Xpos { get; set; }
		public Single ExtraStud2Xpos { get; set; }
		public bool EW1GableHasAWindow { get; set; }
		public bool EW1GableHasExtraStuds { get; set; }

		public EW4BACK()
		{
			DimTopBottom = 72;
			DimMainStuds = 44;
			DimSecondaryStuds = 72;
			DimBoardsWidth = 145;
			DimStandardSheets = 1200;
			LengthSpecialFirstAndLastSheets = 839;
			NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT = 2;
			ExtraStud1Xpos = 3310;
			ExtraStud2Xpos = 5683;
			EW1GableHasAWindow = true;
			EW1GableHasExtraStuds = true;
		}
	}

	public class ModularProject
	{
		public Guid ID { get; set; }
		public string ProjectName { get; set; }
		public string CustomerName { get; set; }
		public int TotalNrOfBuildings { get; set; }
		public Single TotalNrOfModules { get; set; }
		public Single CompletedModuleCount { get; set; }
		public List<Module> Modules { get; set; }
	}

	public class Module
    {
		public Guid ID { get; set; }
		public Guid ProjectID { get; set; }
		public Guid BuildingID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime DateProductionStarted { get; set; }
		public Single ThroughputTimeHours { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public List<IElement> Elements { get; set; }
    }



    public interface IElement
    {
		public Guid ID { get; set; }
		public Guid ModuleID { get; set; }
		public Guid ElementTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public Single BoundsLength { get; set; }
		public Single BoundsWidth { get; set; }
		public Single BoundsHight { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
		public Single ZPosMeters { get; set; }
        public List<Board> Boards { get; set; }
		public List<IElement> IElements { get; set; }
		public Single DimTopBottom { get; set; }
		public Single DimMainStuds { get; set; }
		public Single DimSecondaryStuds { get; set; }
		public Single DimBoardsWidth { get; set; }
		public Single DimStandardSheets { get; set; }
		public Single LengthSpecialFirstAndLastSheets { get; set; }
		public Single NrOfOrdinaryStudPlacesTakenUpBySUBELEMENT { get; set; }
		public Single ExtraStud1Xpos { get; set; }
		public Single ExtraStud2Xpos { get; set; }
		public bool EW1GableHasAWindow { get; set; }
		public bool EW1GableHasExtraStuds { get; set; }

	}

	public class Board
    {
		public Guid ID { get; set; }
		public Guid BoardTypeID { get; set; }
		public Guid ParentID { get; set; }
		public string Description { get; set; }
		public Single Length { get; set; }
		public int RoatationX { get; set; }
		public int RoatationY { get; set; }
		public int RoatationZ { get; set; }
		public Single PositionX { get; set; }
		public Single PositionY { get; set; }
		public Single PositionZ { get; set; }
	}

	public class BoardType
    {
		public Guid ID { get; set; }
		public string Label { get; set; }
		public Single Width { get; set; }
		public Single Thickness { get; set; }
		public string Grade { get; set; }
		public string TypeOfWood { get; set; }
		public bool FireTreated { get; set; }
	public BoardType()
        {
			ID = Guid.NewGuid();
			Label = "2x6",
			Thickness = 44;
			Width = 90;
			Grade = "A";
			TypeOfWood = "Pine";
			FireTreated = true;
		}

	}

}

