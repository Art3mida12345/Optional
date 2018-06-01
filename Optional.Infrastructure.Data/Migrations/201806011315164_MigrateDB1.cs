namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "LecturerId", "dbo.Lecturers");
            DropIndex("dbo.Courses", new[] { "LecturerId" });
            AddColumn("dbo.Courses", "Lecturer_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Courses", "LecturerId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Courses", "Lecturer_Id");
            AddForeignKey("dbo.Courses", "Lecturer_Id", "dbo.Lecturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Lecturer_Id", "dbo.Lecturers");
            DropIndex("dbo.Courses", new[] { "Lecturer_Id" });
            AlterColumn("dbo.Courses", "LecturerId", c => c.String(maxLength: 128));
            DropColumn("dbo.Courses", "Lecturer_Id");
            CreateIndex("dbo.Courses", "LecturerId");
            AddForeignKey("dbo.Courses", "LecturerId", "dbo.Lecturers", "Id");
        }
    }
}
