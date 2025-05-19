using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Service;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Api/[controller]")]
    public class CompteController : ControllerBase
    {
        private readonly BanqueService _service = new BanqueService();
        [HttpGet]
        public IActionResult GetComptes() => Ok(_service.AfficherComptes());

        [HttpPost]
        public IActionResult AddCompte([FromBody] Compte compte)
        {
            _service.AjouterCompte(compte);
            return Ok();
        }

        [HttpPost("virement")]
        public IActionResult Virement([FromQuery] int source, [FromQuery] int destination, [FromQuery] decimal montant)
        {
            if (_service.Virement(source, destination, montant))
            {
                return Ok("virement réussi");
            }
            return BadRequest("Echec de virement");
        }

        [HttpDelete("{id}")]
        public IActionResult SupprimerCompte(int id)
        {
            var compte = _service.RechercherCompte(id);
            if (compte == null)
            {
                return NotFound($"Aucun compte trouvé avec l'id {id}");
            }
            _service.SupprimerCompte(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Compte> RechercherCompteParId(int id)
        {
            var compte = _service.RechercherCompte(id);
            if (compte == null)
            {
                return NotFound($"Aucun compte trouvé avec l'id {id}");
            }
            return Ok(compte);
        }

        [HttpGet("client/{clientId}")]
        public ActionResult<IEnumerable<Compte>> RechercherComptesClient(int clientId)
        {
            var comptes = _service.ListeComptesClient(clientId);
            if (comptes == null || !comptes.Any())
            {
                return NotFound($"Aucun compte(s) trouvé(s) avec l'id {clientId}");
            }
            return Ok(comptes);
        }

     
    }
}


// ENTITY FRAAMEWORK - MODELS
// CONNECTER L'API
// INSERTION DANS LA BD
// UTILISATION DE POSTMAN