//Aqu� se est�n importando los espacios de nombres necesarios para la aplicaci�n, incluyendo el espacio de nombres
//principal (MagicVilla_API), el espacio de nombres para el manejo de datos (MagicVilla_API.Datos), y el espacio de
//nombres para Entity Framework Core (Microsoft.EntityFrameworkCore).



using MagicVilla_API;
using MagicVilla_API.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

//Creaci�n del Constructor de Aplicaci�n:

//WebApplication.CreateBuilder(args): Esta l�nea crea un constructor de aplicaci�n web utilizando los argumentos de l�nea
//de comandos proporcionados (args). El objeto builder se utilizar� para configurar y construir la aplicaci�n.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//A�adir Servicios al Contenedor de Inyecci�n de Dependencias:

builder.Services.AddControllers().AddNewtonsoftJson();   //Configura los servicios para controladores y utiliza Newtonsoft.Json como el serializador JSON para esos controladores.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();  //  Agrega servicios para exponer informaci�n sobre los puntos finales de la API, �til para herramientas de documentaci�n como Swagger.
builder.Services.AddSwaggerGen();  // Agrega servicios para generar la especificaci�n Swagger/OpenAPI.



//Configura el servicio del contexto de la base de datos para Entity Framework Core, utilizando SQL Server como proveedor de base de datos.
//La cadena de conexi�n se obtiene desde la configuraci�n de la aplicaci�n.


builder.Services.AddDbContext<ApplicationDbContext>(option =>
{

    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Agrega servicios relacionados con AutoMapper, indicando que la configuraci�n de mapeo se encuentra en la clase MappingConfig.

builder.Services.AddAutoMapper(typeof(MappingConfig));




//Construcci�n de la Aplicaci�n:

var app = builder.Build();    // builder.Build(): Construye la aplicaci�n utilizando la configuraci�n previamente definida.

// Configure the HTTP request pipeline.
//Configure la canalizaci�n de solicitudes HTTP.
//Configuraci�n del Pipeline de Solicitudes HTTP:

//app.Environment.IsDevelopment(): Verifica si la aplicaci�n est� en modo de desarrollo. En el modo de desarrollo,
//habilita Swagger y Swagger UI para la documentaci�n de la API.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware para Redirecci�n HTTPS:
//app.UseHttpsRedirection(): Habilita el middleware para redirigir solicitudes HTTP a HTTPS.

app.UseHttpsRedirection();

//Middleware de Autorizaci�n:
//app.UseAuthorization(): Habilita el middleware de autorizaci�n, que verifica la autorizaci�n del usuario para acceder a
//los recursos protegidos.
app.UseAuthorization();


//Enrutamiento de Controladores:
//app.MapControllers(): Configura el enrutamiento para los controladores, indicando que los controladores deben ser accesibles a
//trav�s de las rutas especificadas en los atributos de los controladores.

app.MapControllers();

//Inicio de la Aplicaci�n:

//app.Run(): Inicia la aplicaci�n y comienza a escuchar solicitudes HTTP.


app.Run();

//En resumen, este c�digo configura y construye una aplicaci�n web utilizando ASP.NET Core, define servicios como controladores,
//configuraci�n de Swagger/OpenAPI, Entity Framework Core para acceso a la base de datos, AutoMapper para mapeo de objetos, y
//establece el pipeline de solicitudes HTTP con middleware para redirecci�n HTTPS y autorizaci�n. Adem�s, en el modo de desarrollo,
//habilita Swagger para documentaci�n.



//El "pipeline de solicitudes HTTP" (HTTP request pipeline en ingl�s) es una serie de componentes (tambi�n conocidos como middleware) que procesan una solicitud HTTP entrante en una aplicaci�n ASP.NET Core antes de que llegue a la l�gica principal de la aplicaci�n. Cada componente en el pipeline tiene la oportunidad de realizar alguna acci�n espec�fica o modificar la solicitud antes de pasarla al siguiente componente.

//En una aplicaci�n ASP.NET Core, el pipeline de solicitudes HTTP es configurado en el m�todo Configure de la clase Startup. El pipeline sigue un orden espec�fico, y cada middleware tiene la opci�n de pasar la solicitud al siguiente middleware o finalizar el procesamiento.

//Aqu� hay una descripci�n general de algunos middleware comunes que pueden formar parte del pipeline de solicitudes HTTP:

//Middleware de Enrutamiento (UseRouting): Este middleware examina la solicitud y la env�a al controlador apropiado bas�ndose en la ruta especificada en la URL.

//Middleware de Autenticaci�n (UseAuthentication): Maneja la autenticaci�n del usuario. Puede realizar la verificaci�n de credenciales y establecer la identidad del usuario en la solicitud.

//Middleware de Autorizaci�n (UseAuthorization): Maneja la autorizaci�n del usuario. Comprueba si el usuario tiene permiso para acceder a los recursos solicitados.

//Middleware de Manejo de Errores (UseExceptionHandler): Captura y maneja excepciones que ocurren durante el procesamiento de la solicitud.

//Middleware de Redirecci�n HTTPS (UseHttpsRedirection): Redirige solicitudes HTTP a HTTPS para garantizar conexiones seguras.

//Middleware de Swagger (UseSwagger y UseSwaggerUI): Permite la generaci�n y visualizaci�n de la documentaci�n de la API usando Swagger/OpenAPI.

//Middleware de Controladores (MapControllers): Enruta la solicitud al controlador apropiado seg�n la ruta especificada en la URL.

//Middleware de Ejecuci�n Final (Run): Define la acci�n final que se realiza antes de enviar la respuesta al cliente.

//Estos son solo algunos ejemplos y la configuraci�n del pipeline puede variar seg�n los requisitos espec�ficos de la aplicaci�n. La capacidad de personalizar el pipeline de solicitudes es una caracter�stica fundamental de ASP.NET Core, y permite a los desarrolladores controlar c�mo se procesan las solicitudes en cada etapa del flujo de trabajo de la aplicaci�n.





