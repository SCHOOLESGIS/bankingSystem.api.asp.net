using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class BanqueService
    {
        private List<Client> Clients = new List<Client>();
        private List<Compte> Comptes = new List<Compte>();

        public bool AjouterClient (Client client)
        {
            bool exists = Clients.Where(c => c.Nom.Equals(client.Nom)).Any();
            if (! exists)
            {
                Clients.Add(client);
                return true;
            }
            return false;
        }

        public bool ModifierClient (Client updateClient)
        {
            Clients.ForEach(c => {
                if (c.Id == updateClient.Id)
                {
                    c.Nom = updateClient.Nom;
                    c.Prenom = updateClient.Prenom;
                    c.Adresse = updateClient.Adresse;
                    c.CodePostal = updateClient.CodePostal;
                    c.Telephone = updateClient.Telephone;
                    c.Ville = updateClient.Ville;
                }
            });

            return false;
        }

        public bool SupprimerClient (int Id)
        {
            Client deleteClient = this.RechercherClient(Id);

            if (deleteClient != null)
            {
                Clients.Remove(deleteClient);
                return true;
            }
            return false;
        }

        public Client RechercherClient (int Id)
        {
            return Clients.FirstOrDefault(c => c.Id == Id);
        }

        public Client RechercherClientParNom(string Nom)
        {
            return Clients.FirstOrDefault(c => c.Nom.StartsWith(Nom));
        }

        public List<Client> RechercherLesClientsParNom(string Nom)
        {
            return Clients.Where(c => c.Nom.StartsWith(Nom)).ToList();
        }

        public List<Client> AfficherClients ()
        {
            return Clients;
        }

        //COMPTES

        public bool AjouterCompte(Compte compte)
        {
            bool exists = Comptes.Where(c => c.NumAcc.Equals(compte.NumAcc)).Any();
            if (!exists)
            {
                Comptes.Add(compte);
                return true;
            }
            return false;
        }

        public bool ModifierCompte(Compte updateCompte)
        {
            Comptes.ForEach(c => {
                if (c.Id == updateCompte.Id)
                {
                    c.NumAcc = updateCompte.NumAcc;
                    c.Libelle = updateCompte.Libelle;
                    c.MontantDecouvert = updateCompte.MontantDecouvert;
                    c.DateOuverture = updateCompte.DateOuverture;
                    c.AutorisationDecouvert = updateCompte.AutorisationDecouvert;
                    c.Solde = updateCompte.Solde;
                }
            });

            return false;
        }

        public bool SupprimerCompte(int Id)
        {
            Compte deleteCompte = RechercherCompte(Id);

            if (deleteCompte != null)
            {
                Comptes.Remove(deleteCompte);
                return true;
            }
            return false;
        }

        public Compte RechercherCompte(int Id)
        {
            return Comptes.FirstOrDefault(c => c.Id == Id);
        }

        public List<Compte> AfficherComptesClient(int Id)
        {
            return Comptes.Where(c => c.Client.Id == Id).ToList();
        }

        public List<Compte> AfficherComptes()
        {
            return Comptes;
        }

        public bool Virement (int sourceId, int destinationId, decimal montant)
        {
            Compte source = RechercherCompte(sourceId);
            Compte destination = RechercherCompte(destinationId);

            if (source != null && destination != null && source.Debit(montant))
            {
                destination.Credit(montant);

                return true;
            }

            return false;
        }
    }
}