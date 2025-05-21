namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Adresse = c.String(),
                        CodePostal = c.String(),
                        Ville = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comptes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumAcc = c.String(),
                        Libelle = c.String(),
                        MontantDecouvert = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateOuverture = c.DateTime(nullable: false),
                        AutorisationDecouvert = c.Boolean(nullable: false),
                        Solde = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientId = c.Int(nullable: false),
                        TauxDeRenumeration = c.Double(),
                        TauxDeRenumeration1 = c.Double(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comptes", "ClientId", "dbo.Clients");
            DropIndex("dbo.Comptes", new[] { "ClientId" });
            DropTable("dbo.Comptes");
            DropTable("dbo.Clients");
        }
    }
}
