namespace GlsLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedbool : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "IsThisShitTrue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "IsThisShitTrue", c => c.Boolean(nullable: false));
        }
    }
}
