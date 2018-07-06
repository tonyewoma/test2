namespace test2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drivers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Route = c.String(),
                        PhoneNumber = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Drivers");
        }
    }
}
