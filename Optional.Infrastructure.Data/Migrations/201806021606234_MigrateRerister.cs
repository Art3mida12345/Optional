namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateRerister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registers", "Course_CourseId", c => c.Int());
            CreateIndex("dbo.Registers", "Course_CourseId");
            AddForeignKey("dbo.Registers", "Course_CourseId", "dbo.Courses", "CourseId");
            DropColumn("dbo.Registers", "StudentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registers", "StudentId", c => c.Int());
            DropForeignKey("dbo.Registers", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.Registers", new[] { "Course_CourseId" });
            DropColumn("dbo.Registers", "Course_CourseId");
        }
    }
}
