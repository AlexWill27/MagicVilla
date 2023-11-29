// 1)Imports:

    //Aquí se están importando los espacios de nombres necesarios, incluyendo AutoMapper y los modelos y DTOs (Data Transfer Objects) 
    //que se van a mapear.


using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

//Este código pertenece a una configuración de mapeo (mapping) utilizando la biblioteca AutoMapper en C#. 
//AutoMapper es una biblioteca que simplifica el proceso de mapeo de datos entre objetos de diferentes tipos. 
//El código que has proporcionado está definido en una clase llamada MappingConfig que hereda de Profile, una clase proporcionada
//por AutoMapper para configurar los mapeos.

namespace MagicVilla_API
{
   //La clase MappingConfig hereda de Profile, que es proporcionada por AutoMapper y se utiliza para definir la configuración de los mapeos.
    public class MappingConfig : Profile
    {
        //Constructor MappingConfig():
        //El constructor de la clase MappingConfig es donde se define la configuración de los mapeos.
        public MappingConfig()
        {
            //Mapeos (CreateMap):

            CreateMap<Villa, VillaDto>();   //Este mapeo indica que se debe mapear un objeto de tipo Villa a un objeto de tipo VillaDto
            CreateMap<VillaDto, Villa>();  //Este mapeo indica que se debe mapear un objeto de tipo VillaDto a un objeto de tipo Villa.

            //Este mapeo indica que se debe mapear un objeto de tipo Villa a un objeto de tipo VillaCreateDto y viceversa.
            //El método ReverseMap simplifica la configuración cuando los DTOs son similares a las entidades, como en el caso 
            //de un DTO utilizado para la creación de una entidad.

            CreateMap<Villa, VillaCreateDto>().ReverseMap();

            //: Similar al anterior, pero con un DTO diferente (VillaUpdateDto), utilizado probablemente para actualizaciones.

            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            //En resumen, este código configura AutoMapper para realizar mapeos entre varios tipos de objetos, principalmente entre
            //las entidades Villa y los DTOs asociados (VillaDto, VillaCreateDto, y VillaUpdateDto). Este enfoque facilita
            //la transformación de datos entre los objetos de la capa de modelos y los objetos utilizados para transferir datos dentro
            //y fuera de la aplicación (DTOs).


        }



    }
}

//El término "mapeo" se refiere al proceso de asociar o vincular los campos de un objeto de un tipo a los campos de otro objeto
//de otro tipo. En el contexto de AutoMapper y otros sistemas de mapeo de objetos en programación, este proceso implica copiar
//datos de un objeto a otro, asegurando que los campos coincidan según ciertas reglas de configuración. En términos más simples,
//el mapeo es la acción de transferir datos de un objeto a otro.

//En términos de objetivos y razones para hacer mapeos:

//Transferencia de Datos:

//El objetivo principal de los mapeos es facilitar la transferencia de datos entre diferentes capas de una aplicación, como entre la capa
//de acceso a datos y la capa de presentación.
//Pueden ser utilizados para convertir objetos de modelos de dominio a objetos de transferencia de datos (DTO) que se envían a través
//de una API.

//Separación de Responsabilidades:

//Los mapeos ayudan a mantener una separación de responsabilidades entre las entidades del dominio y los objetos utilizados para
//la transferencia de datos.
//Las entidades del dominio pueden contener lógica específica del negocio, mientras que los DTOs están diseñados para la transferencia
//eficiente de datos.


//Adaptación de Estructuras Diferentes:

/*Pueden ser útiles cuando las estructuras de los objetos en diferentes capas de la aplicación no son idénticas, por ejemplo, cuando 
los modelos de base de datos no coinciden exactamente con los modelos utilizados en la  interfaz de usuario. */


//En el código que proporcionaste, los objetos mencionados son:

//Villa: Una entidad o modelo que probablemente representa una entidad en la base de datos.
//VillaDto: Un objeto de transferencia de datos (DTO) que se utiliza para transferir información relacionada con Villa.
//VillaCreateDto y VillaUpdateDto: DTOs utilizados para la creación y actualización de entidades Villa.

//En resumen, los mapeos se configuran para facilitar la transformación de datos entre diferentes tipos de objetos, permitiendo
//una comunicación más eficiente y estructurada entre diferentes capas de una aplicación.