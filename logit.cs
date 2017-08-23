using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text;


/**
 * Script para escritura de un archivo log.
 * 
 * DB-dev
 * 
 * v 1.0
 * 
 * //////////////
 * //// TODO ////
 * //////////////
 * 
 * 
 **/


namespace OrganizarTiming
{
    class logit
    {
        public static void logThis(String text)
        {
            try
            {
                // Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                // Esta sentencia devuelve la ruta donde está el exe, sin el nombre del exe.
                // Assembly.GetEntryAssembly().Location
                // Esta retorna la ruta donde está el exe, con el nombre del exe.

                // ¿Existe la carpeta logs en la ruta del ejecutable?
                if (!Directory.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\logs"))
                    Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\logs");

                StringBuilder sb = new StringBuilder();

                text = "[ [" + DateTime.Now.ToString() + "] ] -->  " + text + "\n";

                sb.Append(text);

                File.AppendAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\logs\\Logs_" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt", sb.ToString());

                sb.Clear();
            }
            catch (Exception ex) { }
        }
    }
}
