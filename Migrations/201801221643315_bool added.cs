namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booladded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsThisShitTrue", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsThisShitTrue");
        }
    }
}
