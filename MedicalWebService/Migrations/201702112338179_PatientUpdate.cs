namespace MedicalWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "DoctorId", c => c.Int());
            AddColumn("dbo.Patients", "NurseId", c => c.Int());
            AddColumn("dbo.Patients", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Patients", "Nurse_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Patients", "Nurse_Id");
            AddForeignKey("dbo.Patients", "Nurse_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Patients", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "ApplicationUserId", c => c.Int());
            DropForeignKey("dbo.Patients", "Nurse_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Patients", new[] { "Nurse_Id" });
            DropColumn("dbo.Patients", "Nurse_Id");
            DropColumn("dbo.Patients", "RowVersion");
            DropColumn("dbo.Patients", "NurseId");
            DropColumn("dbo.Patients", "DoctorId");
        }
    }
}
