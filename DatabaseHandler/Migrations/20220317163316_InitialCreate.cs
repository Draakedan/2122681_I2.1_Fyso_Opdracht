using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseHandler.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionPlans",
                columns: table => new
                {
                    ActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionsPerWeek = table.Column<int>(type: "int", nullable: false),
                    TimePerSession = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPlans", x => x.ActionID);
                });

            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    AdressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.AdressID);
                });

            migrationBuilder.CreateTable(
                name: "FysioWorkers",
                columns: table => new
                {
                    FysioWorkerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIGNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsStudent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FysioWorkers", x => x.FysioWorkerID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdressID = table.Column<int>(type: "int", nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    IsStudent = table.Column<bool>(type: "bit", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_Patients_Adresses_AdressID",
                        column: x => x.AdressID,
                        principalTable: "Adresses",
                        principalColumn: "AdressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientFiles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnoseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnoseCodeComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsStudent = table.Column<bool>(type: "bit", nullable: false),
                    IntakeDoneByID = table.Column<int>(type: "int", nullable: false),
                    IdintakeSuppervisedBy = table.Column<int>(type: "int", nullable: false),
                    IntakeSuppervisedByFysioWorkerID = table.Column<int>(type: "int", nullable: true),
                    IdmainTherapist = table.Column<int>(type: "int", nullable: false),
                    MainTherapistFysioWorkerID = table.Column<int>(type: "int", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentIDs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdactionPlan = table.Column<int>(type: "int", nullable: false),
                    ActionPlanActionID = table.Column<int>(type: "int", nullable: true),
                    SessionIDs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFiles_ActionPlans_ActionPlanActionID",
                        column: x => x.ActionPlanActionID,
                        principalTable: "ActionPlans",
                        principalColumn: "ActionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFiles_FysioWorkers_IntakeDoneByID",
                        column: x => x.IntakeDoneByID,
                        principalTable: "FysioWorkers",
                        principalColumn: "FysioWorkerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFiles_FysioWorkers_IntakeSuppervisedByFysioWorkerID",
                        column: x => x.IntakeSuppervisedByFysioWorkerID,
                        principalTable: "FysioWorkers",
                        principalColumn: "FysioWorkerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFiles_FysioWorkers_MainTherapistFysioWorkerID",
                        column: x => x.MainTherapistFysioWorkerID,
                        principalTable: "FysioWorkers",
                        principalColumn: "FysioWorkerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFiles_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateMade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommenterID = table.Column<int>(type: "int", nullable: false),
                    CommentMadeByFysioWorkerID = table.Column<int>(type: "int", nullable: true),
                    VisibleToPatient = table.Column<bool>(type: "bit", nullable: false),
                    PatientFileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_FysioWorkers_CommentMadeByFysioWorkerID",
                        column: x => x.CommentMadeByFysioWorkerID,
                        principalTable: "FysioWorkers",
                        principalColumn: "FysioWorkerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_PatientFiles_PatientFileID",
                        column: x => x.PatientFileID,
                        principalTable: "PatientFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TherapySessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPractiseRoom = table.Column<bool>(type: "bit", nullable: false),
                    Specials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionDoneByID = table.Column<int>(type: "int", nullable: false),
                    SesionDoneByFysioWorkerID = table.Column<int>(type: "int", nullable: true),
                    SessionStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientFileID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapySessions_FysioWorkers_SesionDoneByFysioWorkerID",
                        column: x => x.SesionDoneByFysioWorkerID,
                        principalTable: "FysioWorkers",
                        principalColumn: "FysioWorkerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TherapySessions_PatientFiles_PatientFileID",
                        column: x => x.PatientFileID,
                        principalTable: "PatientFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentMadeByFysioWorkerID",
                table: "Comments",
                column: "CommentMadeByFysioWorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PatientFileID",
                table: "Comments",
                column: "PatientFileID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_ActionPlanActionID",
                table: "PatientFiles",
                column: "ActionPlanActionID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_IntakeDoneByID",
                table: "PatientFiles",
                column: "IntakeDoneByID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_IntakeSuppervisedByFysioWorkerID",
                table: "PatientFiles",
                column: "IntakeSuppervisedByFysioWorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_MainTherapistFysioWorkerID",
                table: "PatientFiles",
                column: "MainTherapistFysioWorkerID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_PatientID",
                table: "PatientFiles",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AdressID",
                table: "Patients",
                column: "AdressID");

            migrationBuilder.CreateIndex(
                name: "IX_TherapySessions_PatientFileID",
                table: "TherapySessions",
                column: "PatientFileID");

            migrationBuilder.CreateIndex(
                name: "IX_TherapySessions_SesionDoneByFysioWorkerID",
                table: "TherapySessions",
                column: "SesionDoneByFysioWorkerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "TherapySessions");

            migrationBuilder.DropTable(
                name: "PatientFiles");

            migrationBuilder.DropTable(
                name: "ActionPlans");

            migrationBuilder.DropTable(
                name: "FysioWorkers");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Adresses");
        }
    }
}
