namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.ListaPrecioBlisters",
               c => new
               {
                   ID = c.Int(nullable: false, identity: true),
                   Gramos = c.Int(nullable: false),
                   Precio = c.String(),
                   ListaID = c.Int(),
               })
               .PrimaryKey(t => t.ID);

            CreateIndex("dbo.ListaPrecioBlisters", "ListaID");
            AddForeignKey("dbo.ListaPrecioBlisters", "ListaID", "dbo.Listas", "ID");


            DropForeignKey("dbo.ListaPrecioBlisters", "ListaID", "dbo.Listas");
            DropIndex("dbo.ListaPrecioBlisters", new[] { "ListaID" });
            DropTable("dbo.ListaPrecioBlisters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ListaPrecioBlisters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Gramos = c.Int(nullable: false),
                        Precio = c.String(),
                        ListaID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.ListaPrecioBlisters", "ListaID");
            AddForeignKey("dbo.ListaPrecioBlisters", "ListaID", "dbo.Listas", "ID");
        }
    }
}
