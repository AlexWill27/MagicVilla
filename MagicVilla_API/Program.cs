//Aquí se están importando los espacios de nombres necesarios para la aplicación, incluyendo el espacio de nombres
//principal (MagicVilla_API), el espacio de nombres para el manejo de datos (MagicVilla_API.Datos), y el espacio de
//nombres para Entity Framework Core (Microsoft.EntityFrameworkCore).



using MagicVilla_API;
using MagicVilla_API.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

//Creación del Constructor de Aplicación:

//WebApplication.CreateBuilder(args): Esta línea crea un constructor de aplicación web utilizando los argumentos de línea
//de comandos proporcionados (args). El objeto builder se utilizará para configurar y construir la aplicación.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Añadir Servicios al Contenedor de Inyección de Dependencias:

builder.Services.AddControllers().AddNewtonsoftJson();   //Configura los servicios para controladores y utiliza Newtonsoft.Json como el serializador JSON para esos controladores.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();  //  Agrega servicios para exponer información sobre los puntos finales de la API, útil para herramientas de documentación como Swagger.
builder.Services.AddSwaggerGen();  // Agrega servicios para generar la especificación Swagger/OpenAPI.



//Configura el servicio del contexto de la base de datos para Entity Framework Core, utilizando SQL Server como proveedor de base de datos.
//La cadena de conexión se obtiene desde la configuración de la aplicación.


builder.Services.AddDbContext<ApplicationDbContext>(option =>
{

    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Agrega servicios relacionados con AutoMapper, indicando que la configuración de mapeo se encuentra en la clase MappingConfig.

builder.Services.AddAutoMapper(typeof(MappingConfig));




//Construcción de la Aplicación:

var app = builder.Build();    // builder.Build(): Construye la aplicación utilizando la configuración previamente definida.

// Configure the HTTP request pipeline.
//Configure la canalización de solicitudes HTTP.
//Configuración del Pipeline de Solicitudes HTTP:

//app.Environment.IsDevelopment(): Verifica si la aplicación está en modo de desarrollo. En el modo de desarrollo,
//habilita Swagger y Swagger UI para la documentación de la API.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware para Redirección HTTPS:
//app.UseHttpsRedirection(): Habilita el middleware para redirigir solicitudes HTTP a HTTPS.

app.UseHttpsRedirection();

//Middleware de Autorización:
//app.UseAuthorization(): Habilita el middleware de autorización, que verifica la autorización del usuario para acceder a
//los recursos protegidos.
app.UseAuthorization();


//Enrutamiento de Controladores:
//app.MapControllers(): Configura el enrutamiento para los controladores, indicando que los controladores deben ser accesibles a
//través de las rutas especificadas en los atributos de los controladores.

app.MapControllers();

//Inicio de la Aplicación:

//app.Run(): Inicia la aplicación y comienza a escuchar solicitudes HTTP.


app.Run();

//En resumen, este código configura y construye una aplicación web utilizando ASP.NET Core, define servicios como controladores,
//configuración de Swagger/OpenAPI, Entity Framework Core para acceso a la base de datos, AutoMapper para mapeo de objetos, y
//establece el pipeline de solicitudes HTTP con middleware para redirección HTTPS y autorización. Además, en el modo de desarrollo,
//habilita Swagger para documentación.



//El "pipeline de solicitudes HTTP" (HTTP request pipeline en inglés) es una serie de componentes (también conocidos como middleware) que procesan una solicitud HTTP entrante en una aplicación ASP.NET Core antes de que llegue a la lógica principal de la aplicación. Cada componente en el pipeline tiene la oportunidad de realizar alguna acción específica o modificar la solicitud antes de pasarla al siguiente componente.

//En una aplicación ASP.NET Core, el pipeline de solicitudes HTTP es configurado en el método Configure de la clase Startup. El pipeline sigue un orden específico, y cada middleware tiene la opción de pasar la solicitud al siguiente middleware o finalizar el procesamiento.

//Aquí hay una descripción general de algunos middleware comunes que pueden formar parte del pipeline de solicitudes HTTP:

//Middleware de Enrutamiento (UseRouting): Este middleware examina la solicitud y la envía al controlador apropiado basándose en la ruta especificada en la URL.

//Middleware de Autenticación (UseAuthentication): Maneja la autenticación del usuario. Puede realizar la verificación de credenciales y establecer la identidad del usuario en la solicitud.

//Middleware de Autorización (UseAuthorization): Maneja la autorización del usuario. Comprueba si el usuario tiene permiso para acceder a los recursos solicitados.

//Middleware de Manejo de Errores (UseExceptionHandler): Captura y maneja excepciones que ocurren durante el procesamiento de la solicitud.

//Middleware de Redirección HTTPS (UseHttpsRedirection): Redirige solicitudes HTTP a HTTPS para garantizar conexiones seguras.

//Middleware de Swagger (UseSwagger y UseSwaggerUI): Permite la generación y visualización de la documentación de la API usando Swagger/OpenAPI.

//Middleware de Controladores (MapControllers): Enruta la solicitud al controlador apropiado según la ruta especificada en la URL.

//Middleware de Ejecución Final (Run): Define la acción final que se realiza antes de enviar la respuesta al cliente.

//Estos son solo algunos ejemplos y la configuración del pipeline puede variar según los requisitos específicos de la aplicación. La capacidad de personalizar el pipeline de solicitudes es una característica fundamental de ASP.NET Core, y permite a los desarrolladores controlar cómo se procesan las solicitudes en cada etapa del flujo de trabajo de la aplicación.





