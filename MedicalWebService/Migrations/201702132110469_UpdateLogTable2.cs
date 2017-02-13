namespace MedicalWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Log", "Description");
            AddColumn("Log","Date",
                 c => c.DateTime(nullable: false)
                 );
        }
        
        public override void Down()
        {
            DropColumn("Log", "Date");
            AddColumn("Log", "Description",
                 c => c.DateTime(nullable: false)
                 );
        }
    }
}
