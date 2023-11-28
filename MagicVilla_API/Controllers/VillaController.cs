//Aquí se están importando los espacios de nombres necesarios para trabajar con ASP.NET Core y Entity Framework.

using MagicVilla_API.Controllers;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Net;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

//. Declaración de la Clase VillaController

//VillaController es la clase principal que hereda de ControllerBase. Esta clase es un controlador de API.
//Route("api/[controller]") especifica la ruta base para las acciones del controlador. [controller] será reemplazado por 
//el nombre del controlador, en este caso, "Villa".

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;

        private readonly ApplicationDbContext _db;

        // Constructor del Controlador

        //Se define un constructor que recibe dos parámetros: ILogger<VillaController> y ApplicationDbContext.
        //ILogger<VillaController> se utiliza para registrar mensajes de registro.
        //ApplicationDbContext es un contexto de base de datos de Entity Framework que permite interactuar con la base de datos.
        public VillaController(ILogger<VillaController> logger , ApplicationDbContext db)
        {

            _logger = logger;
            _db = db;

        }






        //Endpoint para Obtener Todas las Villas(GET)
        //Este es el 1er endpoint que nos devuelve todas las villas

//[HttpGet] especifica que este método maneja solicitudes HTTP GET.
//[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
//ActionResult<IEnumerable<VillaDto>> indica que este método puede devolver una lista de objetos VillaDto.
//Dentro del método, se logea un mensaje, y se retorna la lista de villas desde la base de datos.

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {


            _logger.LogInformation("Obtener las Villas");
           // return Ok(VillaStore.villaList);

            return Ok(_db.Villas.ToList());


        }

        // Endpoint para Obtener una Sola Villa por Id (GET)
        //Este es el 2do endpoint que nos devuelve una sola villa.

//[HttpGet("{id:int}", Name = "GetVilla")] especifica que este método maneja solicitudes GET con una variable de ruta id de tipo entero.
//[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
//ActionResult<VillaDto> indica que este método puede devolver un objeto VillaDto.
//GetVilla busca una villa por su ID en la base de datos y responde según el resultado.


        [HttpGet("id: int", Name ="GetVilla")]
        //codigos de estado para la documentacion.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDto> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Erro al traer Villa con Id " + id);
                return BadRequest();
            }

           // var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }


            return Ok(villa);

        }


       //[HttpPost] especifica que este método maneja solicitudes HTTP POST.
       //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
       //ActionResult<VillaDto> indica que este método puede devolver un objeto VillaDto.
       //FromBody indica que los datos se toman del cuerpo de la solicitud.
       //Este método crea una nueva villa en la base de datos.



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            // Paso 1: Validar el estado del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            //  Paso 2: Verificar si ya existe una Villa con el mismo nombre

            if (//VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null
                _db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)   
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe");
                return BadRequest(ModelState);  
            }

            //  Paso 3: Verificar si la VillaDto proporcionada no es nula

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            //  Paso 4: Verificar si la propiedad Id de la VillaDto es mayor que cero

            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

            // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

            // VillaStore.villaList.Add(villaDto);

            Villa modelo = new()
            {
                // Id = villaDto.Id,  no hay necesidad es automatico
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad


            };

            _db.Villas.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id=villaDto.Id}, villaDto);

        }


// Endpoint para Eliminar una Villa por Id(DELETE)

//[HttpDelete("{id:int}")] especifica que este método maneja solicitudes DELETE con una variable de ruta id de tipo entero.
//[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
//IActionResult indica que este método puede devolver cualquier tipo de resultado.
//Este método elimina una villa de la base de datos por su ID.



        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVilla(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }

           // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id==id);
            if (villa == null)
            {
                return NotFound();
            }

            // VillaStore.villaList.Remove(villa);

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();


        }

        // Endpoint para Actualizar una Villa por Id (PUT)

        //[HttpPut("{id:int}")] especifica que este método maneja solicitudes PUT con una variable de ruta id de tipo entero.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //IActionResult indica que este método puede devolver cualquier tipo de resultado.
        //Este método actualiza una villa en la base de datos por su ID.



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {

            if (villaDto == null || id != villaDto.Id)
            {


                return BadRequest();

            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id); 
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad


            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();


            return NoContent();
            

        }


        //Endpoint para Actualizar Parcialmente una Villa por Id (PATCH)

//[HttpPatch("{id:int}")] especifica que este método maneja solicitudes PATCH con una variable de ruta id de tipo entero.
//[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
//IActionResult indica que este método puede devolver cualquier tipo de resultado.
//Este método actualiza parcialmente una villa en la base de datos por su ID.




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {


                return BadRequest();

            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad

            };

            if (villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villaDto, ModelState);
            
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);

            }


            Villa modelo = new()
            {

                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad


            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();


        }





    }
}
