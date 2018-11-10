namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Cost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Cost");
        }
    }
}
