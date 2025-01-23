# AppBibliotecaWebG1

**AppBibliotecaWebG1** es una aplicación web desarrollada en **ASP.NET Core** que permite gestionar una biblioteca de forma eficiente. Incluye funcionalidades como la visualización, creación, edición y eliminación de libros, asegurando la seguridad de los datos mediante un sistema de autenticación de usuarios. Su diseño intuitivo y características avanzadas hacen que la gestión de la biblioteca sea más sencilla y accesible.

## 📋 Características Principales

- **Autenticación de Usuarios**: Acceso restringido a usuarios registrados, garantizando la privacidad y seguridad de la información.
- **Gestión de Libros**: Funcionalidades completas de CRUD (Crear, Leer, Actualizar, Eliminar) para administrar los libros de la biblioteca.
- **Carga de Imágenes**: Posibilidad de agregar una foto representativa para cada libro, almacenada en la base de datos.
- **Visualización Amigable**: Los libros se listan en una interfaz atractiva y fácil de usar.
- **Funcionalidad de Correos Electrónicos**: Se envían correos automáticos a los nuevos usuarios registrados con detalles de su cuenta.

## 🛠️ Tecnologías Utilizadas

- **ASP.NET Core**: Framework para construir aplicaciones web modernas y escalables.
- **Entity Framework Core**: ORM para la interacción eficiente con la base de datos.
- **HTML/CSS**: Construcción de la interfaz visual.
- **JavaScript**: Mejoras interactivas en la interfaz del usuario.
- **SMTP**: Para la funcionalidad de envío de correos electrónicos.

## 📚 Funcionalidades

### 1. Gestión de Libros
- **Agregar**: Crear nuevos registros de libros, con información como título, autor, foto, etc.
- **Editar**: Modificar la información existente de un libro.
- **Eliminar**: Borrar registros no deseados de forma segura.
- **Ver Detalles**: Visualizar información completa sobre un libro específico.

### 2. Autenticación
- Solo los usuarios autenticados pueden acceder al sistema.
- Funcionalidad de recuperación de contraseñas mediante correo electrónico.

### 3. Envío de Correos Electrónicos
Se utiliza una clase personalizada para enviar correos electrónicos a los usuarios, incluyendo contraseñas temporales y notificaciones importantes.

#### Configuración del Correo
La clase `Email` utiliza el protocolo **SMTP** para enviar correos electrónicos desde un servidor configurado (por defecto, **Outlook**):

```csharp
smtp.Host = "smtp-mail.outlook.com";
smtp.Port = 587;
smtp.EnableSsl = true;
smtp.Credentials = new NetworkCredential("correo@outlook.com", "contraseña_segura");
```

El correo enviado incluye:
- Asunto: "Datos de registro en plataforma web biblioteca CR".
- Mensaje personalizado con el nombre del usuario, correo registrado y contraseña temporal.

### 4. Carga y Almacenamiento de Imágenes
- Las imágenes de los libros se cargan desde el formulario y se almacenan en la carpeta `wwwroot/css/img`.
- Se asegura que los nombres de los archivos sean únicos para evitar conflictos.

## 📑 Requisitos del Sistema

- **.NET SDK 6.0 o superior**.
- **SQL Server** para la base de datos.
- Navegador web actualizado.
- **SMTP** configurado para el envío de correos.

## 🚀 Cómo Ejecutar el Proyecto

1. **Clonar el Repositorio**:
   ```bash
   git clone https://github.com/tu-usuario/AppBibliotecaWebG1.git
   ```
2. **Configurar la Base de Datos**:
   - Edita la conexión en `appsettings.json` para que apunte a tu servidor SQL:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=tu_servidor;Database=BibliotecaDB;User Id=usuario;Password=contraseña;"
     }
     ```

3. **Restaurar Paquetes NuGet**:
   ```bash
   dotnet restore
   ```

4. **Ejecutar Migraciones**:
   ```bash
   dotnet ef database update
   ```

5. **Iniciar la Aplicación**:
   ```bash
   dotnet run
   ```

6. Abre tu navegador y accede a `http://localhost:5000`.

## 📂 Estructura del Proyecto

- **Controllers**: Contiene los controladores de la aplicación, como `LibrosController`.
- **Models**: Define las clases de entidad, como `Libro`.
- **Views**: Archivos Razor (.cshtml) para la interfaz de usuario.
- **wwwroot**: Archivos estáticos como CSS, imágenes y JavaScript.
