namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123123123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompetitionEventDetails",
                c => new
                    {
                        CompetitionID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                        Round = c.Int(nullable: false),
                        CutOffDetails = c.String(),
                        TimeLimit = c.String(),
                        HowManyProced = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                    })
                .PrimaryKey(t => new { t.CompetitionID, t.EventID })
                .ForeignKey("dbo.Competitions", t => t.CompetitionID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.CompetitionID)
                .Index(t => t.EventID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompetitionEventDetails", "EventID", "dbo.Events");
            DropForeignKey("dbo.CompetitionEventDetails", "CompetitionID", "dbo.Competitions");
            DropIndex("dbo.CompetitionEventDetails", new[] { "EventID" });
            DropIndex("dbo.CompetitionEventDetails", new[] { "CompetitionID" });
            DropTable("dbo.CompetitionEventDetails");
        }
    }
}
