namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "LecturerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "LecturerId", c => c.Int());
        }
    }
}
