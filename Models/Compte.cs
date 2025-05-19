using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    //public enum TypeCompte { COURANT, LIVRE_A, LEP, PEL}
    public abstract class Compte
    {
        public int Id { get; set; }
        public string NumAcc { get; set; }
        public string Libelle { get; set; }
        public decimal MontantDecouvert { get; set; }
        public DateTime DateOuverture { get; set; }
        public bool AutorisationDecouvert { get; set; }
        public decimal Solde { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public virtual void Credit (decimal montant) => Solde += montant;
        public virtual bool Debit (decimal montant)
        {
            if ((Solde - montant) >= (AutorisationDecouvert?-MontantDecouvert:0))
            {
                return true;
            }
            return false;
        }
    }
}