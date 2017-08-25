using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

/**
 * Script para el copiado diario del archivo de timing.
 * 
 * DB-dev
 * 
 * v 2.1.3
 * 
 * //////////////
 * //// TODO ////
 * //////////////
 * 
 * 
 **/


namespace OrganizarTiming
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime fecha_hoy = System.DateTime.Today;
            String p_Desktop = "";
            String f_Desktop = "";
            String f_extension = "";
            String p_Plantilla = "";
            String f_Plantilla = "";
            int dias = -1;  // Dias a restar

            int ks = 0; // Kill switch

            logit.logThis("INICIO DE LA EJECUCIÓN.");

            logit.logThis("¿Existe el archivo XML?");
            // Archivo XML, si no existe lo creamos y salimos.
            if (!File.Exists("config.xml"))
            {
                logit.logThis("No existe, lo creamos...");
                XDocument archivo = new XDocument(
                    new XElement("CONFIGURACION",
                        new XElement("HABILITAR", "0"),
                        new XElement("RUTA_ESCRITORIO", "C:\\Users\\xxxxx\\Desktop\\"),
                        new XElement("NOMBRE_ARCHIVO_ESCRITORIO", "hoy.xlsx"),
                        new XElement("EXTENSION_ARCHIVO", ".xlsx"),
                        new XElement("RUTA_PLANTILLA", "C:\\xxxxx\\BACKUP\\"),
                        new XElement("NOMBRE_ARCHIVO_PLANTILLA", "0_plantilla.xlsx")
                    )
                );

                archivo.Save("config.xml");
                logit.logThis("Archivo \"config.xml\" creado.");
            }
            else
            {
                logit.logThis("Archivo XML existe.");
                // Existe el archivo XML, lo leemos.
                XmlDocument archivo = new XmlDocument();
                archivo.Load("config.xml");

                String aux_temp =archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/HABILITAR").InnerText.ToString();
                int.TryParse(aux_temp, out ks);
                
                p_Desktop = archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/RUTA_ESCRITORIO").InnerText.ToString();
                f_Desktop = archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/NOMBRE_ARCHIVO_ESCRITORIO").InnerText.ToString();
                f_extension = archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/EXTENSION_ARCHIVO").InnerText.ToString();
                p_Plantilla = archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/RUTA_PLANTILLA").InnerText.ToString();
                f_Plantilla = archivo.DocumentElement.SelectSingleNode("/CONFIGURACION/NOMBRE_ARCHIVO_PLANTILLA").InnerText.ToString();

                logit.logThis("Archivo XML leido.");

                if (ks!=0) // Kill switch, si está a 0 no entra.
                {
                    try
                    {

                        // Si el check da correcto (0 sin errores), pasamos a copiar y tal.
                        if (checkValid.ok(fecha_hoy, p_Desktop, f_Desktop, f_extension, p_Plantilla, f_Plantilla) == 0)
                        {

                            //mi. ju. vi. sá. do. lu. ma.
                            // Comprobamos si es entre semana, si es sabado o domingo, fuera, si es lunes hay que restar 3 en lugar de uno para la fecha 
                            logit.logThis("¿Es fin de semana o día de semana?");
                            if (fecha_hoy.ToString("ddd") != "sá." && fecha_hoy.ToString("ddd") != "do.")
                            {

                                logit.logThis("Es día de semana.");

                                // Si es lunes, hay que restar 3 dias, no 1 solo.
                                if (fecha_hoy.ToString("ddd") == "lu.")
                                {
                                    dias = -3;
                                    logit.logThis("Es Lunes, hay que restar 3 días.");
                                }

                                logit.logThis("¿Existe el archivo en el escritorio?");
                                // Comprobamos si existe el archivo en el Escritorio.
                                if (File.Exists(p_Desktop + f_Desktop))
                                {
                                    logit.logThis(p_Desktop + f_Desktop + " --> Existe.");

                                    logit.logThis("¿Existe el archivo en la carpeta de la plantilla?");
                                    // Si existe lo movemos a la carpeta donde esta la plantilla y cambiamos el nombre
                                    if (!File.Exists(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension))
                                    {
                                        File.Move(p_Desktop + f_Desktop, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                                        logit.logThis("Movemos \"" + p_Desktop + f_Desktop + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");

                                        logit.logThis("¿Existe la carpeta del mes?");
                                        // Comprobamos si existe ya la carpeta final del mes y lo movemos, sino, pues creamos la carpeta
                                        if (Directory.Exists(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM")))
                                        {
                                            logit.logThis(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + " --> Existe.");

                                            logit.logThis("¿Existe el archivo en la carpeta del mes?");
                                            // Movemos
                                            if (!File.Exists(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension))
                                            {
                                                File.Move(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                                                logit.logThis("Movemos \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");
                                            }
                                            else { logit.logThis("ERROR, DUPLICADO. YA EXISTE UN ARCHIVO (" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + ") ESE NOMBRE EN LA CARPETA " + fecha_hoy.AddDays(dias).ToString("yyyy-MM")); }

                                        }
                                        else // No existe la carpeta la creamos y lo movemos
                                        {
                                            logit.logThis(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + " --> No Existe.");
                                            // Creamos la carpeta
                                            Directory.CreateDirectory(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM"));
                                            logit.logThis(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + " --> Creada carpeta.");

                                            // Movemos, no hace falta comprobar si existe el archivo, acabamos de crear la carpeta.
                                            File.Move(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                                            logit.logThis("Movemos \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");
                                        }
                                    }
                                    else { logit.logThis("ERROR, DUPLICADO. EXISTE UN ARCHIVO CON EL NOMBRE " + f_Plantilla + " EN LA CARPETA " + p_Plantilla + "."); }
                                }

                                // Creamos el fichero nuevo del escritorio
                                logit.logThis("¿Existe el archivo en el escritorio para pegarlo?");
                                if (!File.Exists(p_Desktop + f_Desktop))
                                {
                                    File.Copy(p_Plantilla + f_Plantilla, p_Desktop + f_Desktop);
                                    logit.logThis("Copiamos \"" + p_Plantilla + f_Plantilla + "\" a \"" + p_Desktop + f_Desktop + "\"");
                                }
                                else { logit.logThis("ERROR, DUPLICADO. EXISTE UN ARCHIVO EN EL ESCRITORIO."); }
                            }
                            else { logit.logThis("Es fin de semana."); }

                        }

                    }
                    catch (Exception ex) { logit.logThis(ex.Message); }
                }else { logit.logThis("El script está deshabilitado, por favor revisa el archivo de configuración."); }
            }

            logit.logThis("FIN DE LA EJECUCIÓN.\n");
        }
    }
}
