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
            if (!exists)
            {
                using (var clientContext = new AppDbContext())
                {
                    if (!clientContext.Clients.Where(c => c.Nom.Equals(client.Nom)).Any())
                    {
                        Clients.Add(client);
                        clientContext.Clients.Add(client);
                        clientContext.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ModifierClient (Client updateClient)
        {
            Clients.ForEach(c => {
                if (c.Id == updateClient.Id)
                {
                    using (var clientContext = new AppDbContext())
                    {
                        Client client = clientContext.Clients.Where(cc => cc.Nom.Equals(updateClient.Nom)).FirstOrDefault();
                        if (client != null)
                        {
                            //list
                            c.Nom = updateClient.Nom;
                            c.Prenom = updateClient.Prenom;
                            c.Adresse = updateClient.Adresse;
                            c.CodePostal = updateClient.CodePostal;
                            c.Telephone = updateClient.Telephone;
                            c.Ville = updateClient.Ville;

                            //context
                            client.Nom = updateClient.Nom;
                            client.Prenom = updateClient.Prenom;
                            client.Adresse = updateClient.Adresse;
                            client.CodePostal = updateClient.CodePostal;
                            client.Telephone = updateClient.Telephone;
                            client.Ville = updateClient.Ville;
                            clientContext.Clients.Add(client);
                            clientContext.SaveChanges();

                        }
                    }
                }
            });
            return false;
        }

        public bool SupprimerClient (int Id)
        {
            Client deleteClient = this.RechercherClient(Id);

            if (deleteClient != null)
            {
                using (var clientContext = new AppDbContext())
                {
                    using (var clientTrans = clientContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var client = clientContext.Clients.Where(c => c.Nom.Equals(deleteClient.Nom));
                            if (client.Any())
                            {
                                var clientComptes = AfficherComptesClient(Id);
                                clientComptes.ForEach(cc => SupprimerCompte(cc.Id));
                                Clients.Remove(deleteClient);
                                clientContext.Clients.Remove(client.FirstOrDefault());
                                clientContext.SaveChanges();
                                return true;
                            }
                            clientTrans.Commit();
                        } catch
                        {
                            clientTrans.Rollback();
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public Client RechercherClient (int Id)
        {
            var client = Clients.Where(c => c.Id == Id);
            if (!client.Any())
            {
                using (var clientContext = new AppDbContext())
                {
                    var cc = clientContext.Clients.Where(c => c.Id == Id);
                    if (!cc.Any())
                    {
                        return cc.FirstOrDefault();
                    } else
                    {
                        return null;
                    }                    
                }
            } else
            {
                return client.FirstOrDefault();
            }
        }

        public Client RechercherClientParNom(string Nom)
        {
            var client = Clients.Where(c => c.Nom.StartsWith(Nom));
            if (!client.Any())
            {
                using (var clientContext = new AppDbContext())
                {
                    var cc = clientContext.Clients.Where(c => c.Nom.StartsWith(Nom));
                    if (!cc.Any())
                    {
                        return cc.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return client.FirstOrDefault();
            }
        }

        public List<Client> RechercherLesClientsParNom(string Nom)
        {
            var clients = Clients.Where(c => c.Nom.StartsWith(Nom)).ToList();
            if (!clients.Any())
            {
                using (var clientContext = new AppDbContext())
                {
                    var cc = clientContext.Clients.Where(c => c.Nom.StartsWith(Nom)).ToList();
                    if (!cc.Any())
                    {
                        return cc;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return clients;
            }
        }

        public List<Client> AfficherClients ()
        {
            if (!Clients.Any())
            {
                using (var clientContext = new AppDbContext())
                {
                    return clientContext.Clients.ToList();
                }
            } else
            {
                return Clients;
            }
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
                using (var compteContext = new AppDbContext())
                {
                    using (var compteTrans = compteContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var compte = compteContext.Comptes.Where(c => c.Id == Id );
                            if (compte.Any())
                            {
                                Comptes.Remove(deleteCompte);
                                compteContext.Comptes.Remove(compte.FirstOrDefault());
                                compteContext.SaveChanges();
                                return true;
                            }
                            compteTrans.Commit();
                        }
                        catch
                        {
                            compteTrans.Rollback();
                        }
                    }
                }
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
            var client = RechercherClient(Id);
            if (client != null)
            {
                var comptesClient = Comptes.Where(cc => cc.ClientId == Id).ToList();
                if (!comptesClient.Any())
                {
                    using (var comptesContext = new AppDbContext())
                    {
                        return comptesContext.Comptes.Where(cc => cc.ClientId == Id).ToList();
                    }
                } else
                {
                    return comptesClient;
                }
            }
            return Comptes.Where(c => c.ClientId == Id).ToList();
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