namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_CondicionIVA_TipoCliente_UandR_TU : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO CondicionIVAs values ('Responsable Inscripto')");
            Sql("INSERT INTO CondicionIVAs values ('Monotributo')");
            Sql("INSERT INTO CondicionIVAs values ('Consumidor Final')");

            Sql("INSERT INTO TipoClientes VALUES ('Dietética')");
            Sql("INSERT INTO TipoClientes VALUES ('Verdulería')");
            Sql("INSERT INTO TipoClientes VALUES ('Supermercado')");
            Sql("INSERT INTO TipoClientes VALUES ('Distribuidor/Mayorista')");

            Sql(@"

                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6bf57ee9-ab99-4b7c-a99b-a666a878e0da', N'guest@naturalfrut.com', 0, N'APS00zOZH8zGze6yqqljg9ufytEXKZocPysEHtW2u8tvwZtwD0U5lB/ZkcHRDdGOVA==', N'10c8b60e-d4ed-4442-8afa-a323080e1da4', NULL, 0, 0, NULL, 1, 0, N'guest@naturalfrut.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'7c9bb30e-506c-48e9-8cb1-dd2797caa496', N'user@naturalfrut.com', 0, N'AJME5w0q7u7jYOU3qD1UhGkDuBZvidTWrl9HhloMQYmi/c0lsuAixD5hnxj2J+Ghng==', N'e742b3bf-4acf-4035-af0b-102ecff4f31f', NULL, 0, 0, NULL, 1, 0, N'user@naturalfrut.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'a1c5abb4-36e6-4211-bceb-eaaec605cdd5', N'administrator@naturalfrut.com', 0, N'AFdPLZlfffWxHbi9qDkWE34+EEl4JGdvsSujG9FIuIre/n5DIXg/0cUa5P39B1lz3g==', N'85fd0e78-7271-423d-b46b-f67eeb2c3578', NULL, 0, 0, NULL, 1, 0, N'administrator@naturalfrut.com')

                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c4ee3b34-bc51-4b83-a8ec-145fa3dfa158', N'Administrator')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'065d51ff-5bed-4d90-a335-53c929b6f434', N'User')

                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7c9bb30e-506c-48e9-8cb1-dd2797caa496', N'065d51ff-5bed-4d90-a335-53c929b6f434')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a1c5abb4-36e6-4211-bceb-eaaec605cdd5', N'c4ee3b34-bc51-4b83-a8ec-145fa3dfa158')


                ");

            Sql("INSERT INTO TiposDeUnidad Values('Kg')");
            Sql("INSERT INTO TiposDeUnidad Values('Bulto Cerrado')");
            Sql("INSERT INTO TiposDeUnidad Values('Unidad')");


        }
        
        public override void Down()
        {
        }
    }
}
