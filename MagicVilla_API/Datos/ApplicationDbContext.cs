using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_API.Datos
{

    //En este ejemplo, ApplicationDbContext hereda de DbContext y define un conjunto de entidades(DbSet<Villa> Villas) que representa 
    //una tabla en la base de datos.También tiene un constructor que acepta opciones de configuración, incluida la cadena de conexión.
    //Con esta clase, puedes interactuar con las entidades (Villas en este caso) y realizar operaciones en la base de datos.
    public class ApplicationDbContext : DbContext
    {

        // Constructor que acepta opciones de configuración, como la cadena de conexión
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // Representa una tabla en la base de datos
        public DbSet<Villa> Villas { get; set; }

        // Este fragmento de código pertenece al método OnModelCreating de una clase que hereda de DbContext.En Entity Framework,
        //este método se utiliza para configurar el modelo de base de datos y establecer ciertas configuraciones.En este caso, se 
        //está utilizando para insertar datos iniciales (seeding) en la base de datos.

        //Sobrecarga del Método OnModelCreating:

        //OnModelCreating es un método protegido que se puede anular en una clase derivada de DbContext.Se llama cuando el 
        //modelo de base de datos está siendo creado.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Configuración de Datos Iniciales(Seeding):

            //Dentro de este método, se utiliza modelBuilder.Entity<T>().HasData() para especificar datos iniciales para la entidad Villa. 
            //HasData se utiliza para insertar registros iniciales en la tabla correspondiente a la entidad.

            modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
                Id = 1,
                Nombre = "Villa Real",
                Detalle = "Detalle de la Villa",
                ImagenUrl = " ",
                Ocupantes = 5,
                MetrosCuadrados = 50,
                Tarifa = 200,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
            },
            new Villa()
                {
                    Id = 2,
                    Nombre = "Premium Vista a la Piscina",
                    Detalle = "Detalle de la Villa",
                    ImagenUrl = " ",
                    Ocupantes = 4,
                    MetrosCuadrados = 40,
                    Tarifa = 150,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,


                }
             );   




        }

    }
}
