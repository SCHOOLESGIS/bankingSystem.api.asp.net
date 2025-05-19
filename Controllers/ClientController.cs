using System;
using System.Collections.Generic;
using WebApplication2.Service;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly BanqueService _service = new BanqueService();

        [HttpGet]
        public IActionResult GetClients() => Ok(_service.ListeClient());

        [HttpPost]
        public IActionResult AddClient([FromBody] Client client)
        {
            _service.AjouterClient(client);
            return Ok();
        }
    }
}
