using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Banktransactions.Repositorio;

namespace Banktransactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("/list")]
        public ActionResult<IEnumerable<string>> GetRegistedUsers()
        {

            Repositorio.Repositorio bankDAO = Repositorio.Repositorio.getInstance();
            return bankDAO.ExecuteQuery("SELECT * FROM :tables ").ToArray<string>();
        }
    }        
}