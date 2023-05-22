using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoController : Controller
    {


        [HttpGet("{idEmpresa}")]
        public List<Autos> TraerAutosByEmpresa(int idEmpresa)
        {
            return ADO_Autos.TraerAutosByEmpresa(idEmpresa);
        }


        //Creo un producto dada toda la info del mismo (el ID se crea automatico en la DB)
        [HttpPost("add")]
        public bool CrearAuto([FromBody] Autos auto)

        {
            return  ADO_Autos.CrearAuto(auto);
        }
        
        //Modifico un producto dada la info del objeto Producto
        [HttpPut("modify")]
        public bool ModificarAuto([FromBody] Autos auto)

        {
            return ADO_Autos.ModificarAuto(auto);
        }

        [HttpDelete("{idAuto}")]
        public bool EliminarAuto(int idAuto)
        {
            return ADO_Autos.EliminarAuto(idAuto);
        }

    }
}
