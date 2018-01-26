namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcompetitorcompetitorevents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompetitorEvents",
                c => new
                    {
                        CompetitionID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                        CompetitiorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompetitionID, t.EventID, t.CompetitiorID })
                .ForeignKey("dbo.Competitions", t => t.CompetitionID, cascadeDelete: true)
                .ForeignKey("dbo.Competitors", t => t.CompetitiorID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.CompetitionID)
                .Index(t => t.EventID)
                .Index(t => t.CompetitiorID);
            
            CreateTable(
                "dbo.Competitors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        WCAIdetificator = c.String(),
                        Email = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompetitorEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.CompetitorEvents", "CompetitiorID", "dbo.Competitors");
            DropForeignKey("dbo.CompetitorEvents", "CompetitionID", "dbo.Competitions");
            DropIndex("dbo.CompetitorEvents", new[] { "CompetitiorID" });
            DropIndex("dbo.CompetitorEvents", new[] { "EventID" });
            DropIndex("dbo.CompetitorEvents", new[] { "CompetitionID" });
            DropTable("dbo.Competitors");
            DropTable("dbo.CompetitorEvents");
        }
    }
}
