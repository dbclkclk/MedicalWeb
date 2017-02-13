namespace MedicalWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OauthClient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        ClientSecret = c.String(),
                        ClientRedirectURL = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
