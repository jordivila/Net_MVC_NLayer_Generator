using System;

namespace $safeprojectname$.Configuration.ConnectionProviders
{
    public static class Info
    {
        public static string GetDatabaseName(ApplicationConfiguration.DatabaseNames connectionString)
        {
            string cnnProvider = string.Empty;
            switch (connectionString)
            {
                case ApplicationConfiguration.DatabaseNames.Membership:
                    cnnProvider = "Membership Database String";
                    break;
                case ApplicationConfiguration.DatabaseNames.Logging:
                    cnnProvider = "Logging Database String";
                    break;
                default:
                    throw new Exception("SQL Provider No Especificado. Debes asignar una cadena de conexion para ejecutar procedimientos o sentencias SQL");
                //break;
            }
            return cnnProvider;
        }
    }
}
