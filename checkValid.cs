using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OrganizarTiming
{
    public class checkValid
    {
        public static int ok(DateTime fecha_hoy, String p_Desktop, String f_Desktop, String f_extension, String p_Plantilla, String f_Plantilla)
        {
            int result = 0; // 0 Es ok para copiar, 1 no se puede copiar.
            int dias = -1;  // Dias a restar
            

            try
            {
                logit.logThis("INICIO DE LA COMPROBACION.");

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
                            //File.Move(p_Desktop + f_Desktop, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                            logit.logThis("Podemos mover \"" + p_Desktop + f_Desktop + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");

                            logit.logThis("¿Existe la carpeta del mes?");
                            // Comprobamos si existe ya la carpeta final del mes y lo movemos, sino, pues creamos la carpeta
                            if (Directory.Exists(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM")))
                            {
                                logit.logThis(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + " --> Existe.");

                                logit.logThis("¿Existe el archivo en la carpeta del mes?");
                                // Movemos
                                if (!File.Exists(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension))
                                {
                                    //File.Move(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                                    logit.logThis("Podemos mover \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");
                                }
                                else
                                {
                                    logit.logThis("ERROR, DUPLICADO. YA EXISTE UN ARCHIVO (" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + ") ESE NOMBRE EN LA CARPETA " + fecha_hoy.AddDays(dias).ToString("yyyy-MM"));
                                    result = 1;
                                }

                            }
                            else // No existe la carpeta la creamos y lo movemos
                            {
                                logit.logThis(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + " --> No Existe.");
                                // Creamos la carpeta
                                //Directory.CreateDirectory(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM"));
                                logit.logThis("Podemos crear la carpeta \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\"");

                                // Movemos, no hace falta comprobar si existe el archivo, acabamos de crear la carpeta.
                                //File.Move(p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension, p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension);
                                logit.logThis("Podemos mover \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\" a \"" + p_Plantilla + fecha_hoy.AddDays(dias).ToString("yyyy-MM") + "\\" + fecha_hoy.AddDays(dias).ToString("yyyy-MM-dd") + f_extension + "\"");
                            }
                        }
                        else
                        {
                            logit.logThis("ERROR, DUPLICADO. EXISTE UN ARCHIVO CON EL NOMBRE " + f_Plantilla + " EN LA CARPETA " + p_Plantilla + ".");
                            result = 1;
                        }
                    }

                    // Creamos el fichero nuevo del escritorio
                    // Exista o no exista el archivo del escritorio no podemos detener el proceso
                    /*logit.logThis("¿Existe el archivo en el escritorio para pegarlo?");
                    if (!File.Exists(p_Desktop + f_Desktop))
                    {
                        //File.Copy(p_Plantilla + f_Plantilla, p_Desktop + f_Desktop);
                        logit.logThis("Podemos copiar \"" + p_Plantilla + f_Plantilla + "\" a \"" + p_Desktop + f_Desktop + "\"");
                    }
                    else
                    {
                        logit.logThis("ERROR, DUPLICADO. EXISTE UN ARCHIVO EN EL ESCRITORIO.");
                        result = 1;
                    }*/
                }
                else
                {
                    logit.logThis("Es fin de semana.");
                    result = 1;
                }

                logit.logThis("FIN DE LA COMPROBACION.");

            }
            catch (Exception ex)
            {
                logit.logThis(ex.Message);
                return result = 1;
            }

            return result;
        }
    }
}
