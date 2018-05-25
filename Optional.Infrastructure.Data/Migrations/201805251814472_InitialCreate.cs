namespace Optional.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Theme = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        LecturerId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Lecturers", t => t.LecturerId)
                .Index(t => t.LecturerId);
            
            CreateTable(
                "dbo.Lecturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Department = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Department = c.String(),
                        Group = c.String(),
                        YearOfStudy = c.Int(nullable: false),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Registers",
                c => new
                    {
                        RegisterId = c.Int(nullable: false, identity: true),
                        Mark = c.Int(nullable: false),
                        StudentId = c.Int(),
                    })
                .PrimaryKey(t => t.RegisterId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_Id = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Course_CourseId })
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registers", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Courses", "LecturerId", "dbo.Lecturers");
            DropIndex("dbo.StudentCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
            DropIndex("dbo.Registers", new[] { "StudentId" });
            DropIndex("dbo.Courses", new[] { "LecturerId" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.Registers");
            DropTable("dbo.Students");
            DropTable("dbo.Lecturers");
            DropTable("dbo.Courses");
        }
    }
}
