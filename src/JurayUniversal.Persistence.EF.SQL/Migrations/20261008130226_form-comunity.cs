using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    /// <inheritdoc />
    public partial class formcomunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MisCommunityFormSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PreferredName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateCountryOfResidence = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FatherFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MotherFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FamilyKindredName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClanName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NumberOfChildren = table.Column<int>(type: "int", nullable: true),
                    ChildrenNames = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelationshipToCommunity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FatherVillageTown = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MotherVillageTown = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FamilyCompound = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KindredLineage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FamilyHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AncestralHome = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FamilyHistory = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RelatedFamilies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaternalGrandfather = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaternalGrandmother = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaternalGrandfather = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaternalGrandmother = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GreatGrandparents = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HighestQualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Institution = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProfessionalCertifications = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentCourseOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentOccupation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BusinessType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BusinessLocation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    ProfessionalSkills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExpertiseAreas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OtherExpertise = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    WillingToOfferSkills = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CurrentCommunityPosition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreviousLeadershipPositions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CommunityAssociations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CommunityContributions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ContributionInterests = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VolunteerInterests = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AlternativePhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmergencyContactRelationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PassportPhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SocialProfileLink = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Hobbies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LanguagesSpoken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConsentGiven = table.Column<bool>(type: "bit", nullable: false),
                    ConsentDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReviewedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedFromIp = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MisCommunityFormSubmissions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MisCommunityFormSubmissions");
        }
    }
}
