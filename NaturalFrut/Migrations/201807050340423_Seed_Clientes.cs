namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_Clientes : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Marcelo', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 1)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Rodrigo', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Rodolfo', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 3)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Marcos', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Julian', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 3)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Pedro', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Josefina', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 3)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Jose', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Pepe', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Maria Juana', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");
            Sql("INSERT INTO [dbo].[Clientes] VALUES ('Roberta', 'Maktuco', '796969', '98696986', 'asdas 0809', 'jpjpoj', 'okasokaok', 98098709, 908098, 'marce@gas.com', 2, 1, 2)");

        }

        public override void Down()
        {
        }
    }
}
