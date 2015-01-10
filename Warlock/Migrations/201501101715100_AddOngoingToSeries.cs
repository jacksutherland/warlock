namespace Warlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOngoingToSeries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "Ongoing", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Series", "Ongoing");
        }
    }
}
