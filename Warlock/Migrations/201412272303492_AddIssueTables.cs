namespace Warlock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddIssueTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Publishers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Issues",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SeriesId = c.Int(nullable: false),
                    Number = c.Double(nullable: false),
                    Description = c.String(),
                    SaleDate = c.DateTime(nullable: true),
                    Price = c.Decimal(nullable: true, precision: 18, scale: 2),
                    ImageUrl = c.String(),
                    Writer = c.String(),
                    Artist = c.String(),
                    Colorist = c.String(),
                    Letterist = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Series", t => t.SeriesId, cascadeDelete: true)
                .Index(t => t.SeriesId);

            CreateTable(
                "dbo.Variants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    IssueId = c.Int(nullable: false),
                    Name = c.String(),
                    Description = c.String(),
                    CoverArtist = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId, cascadeDelete: true)
                .Index(t => t.IssueId);

            AddColumn("dbo.Series", "PublisherId", c => c.Int(nullable: false));
            AddColumn("dbo.Series", "Description", c => c.String());
            AddForeignKey("dbo.Series", "PublisherId", "dbo.Publishers", "Id", cascadeDelete: true);
            CreateIndex("dbo.Series", "PublisherId");
        }

        public override void Down()
        {
            DropIndex("dbo.Variants", new[] { "IssueId" });
            DropIndex("dbo.Issues", new[] { "SeriesId" });
            DropIndex("dbo.Series", new[] { "PublisherId" });
            DropForeignKey("dbo.Variants", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Issues", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Series", "PublisherId", "dbo.Publishers");
            DropColumn("dbo.Series", "Description");
            DropColumn("dbo.Series", "PublisherId");
            DropTable("dbo.Variants");
            DropTable("dbo.Issues");
            DropTable("dbo.Publishers");
        }
    }
}
