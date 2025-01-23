using Microsoft.EntityFrameworkCore;

namespace AppBibliotecaWebG1.Models
{
    //Muy importante que la clase Herede de la clase padre DbContext
    public class DbContextBiblioteca : DbContext
    {
        /// <summary>
        /// Constructor con parámetros recibe la referencia del  ORM
        /// </summary>
        /// <param name="options"></param>
        public DbContextBiblioteca(DbContextOptions<DbContextBiblioteca> options) : base(options)
        {

        }
        //Propiedad  DbSet que permite dar mantenimiento al catalogo de libros
        public DbSet<Libro> Libros { get; set; }

        //propiedad DbSet que permite dar mantenimiento al catálogo de usuarios
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }


        //Método encargado de crear la table para el entidad en la db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>().HasData(new Libro()
            {
                ISBN = 1,
                Titulo = "Lenguajes de programación",
                Editorial = "Puntarenas",
                PrecioVenta = 27500,
                Foto="ND",
                FechaPublicacion = DateTime.Now,
                Estado = 'A'
            });
            modelBuilder.Entity<Persona>()
                .HasNoKey(); // Configura la entidad como sin clave
        }
       

    } //cierre class
} //cierre namespace
