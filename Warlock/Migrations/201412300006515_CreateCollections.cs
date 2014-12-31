namespace Warlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCollections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Series", "CollectionId", c => c.Int(nullable: true));
            AddColumn("dbo.Series", "Volume", c => c.Int(nullable: true));
            AddColumn("dbo.Issues", "Owned", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "Owned");
            DropColumn("dbo.Series", "Volume");
            DropColumn("dbo.Series", "CollectionId");
            DropTable("dbo.Collections");            
        }
    }
}
