namespace TataApp.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemarks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentTypeId)
                .Index(t => t.Description, unique: true, name: "DocumentType_Description_Index");
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EmployeeCode = c.Int(nullable: false),
                        DocumentTypeId = c.Int(nullable: false),
                        LoginTypeId = c.Int(nullable: false),
                        Document = c.String(nullable: false, maxLength: 20),
                        Picture = c.String(),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeId)
                .ForeignKey("dbo.LoginTypes", t => t.LoginTypeId)
                .Index(t => t.EmployeeCode, unique: true, name: "Employee_EmployeeCode_Index")
                .Index(t => new { t.DocumentTypeId, t.Document }, unique: true, name: "Employee_DocumentTypeId_Document_Index")
                .Index(t => t.LoginTypeId)
                .Index(t => t.Email, unique: true, name: "User_Email_Index");
            
            CreateTable(
                "dbo.LoginTypes",
                c => new
                    {
                        LoginTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.LoginTypeId)
                .Index(t => t.Description, unique: true, name: "LoginType_Description_Index");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "LoginTypeId", "dbo.LoginTypes");
            DropForeignKey("dbo.Employees", "DocumentTypeId", "dbo.DocumentTypes");
            DropIndex("dbo.LoginTypes", "LoginType_Description_Index");
            DropIndex("dbo.Employees", "User_Email_Index");
            DropIndex("dbo.Employees", new[] { "LoginTypeId" });
            DropIndex("dbo.Employees", "Employee_DocumentTypeId_Document_Index");
            DropIndex("dbo.Employees", "Employee_EmployeeCode_Index");
            DropIndex("dbo.DocumentTypes", "DocumentType_Description_Index");
            DropTable("dbo.LoginTypes");
            DropTable("dbo.Employees");
            DropTable("dbo.DocumentTypes");
        }
    }
}
