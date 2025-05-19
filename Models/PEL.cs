using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PEL : Compte
    {
        public double TauxDeRenumeration { get; set; }
        public override bool Debit(decimal montant)
        {
            if ((DateTime.Now - DateOuverture).TotalDays >= 365*5)
            {
                return base.Debit(montant);
            }
            return false; //Retrait interdit avant 5 ans
        }

        public void Interet ()
        {
            var interet = Solde * (decimal)(TauxDeRenumeration / 100);
            Credit(interet);
        }
    }
}