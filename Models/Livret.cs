using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Livret : Compte
    {
        public double TauxDeRenumeration { get; set; }
        public void AjouterInterets ()
        {
            var interet = Solde * (decimal)(TauxDeRenumeration / 100);
            Credit(interet);
        }

        public override bool Debit(decimal montant) => false; //Interdit le decouvert
    }
}