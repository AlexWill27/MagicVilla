//Imports de Espacios de Nombres

//Estos son los espacios de nombres necesarios para trabajar con ASP.NET Core, Entity Framework, y otras dependencias como AutoMapper,
//que es una biblioteca para facilitar el mapeo entre objetos.


using AutoMapper;
using MagicVilla_API.Controllers;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

//. Declaración de la Clase VillaController

//La clase VillaController hereda de ControllerBase y está decorada con atributos como [Route("api/[controller]")] y [ApiController],
//que configuran la ruta base para las acciones del controlador y habilitan la validación automática de modelos en las solicitudes.

//Se definen campos privados para inyectar dependencias, como un logger (ILogger), el contexto de la base de datos (ApplicationDbContext),
//y un mapeador (IMapper) para usar con AutoMapper.

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        // Constructor del Controlador

        //Se define un constructor que recibe dos parámetros: ILogger<VillaController> y ApplicationDbContext.
        //ILogger<VillaController> se utiliza para registrar mensajes de registro.
        //ApplicationDbContext es un contexto de base de datos de Entity Framework que permite interactuar con la base de datos.
        public VillaController(ILogger<VillaController> logger , ApplicationDbContext db, IMapper mapper)
        {

            _logger = logger;
            _db = db;
            _mapper = mapper;


        }






        //Endpoint para Obtener Todas las Villas(GET)
        //Este es el 1er endpoint que nos devuelve todas las villas

        //[HttpGet] especifica que este método maneja solicitudes HTTP GET.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //ActionResult<IEnumerable<VillaDto>> indica que este método puede devolver una lista de objetos VillaDto.
        //Dentro del método, se logea un mensaje, y se retorna la lista de villas desde la base de datos.

        //Este método maneja solicitudes HTTP GET para obtener todas las villas.
        //Utiliza el contexto de la base de datos(_db) para obtener la lista de villas y luego mapea estas entidades a
        //objetos DTO(VillaDto) usando AutoMapper.
        //Devuelve un resultado Ok con la lista de objetos DTO.

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {


            _logger.LogInformation("Obtener las Villas");

            // return Ok(VillaStore.villaList);


            //Estas líneas de código parecen formar parte de un método de un controlador en una aplicación ASP.NET Core que utiliza
            //Entity Framework Core para acceder a una base de datos y AutoMapper para mapear entre los modelos de datos y
            //los DTOs (Data Transfer Objects). Aquí tienes una explicación detallada:

            //_db.Villas.ToListAsync():

            // _db parece ser una instancia de un DbContext(posiblemente ApplicationDbContext).
            //Villas es una propiedad DbSet en el DbContext que representa la tabla de base de datos asociada a la entidad Villa.
            //.ToListAsync() es un método que ejecuta una consulta para obtener todos los elementos de la tabla de manera asíncrona y devuelve
            //una lista. El método ToListAsync es asincrónico, lo que significa que no bloqueará el hilo principal mientras espera que se
            //completen las operaciones de base de datos.


            //IEnumerable<Villa> villalist = await _db.Villas.ToListAsync();:

//Se está declarando e inicializando una variable llamada villalist de tipo IEnumerable<Villa>. Esta variable se utiliza para almacenar
//los resultados de la consulta a la base de datos.
//await se usa porque ToListAsync es una operación asincrónica.Permite que el hilo no se bloquee mientras se espera que se completen
//las operaciones de base de datos.
//Después de ejecutar la consulta y obtener los resultados, villalist contendrá una lista de objetos Villa recuperados de la base de datos.



            IEnumerable<Villa> villalist = await _db.Villas.ToListAsync();


            //_mapper.Map<IEnumerable<VillaDto>>(villalist):

            //            _mapper parece ser una instancia de AutoMapper, una biblioteca que facilita el mapeo entre objetos en.NET.
            //            Map es un método de AutoMapper que toma una fuente(en este caso, villalist) y la mapea a un tipo de destino(VillaDto).
            //IEnumerable<VillaDto> es el tipo al que se está mapeando la lista de Villa.Indica que se espera una enumeración de objetos VillaDto.

            //return Ok(...):

//          Ok es un método del controlador que devuelve un código de estado HTTP 200(OK) junto con un resultado.
//En este caso, el resultado es el resultado del mapeo de villalist a IEnumerable<VillaDto> utilizando AutoMapper.


            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villalist));


        }

        // Endpoint para Obtener una Sola Villa por Id (GET)
        //Este es el 2do endpoint que nos devuelve una sola villa.

        //[HttpGet("{id:int}", Name = "GetVilla")] especifica que este método maneja solicitudes GET con una variable de ruta id de tipo entero.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //ActionResult<VillaDto> indica que este método puede devolver un objeto VillaDto.
        //GetVilla busca una villa por su ID en la base de datos y responde según el resultado.

        //Este método maneja solicitudes HTTP GET para obtener una villa por su ID.
        //Realiza validaciones y luego utiliza el contexto de la base de datos para obtener la villa por ID.
        //Devuelve un resultado Ok con el objeto DTO correspondiente a la villa encontrada.



                [HttpGet("id: int", Name ="GetVilla")]
        //codigos de estado para la documentacion.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Erro al traer Villa con Id " + id);
                return BadRequest();
            }

           // var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<VillaDto>(villa));

        }


        //[HttpPost] especifica que este método maneja solicitudes HTTP POST.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //ActionResult<VillaDto> indica que este método puede devolver un objeto VillaDto.
        //FromBody indica que los datos se toman del cuerpo de la solicitud.
        //Este método crea una nueva villa en la base de datos.



        //Este método maneja solicitudes HTTP POST para crear una nueva villa.
        //Realiza validaciones y crea un nuevo objeto Villa mapeado desde el objeto VillaCreateDto recibido.
        //Lo agrega al contexto de la base de datos y guarda los cambios, luego devuelve un resultado CreatedAtRoute con el
        //objeto DTO de la villa recién creada.

        //Este código parece ser un método de un controlador en una aplicación ASP.NET Core que crea una nueva entidad Villa
        //en la base de datos. 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            // Paso 1: Validar el estado del modelo
            //Se verifica si el modelo (createDto) pasado en la solicitud es válido. Si no es válido, se devuelve una respuesta
            //HTTP 400 (BadRequest) junto con los errores de validación del modelo.

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            //  Paso 2: Verificar si ya existe una Villa con el mismo nombre

            //Se consulta la base de datos para verificar si ya existe una villa con el mismo nombre. Si existe, se agrega un error
            //al modelo de estado (ModelState) y se devuelve una respuesta HTTP 400 con los errores de validación.

            if (//VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null
               await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)   
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe");
                return BadRequest(ModelState);  
            }

            //  Paso 3: Verificar si la VillaDto proporcionada no es nula

            //Se verifica si la VillaDto proporcionada (createDto) es nula. Si es nula, se devuelve una respuesta HTTP 400.

            if (createDto == null)
            {
                return BadRequest(createDto);
            }


            // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

            // VillaStore.villaList.Add(villaDto);



            // Paso 4: Mapear la VillaCreateDto a la entidad Villa
            //Se utiliza AutoMapper (_mapper) para mapear la VillaCreateDto (createDto) a la entidad Villa (modelo). Esto facilita
            //la asignación de propiedades entre los objetos.

            Villa modelo = _mapper.Map<Villa>(createDto);



            //Villa modelo = new()
            //{
            //    // Id = villaDto.Id,  no hay necesidad es automatico
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad


            //};


            // Paso 5: Agregar la nueva Villa al contexto y guardar los cambios

            //Se agrega la nueva entidad Villa al contexto de la base de datos y se guardan los cambios asincrónicamente.

            await _db.Villas.AddAsync(modelo);
           await _db.SaveChangesAsync();

            // Paso 6: Retornar una respuesta con código 201 (Created) y la nueva entidad creada

            //Se devuelve una respuesta HTTP 201 (Created) indicando que la operación de creación fue exitosa. También se incluye
            //la nueva entidad (modelo) en la respuesta. La ruta "GetVilla" es probablemente una ruta de detalle para obtener
            //información sobre la entidad recién creada.


            return CreatedAtRoute("GetVilla", new {id=modelo.Id}, modelo);

            //En resumen, este método realiza la validación del modelo, verifica la existencia de duplicados, crea una nueva entidad
            //Villa en la base de datos y devuelve una respuesta HTTP con la entidad creada.

        }


        // Endpoint para Eliminar una Villa por Id(DELETE)

        //[HttpDelete("{id:int}")] especifica que este método maneja solicitudes DELETE con una variable de ruta id de tipo entero.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //IActionResult indica que este método puede devolver cualquier tipo de resultado.
        //Este método elimina una villa de la base de datos por su ID.

        //Este código representa un endpoint en un controlador de una aplicación ASP.NET Core que maneja solicitudes DELETE para
        //eliminar una entidad Villa de la base de datos por su ID. Aquí tienes una explicación paso a paso:

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteVilla(int id)
        {

            // Paso 1: Verificar si el ID es igual a cero, en cuyo caso se devuelve un BadRequest.
            //Se verifica si el ID proporcionado en la solicitud es igual a cero. Si es así, se devuelve una respuesta
            //HTTP 400 (BadRequest). Esto puede indicar que el ID proporcionado no es válido.


            if (id==0)
            {
                return BadRequest();
            }



            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            // Paso 2: Consultar la base de datos para obtener la villa con el ID proporcionado.
            //Se utiliza Entity Framework Core para consultar la base de datos y obtener la entidad Villa con el ID proporcionado.
            //El resultado se almacena en la variable villa.


            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id==id);
            // Si no se encuentra la villa, se devuelve un NotFound.
            // Paso 3: Verificar si la villa no se encontró(es nula):
            //Se verifica si la variable villa es nula, lo que significa que no se encontró una villa con el ID proporcionado
            //en la base de datos. En este caso, se devuelve una respuesta HTTP 404 (NotFound).

            if (villa == null)
            {
                return NotFound();
            }

            // VillaStore.villaList.Remove(villa);
            // Paso 4: Eliminar la villa de la base de datos.
            //Se utiliza Entity Framework Core para eliminar la entidad villa de la base de datos. La eliminación se confirma
            //llamando a SaveChangesAsync para guardar los cambios en la base de datos de manera asincrónica.

            _db.Villas.Remove(villa);
           await _db.SaveChangesAsync();


            // Paso 5: Devolver una respuesta sin contenido (204 No Content) indicando que la eliminación fue exitosa.
            //Finalmente, se devuelve una respuesta HTTP 204 (No Content) para indicar que la operación de eliminación fue exitosa
            //y que no hay contenido adicional que enviar en el cuerpo de la respuesta.


            return NoContent();


        }





        // Endpoint para Actualizar una Villa por Id (PUT)

        //[HttpPut("{id:int}")] especifica que este método maneja solicitudes PUT con una variable de ruta id de tipo entero.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //IActionResult indica que este método puede devolver cualquier tipo de resultado.
        //Este método actualiza una villa en la base de datos por su ID.


        //Este código es un método de un controlador de una API en ASP.NET Core que se encarga de manejar solicitudes HTTP PUT para
        //actualizar una entidad de tipo Villa en la base de datos. A continuación, te explico paso a paso lo que hace el código:



        // 1) Atributos de Ruta y de Respuesta

        //[HttpPut("{id:int}")]: Indica que este método maneja solicitudes HTTP PUT y espera un parámetro de ruta llamado id que
        //debe ser un entero.

        //[ProducesResponseType]: Especifica los códigos de estado de respuesta que puede devolver este método.En este caso,
        //204 (No Content) si la actualización fue exitosa y 400 (Bad Request) si hay algún problema.
        


                [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        // 2) Método UpdateVilla

        //public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto): Este es el encabezado del método.
        //Recibe el parámetro id de la ruta y el objeto VillaUpdateDto desde el cuerpo de la solicitud.
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            // Validar si el DTO es nulo o si el id en el DTO no coincide con el id en la ruta.

            //La primera validación (if (updateDto == null || id != updateDto.Id)) verifica si el DTO recibido es nulo o si el id en
            //el DTO no coincide con el id en la ruta. En ese caso, se devuelve un resultado BadRequest indicando que la solicitud no
            //es válida.


            if (updateDto == null || id != updateDto.Id)
            {


                return BadRequest();

            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id); 
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;



            // Mapear el DTO a la entidad del modelo
            //Se utiliza AutoMapper (_mapper) para mapear el objeto VillaUpdateDto a la entidad del modelo Villa.

            Villa modelo = _mapper.Map<Villa>(updateDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad


            //};


            // Actualizar la entidad en la base de datos

            //Se actualiza la entidad en la base de datos utilizando _db.Villas.Update(modelo);.


            _db.Villas.Update(modelo);

            //Se guardan los cambios en la base de datos con await _db.SaveChangesAsync();.

            await _db.SaveChangesAsync();


            // Devolver respuesta exitosa (204 - No Content)
            //Finalmente, se devuelve un resultado exitoso (204 - No Content) indicando que la actualización fue exitosa.

            return NoContent();

            //En resumen, este método maneja solicitudes PUT para actualizar una entidad Villa en la base de datos.
            //Realiza validaciones, mapea el objeto DTO a la entidad del modelo, actualiza la entidad en la base de datos y
            //devuelve un código de estado 204 si la actualización fue exitosa.



        }




        //Endpoint para Actualizar Parcialmente una Villa por Id (PATCH)

        //[HttpPatch("{id:int}")] especifica que este método maneja solicitudes PATCH con una variable de ruta id de tipo entero.
        //[ProducesResponseType] especifica los códigos de estado de respuesta para la documentación.
        //IActionResult indica que este método puede devolver cualquier tipo de resultado.
        //Este método actualiza parcialmente una villa en la base de datos por su ID.

        //Este código es un método de un controlador de una API en ASP.NET Core que maneja solicitudes HTTP PATCH para actualizar
        //parcialmente una entidad de tipo Villa en la base de datos. A continuación, te explico paso a paso lo que hace el código:


        // 1) Atributos de Ruta y de Respuesta

        //[HttpPatch("{id:int}")]: Indica que este método maneja solicitudes HTTP PATCH y espera un parámetro de ruta llamado id
        //que debe ser un entero.

        // [ProducesResponseType]: Especifica los códigos de estado de respuesta que puede devolver este método.En este caso,
        // 204 (No Content) si la actualización parcial fue exitosa y 400 (Bad Request) si hay algún problema.
        



        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //2) Método UpdatePartialVilla

        //public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto): Este es el encabezado
        //del método. Recibe el parámetro id de la ruta y un JsonPatchDocument que contiene los cambios parciales en el cuerpo de
        //la solicitud.


        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            // Validar si el JsonPatchDocument o el id son inválidos
            //La primera validación (if (patchDto == null || id == 0)) verifica si el JsonPatchDocument o el id son inválidos.
            //En ese caso, se devuelve un resultado BadRequest indicando que la solicitud no es válida.

            if (patchDto == null || id == 0)
            {


                return BadRequest();

            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            // Obtener la villa desde la base de datos sin realizar tracking (AsNoTracking)
            //Se obtiene la entidad Villa desde la base de datos sin realizar tracking (AsNoTracking) para evitar problemas de concurrencia.

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);


            // Mapear la villa a un DTO para aplicar los cambios parciales
            //Se mapea la entidad Villa a un objeto VillaUpdateDto para aplicar los cambios parciales.

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);


            //VillaUpdateDto villaDto = new()
            //{
            //    Id = villa.Id,
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    ImagenUrl = villa.ImagenUrl,
            //    Ocupantes = villa.Ocupantes,
            //    Tarifa = villa.Tarifa,
            //    MetrosCuadrados = villa.MetrosCuadrados,
            //    Amenidad = villa.Amenidad

            //};


            // Si la villa no existe, devolver un BadRequest
            //Se verifica si la villa existe en la base de datos y, si no existe, se devuelve un resultado BadRequest.

            if (villa == null)
            {
                return BadRequest();
            }

            // Aplicar los cambios parciales al DTO usando el JsonPatchDocument
            //Se aplican los cambios parciales al DTO usando el método ApplyTo del JsonPatchDocument.

            patchDto.ApplyTo(villaDto, ModelState);


            // Validar si el modelo es válido según las reglas de validación del DTO
            //Se valida si el modelo es válido según las reglas de validación del DTO. Si no es válido, se devuelve un resultado
            //BadRequest con los errores de validación.

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);

            }


            // Mapear el DTO actualizado a la entidad del modelo
            //Se mapea el DTO actualizado a la entidad del modelo.

            Villa modelo = _mapper.Map<Villa>(villaDto);


            //Villa modelo = new()
            //{

            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad


            //};


            // Actualizar la entidad en la base de datos
            //Se actualiza la entidad en la base de datos con _db.Villas.Update(modelo); y se guardan los cambios
            //con await _db.SaveChangesAsync();.

            _db.Villas.Update(modelo);
           await _db.SaveChangesAsync();


            // Devolver respuesta exitosa (204 - No Content)
            //Finalmente, se devuelve un resultado exitoso (204 - No Content) indicando que la actualización parcial fue exitosa.

            return NoContent();


        }


        //En resumen, este método maneja solicitudes PATCH para actualizar parcialmente una entidad Villa en la base de datos.
        //Realiza validaciones, aplica los cambios parciales, actualiza la entidad en la base de datos y devuelve un código de
        //estado 204 si la actualización parcial fue exitosa.


    }
}
