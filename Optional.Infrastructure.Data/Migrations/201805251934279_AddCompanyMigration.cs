namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Title", c => c.String());
            AlterColumn("dbo.Courses", "Theme", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "Theme", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "Title");
        }
    }
}
