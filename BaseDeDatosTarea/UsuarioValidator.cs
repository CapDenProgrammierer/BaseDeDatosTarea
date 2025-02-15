using System.Data.SqlClient;
using System;

namespace BaseDeDatosTarea
{
    public class UsuarioValidator
    {
        public bool ValidarUsuario(string servidor, string baseDatos, string username, string password)
        {
            // Construir la cadena de conexión con las credenciales proporcionadas,
            // agregando además Encrypt=False y MultipleActiveResultSets=True según tu configuración.
            string connectionString = $"Data Source={servidor};Initial Catalog={baseDatos};User ID={username};Password={password};Encrypt=False;MultipleActiveResultSets=True;";

            try
            {
                // Intentar abrir la conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Si la conexión se abrió correctamente, se considera que el usuario es válido
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Error al intentar conectarse (credenciales inválidas o problemas de conexión)
                Console.WriteLine("Error al validar el usuario: " + ex.Message);
                return false;
            }
        }
    }
}