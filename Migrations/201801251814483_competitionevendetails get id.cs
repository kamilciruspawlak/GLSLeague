namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class competitionevendetailsgetid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CompetitionEventDetails");
            AddColumn("dbo.CompetitionEventDetails", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CompetitionEventDetails", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CompetitionEventDetails");
            DropColumn("dbo.CompetitionEventDetails", "ID");
            AddPrimaryKey("dbo.CompetitionEventDetails", new[] { "CompetitionID", "EventID" });
        }
    }
}
