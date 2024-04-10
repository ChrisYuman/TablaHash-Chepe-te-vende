using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static Hashtable usuarios = new Hashtable();

    static void Main(string[] args)
    {
        // Ejemplo de registro de usuarios
        RegistrarUsuario("Juan", "juan@correo.com", "juan123");
        RegistrarUsuario("María", "maria@correo.com", "maria123");
        RegistrarUsuario("Pedro", "pedro@correo.com", "pedro123");

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Ingresar un nuevo usuario");
            Console.WriteLine("2. Buscar un usuario por su correo electrónico");
            Console.WriteLine("3. Salir");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    IngresarNuevoUsuario();
                    break;
                case "2":
                    BuscarUsuarioPorCorreo();
                    break;
                case "3":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                    break;
            }
        }
    }

    static void IngresarNuevoUsuario()
    {
        Console.WriteLine("Ingrese el nombre del nuevo usuario:");
        string nombreNuevoUsuario = Console.ReadLine();
        Console.WriteLine("Ingrese el correo electrónico del nuevo usuario:");
        string correoNuevoUsuario = Console.ReadLine();
        Console.WriteLine("Ingrese la contraseña del nuevo usuario:");
        string contraseñaNuevoUsuario = Console.ReadLine();

        RegistrarUsuario(nombreNuevoUsuario, correoNuevoUsuario, contraseñaNuevoUsuario);
    }

    static void BuscarUsuarioPorCorreo()
    {
        Console.WriteLine("Ingrese el correo electrónico del usuario que desea buscar:");
        string correoBusqueda = Console.ReadLine();
        Usuario usuarioEncontrado = ObtenerUsuarioPorCorreo(correoBusqueda);
        if (usuarioEncontrado != null)
        {
            Console.WriteLine($"Nombre: {usuarioEncontrado.Nombre}, Correo Electrónico: {usuarioEncontrado.Correo}, Contraseña: { usuarioEncontrado.Password}");
        }
        else
        {
            Console.WriteLine("Usuario no encontrado.");
        }
    }

    static void RegistrarUsuario(string nombre, string correo, string password)
    {
        // Verificar si el correo electrónico ya está en uso
        if (usuarios.ContainsKey(correo))
        {
            Console.WriteLine($"El correo electrónico '{correo}' ya está en uso.");
            return;
        }

        // Encriptar la contraseña con SHA-256
        string passwordEncriptado = EncriptarSHA256(password);

        // Crear el usuario y almacenarlo en la tabla hash
        Usuario nuevoUsuario = new Usuario(nombre, correo, passwordEncriptado);
        usuarios.Add(correo, nuevoUsuario);
        Console.WriteLine("Usuario registrado exitosamente.");
    }

    static Usuario ObtenerUsuarioPorCorreo(string correo)
    {
        // Buscar el usuario en la tabla hash por su correo electrónico
        if (usuarios.ContainsKey(correo))
        {
            Usuario usuario = (Usuario)usuarios[correo];
            return new Usuario(usuario.Nombre, usuario.Correo, EncriptarSHA256(usuario.Password));
        }
        else
        {
            return null;
        }
    }

    static string EncriptarSHA256(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

class Usuario
{
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Password { get; set; }

    public Usuario(string nombre, string correo, string password)
    {
        Nombre = nombre;
        Correo = correo;
        Password = password;
    }
}
