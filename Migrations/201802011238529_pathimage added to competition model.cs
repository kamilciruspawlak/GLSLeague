namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pathimageaddedtocompetitionmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "PathImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "PathImage");
        }
    }
}
