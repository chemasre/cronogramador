using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Cronograma
{
    class Program
    {
        const string nombreFicheroCalendario = "Calendario.json";
        const string nombreFicheroAsignatura = "Asignatura.json";
        const string nombreFicheroCronograma = "Cronograma.xlsx";

        static Calendario calendario;
        static Asignatura asignatura;


        static void MuestraDespedida()
        {
            Console.WriteLine("**********************************");
            Console.WriteLine("*                                *");
            Console.WriteLine("*  Gracias por usar el generador *");
            Console.WriteLine("*  de cronogramas.               *");
            Console.WriteLine("*  Si te ha hecho ganar tiempo   *");
            Console.WriteLine("*  invítame a un kofi ;)         *");
            Console.WriteLine("*                                *");
            Console.WriteLine("*  https://ko-fi.com/chemasolis  *");
            Console.WriteLine("*                                *");
            Console.WriteLine("**********************************");

        }

        static void MuestraMenuPrincipal()
        {
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|    GENERADOR DE CRONOGRAMAS    |");
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|                                |");
            
            calendario.Imprime(false);

            Console.WriteLine("|                                |");

            asignatura.Imprime(false);

            Console.WriteLine("|                                |");
            Console.WriteLine("| 1.- Modificar calendario       |");
            Console.WriteLine("| 2.- Modificar asignatura       |");
            Console.WriteLine("| 3.- Generar cronograma         |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 0.- Salir                      |");
            Console.WriteLine("+--------------------------------+");
        }

        static void MuestraMenuCalendario()
        {
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|       MODIFICAR CALENDARIO     |");
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|                                |");

            calendario.Imprime(true);

            Console.WriteLine("|                                |");
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("| 1.- Nuevo calendario           |");
            Console.WriteLine("| 2.- Anyadir festivo            |");
            Console.WriteLine("| 3.- Eliminar festivo           |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 0.- Volver                     |");
            Console.WriteLine("+--------------------------------+");
        }

        static void MuestraMenuAsignatura()
        {
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|       MODIFICAR ASIGNATURA     |");
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|                                |");

            asignatura.Imprime(true);

            Console.WriteLine("|                                |");
            Console.WriteLine("| 1.- Nueva asignatura           |");
            Console.WriteLine("| 2.- Anyadir UF                 |");
            Console.WriteLine("| 3.- Eliminar UF                |");
            Console.WriteLine("| 4.- Anyadir dia semana         |");
            Console.WriteLine("| 5.- Eliminar dia semana        |");
            Console.WriteLine("|                                |");
            Console.WriteLine("| 0.- Volver                     |");
            Console.WriteLine("+--------------------------------+");
        }

        static int LeeOpcion()
        {
            int resultado = 0;
            bool leido = false;

            while (!leido)
            {
                Console.Write(">");

                try { resultado = Int32.Parse(Console.ReadLine()); leido = true; }
                catch(Exception e) { Utils.MuestraError("Tienes que escribir un numero"); }
            }

            return resultado;
        }

        static DayOfWeek LeeDiaSemana()
        {
            int resultado = 0;
            bool leido = false;

            while (!leido)
            {
                Console.Write("[1-5]>");

                try
                {
                    resultado = Int32.Parse(Console.ReadLine());

                    if(resultado < 1 || resultado > 5) { Utils.MuestraError("Tienes que escribir un numero entre 1 (Lunes) y 5 (Viernes)"); }
                    else { leido = true; }
                }
                catch (Exception e) { Utils.MuestraError("Tienes que escribir un numero"); }
            }

            return (DayOfWeek)resultado;
        }

        static int LeeNatural()
        {
            int resultado = 0;
            bool leido = false;

            while (!leido)
            {
                Console.Write(">");

                try
                {
                    resultado = Int32.Parse(Console.ReadLine());
                    
                    if(resultado <= 0) { Utils.MuestraError("El numero tiene que ser mayor que cero"); }
                    else { leido = true; }


                }
                catch (Exception e) { Utils.MuestraError("Tienes que escribir un numero"); }
            }

            return resultado;
        }

        static bool LeeBooleano()
        {
            bool resultado = false;
            bool leido = false;


            while(!leido)
            {
                Console.Write("[s-n]>");

                try
                {
                    string s = Console.ReadLine();
                    if(s.ToUpper()[0] == 'S') { resultado = true; }
                
                    leido = true;
                }
                catch (Exception e)
                {
                    Utils.MuestraError("Tienes que escribir s o n");
                }
            }

            return resultado;

        }

        static List<DateTime> LeeDias()
        {
            var resultado = new List<DateTime>();
            bool leido = false;

            while (!leido)
            {
                Console.Write("[dd/mm/aaaa-dd/mm/aaaa]>");

                try
                {

                    string s = Console.ReadLine();
                    string[] partes = s.Split('-');
                    string[] partesInicio = partes[0].Split('/');
                    string[] partesFin = partes[1].Split('/');

                    int d = Int32.Parse(partesInicio[0]);
                    int m = Int32.Parse(partesInicio[1]);
                    int a = Int32.Parse(partesInicio[2]);

                    DateTime diaInicio = new DateTime(a, m, d);

                    d = Int32.Parse(partesFin[0]);
                    m = Int32.Parse(partesFin[1]);
                    a = Int32.Parse(partesFin[2]);

                    DateTime diaFin = new DateTime(a, m, d);

                    if(diaFin < diaInicio) { throw new Exception(); }

                    for(DateTime dia = diaInicio; dia <= diaFin; dia = dia.AddDays(1))
                    {
                        resultado.Add(dia);
                    }

                    leido = true;
                }
                catch (Exception e) { Utils.MuestraError("Tienes que escribir dos fechas con formato dd/mm/aaaa separadas pon un guion, por ejemplo 25/11/2023-29/11/2024"); }
            }

            return resultado;

        }

        static DateTime LeeDia()
        {
            var resultado = new DateTime();
            bool leido = false;

            while(!leido)
            {
                Console.Write("[dd/mm/aaaa]>");

                try
                {

                    string s = Console.ReadLine();
                    string[] partes = s.Split('/');

                    int diaActual = Int32.Parse(partes[0]);
                    int mes = Int32.Parse(partes[1]);
                    int anyo = Int32.Parse(partes[2]);

                    resultado = new DateTime(anyo, mes, diaActual);

                    leido = true;
                }
                catch (Exception e) { Utils.MuestraError("Tienes que escribir una fecha con formato dd/mm/aaaa, por ejemplo 25/11/2023"); }
            }

            return resultado;
        }

        static string LeeNombre()
        {
            string resultado = "";
            bool leido = false;


            while (!leido)
            {
                Console.Write(">");

                resultado = Console.ReadLine().Trim();
                    
                if(resultado.Length <= 0) { Utils.MuestraError("El nombre no puede estar vacio");  }
                else { leido = true; }
            }

            return resultado;
        }

        static void Main(string[] args)
        {
            bool fin = false;
            int opcion = -1;
            int menu = 1;

            calendario = new Calendario();
            asignatura = new Asignatura();

            if (File.Exists(nombreFicheroCalendario)) { calendario.Carga(nombreFicheroCalendario); }
            else { calendario.Guarda(nombreFicheroCalendario); }
            if (File.Exists(nombreFicheroAsignatura)) { asignatura.Carga(nombreFicheroAsignatura); }
            else { asignatura.Guarda(nombreFicheroAsignatura); }


            while (!fin)
            {
                Console.Clear();

                if (menu == 1) { MuestraMenuPrincipal(); }
                else if (menu == 2) { MuestraMenuCalendario(); }
                else if (menu == 3) { MuestraMenuAsignatura(); }

                opcion = LeeOpcion();

                if (menu == 1)
                {
                    if (opcion == 1) { menu = 2; }
                    else if (opcion == 2) { menu = 3; }
                    else if (opcion == 3)
                    {
                        bool calendarioCorrecto = calendario.CompruebaCorrecto();
                        bool asignaturaCorrecta = asignatura.CompruebaCorrecta();

                        if(!calendarioCorrecto || !asignaturaCorrecta)
                        {
                            Utils.MuestraError("No se puede generar el cronograma porque existen errores");
                        }
                        else
                        {
                            bool empezarEnDiaNuevo;
                            bool estiloContinuo;
                            Console.WriteLine("¿Prefieres que las UFs siempre empiecen en dia nuevo?");
                            empezarEnDiaNuevo = LeeBooleano();
                            Console.WriteLine("¿Quieres mostrar las UFs como una serie continua?");
                            Console.WriteLine(" -> Si eliges no, se marcarán sólo los días de la semana en que hay clase de la asignatura");
                            estiloContinuo = LeeBooleano();

                            var cronograma = new Cronograma(calendario, asignatura, empezarEnDiaNuevo, estiloContinuo);

                            bool generar = true;

                            if (!cronograma.CompruebaCorrecto()) { Console.WriteLine("Se han encontrado errores en el cronograma ¿Quieres generarlo igualmente?"); generar = LeeBooleano(); }

                            if(generar)
                            {
                                try
                                {
                                    cronograma.GeneraExcel(nombreFicheroCronograma);

                                    Utils.MuestraMensaje("Generado archivo " + nombreFicheroCronograma);
                                }
                                catch(Exception e)
                                {
                                    Utils.MuestraError("Fallo al generar el archivo excel: " + e.Message);
                                }
                            }

                        }
                    }
                    else if (opcion == 0)
                    {
                        Console.WriteLine("¿De verdad quieres salir?");

                        if(LeeBooleano())
                        {
                            fin = true;
                        }
                    }
                }
                else if (menu == 2)
                {
                    if(opcion == 1)
                    {
                        Console.WriteLine("Esto borrara el calendario del curso junto con los festivos que hayas introducido");
                        Console.WriteLine("¿Estas seguro/a?");
                        if(LeeBooleano())
                        {
                            var nuevoCalendario = new Calendario();

                            Console.WriteLine("¿En que dia empieza el curso?");
                            nuevoCalendario.PonDiaInicio(LeeDia());
                            Console.WriteLine("¿En que dia finaliza el curso?");
                            nuevoCalendario.PonDiaFin(LeeDia());

                            if(nuevoCalendario.CompruebaCorrecto())
                            {
                                calendario = nuevoCalendario;
                                calendario.Guarda(nombreFicheroCalendario);
                                Utils.MuestraMensaje("Se ha creado un nuevo calendario");
                            }
                            else
                            {
                                Utils.MuestraError("No se ha borrado el calendario porque se han encontrado errores");
                            }
                        }
                    }
                    else if(opcion == 2)
                    {
                        bool diaUnico;
                        var nuevoCalendario = calendario.Clonar();

                        Console.WriteLine("¿Quieres introducir varios días festivos consecutivos?");
                        if(LeeBooleano())
                        {
                            Console.WriteLine("Introduce el primer y el último día festivo");

                            List<DateTime> dias = LeeDias();
                            foreach (DateTime dia in dias) { nuevoCalendario.AnyadeFestivo(dia); }

                            diaUnico = false;
                        }
                        else
                        {
                            Console.WriteLine("Introduce el nuevo dia festivo");
                            nuevoCalendario.AnyadeFestivo(LeeDia());

                            diaUnico = true;
                        }

                        if (nuevoCalendario.CompruebaCorrecto())
                        {
                            calendario = nuevoCalendario;
                            calendario.Guarda(nombreFicheroCalendario);
                            Utils.MuestraMensaje(diaUnico ? "El festivo se ha anyadido al calendario":"Los festivos se han añadido al calendario");
                        }
                        else
                        {
                            Utils.MuestraError(diaUnico ? "No se ha anyadido el festivo porque se han encontrado errores" : "No se han anyadido los festivos porque se han encontrado errores");
                        }
                    }
                    else if(opcion == 3)
                    {
                        Console.WriteLine("¿Quieres eliminar varios días festivos consecutivos?");
                        if (LeeBooleano())
                        {
                            Console.WriteLine("Introduce el primer y el último día festivo que quieres eliminar");

                            List<DateTime> dias = LeeDias();

                            foreach (DateTime dia in dias) { if(calendario.EsFestivo(dia)) { calendario.EliminaFestivo(dia); } }
                            calendario.Guarda(nombreFicheroCalendario);

                            Utils.MuestraMensaje("Los festivos se han eliminado");
                        }
                        else
                        {
                            Console.WriteLine("Introduce el dia festivo que quieres eliminar");

                            DateTime dia = LeeDia();

                            if(calendario.EsFestivo(dia))
                            {
                                calendario.EliminaFestivo(dia);
                                calendario.Guarda(nombreFicheroCalendario);
                            }

                            Utils.MuestraMensaje("El festivo se ha eliminado");

                        }

                        
                    }
                    else if (opcion == 0) { menu = 1; }
                }
                else if (menu == 3)
                {
                    if (opcion == 1)
                    {
                        Console.WriteLine("Esto borrara la asignatura junto con las UFs y las horas semanales que hayas introducido");
                        Console.WriteLine("¿Estas seguro/a?");
                        if (LeeBooleano())
                        {
                            var nuevaAsignatura = new Asignatura();

                            Console.WriteLine("¿Cual es el nombre de la asignatura?");
                            nuevaAsignatura.PonNombre(LeeNombre());

                            asignatura = nuevaAsignatura;
                            asignatura.Guarda(nombreFicheroAsignatura);
                            Utils.MuestraMensaje("Se ha creado una nueva asignatura");
                        }
                    }
                    else if (opcion == 2)
                    {
                        int uf;
                        int horas;

                        Console.WriteLine("¿Cual es el numero de esta UF?");
                        uf = LeeNatural();

                        if(asignatura.TieneUF(uf)) { Utils.MuestraError("No se ha anyadido la UF porque ya existia"); }
                        else
                        {
                            Console.WriteLine("¿Cuantas horas tiene la UF?");
                            horas = LeeNatural();
                        
                            asignatura.AnyadeUF(uf, horas);
                            asignatura.Guarda(nombreFicheroAsignatura);

                            Utils.MuestraMensaje("Se ha anyadido la UF a la asignatura");
                        }


                    }
                    else if (opcion == 3)
                    {
                        Console.WriteLine("Introduce la UF que quieres eliminar");
                        int uf = LeeNatural();

                        if (asignatura.TieneUF(uf))
                        {
                            asignatura.EliminaUF(uf);
                            asignatura.Guarda(nombreFicheroAsignatura);
                            Utils.MuestraMensaje("La UF se ha eliminado");
                        }
                        else { Utils.MuestraError("No se ha eliminado la UF porque no existia"); }


                    }
                    else if (opcion == 4)
                    {
                        DayOfWeek diaSemana;
                        int horas;

                        Console.WriteLine("¿Que dia de la semana quieres anyadir?");
                        diaSemana = LeeDiaSemana();

                        if (asignatura.TieneDiaSemana(diaSemana)) { Utils.MuestraError("No se ha anyadido el dia de la semana porque ya existia"); }
                        else
                        {
                            Console.WriteLine("¿Cuantas horas se daran en este dia?");
                            horas = LeeNatural();

                            asignatura.AnyadeDiaSemana(diaSemana, horas);
                            asignatura.Guarda(nombreFicheroAsignatura);

                            Utils.MuestraMensaje("Se ha anyadido el dia de la semana a la asignatura");
                        }


                    }
                    else if (opcion == 5)
                    {
                        Console.WriteLine("¿Que dia de la semana quieres eliminar?");
                        DayOfWeek diaSemana = LeeDiaSemana();

                        if (asignatura.TieneDiaSemana(diaSemana))
                        {
                            asignatura.EliminaDiaSemana(diaSemana);
                            asignatura.Guarda(nombreFicheroAsignatura);
                            Utils.MuestraMensaje("El dia de la semana se ha eliminado");
                        }
                        else { Utils.MuestraError("No se ha eliminado el dia de la semana porque no existia"); }


                    }
                    else if (opcion == 0) { menu = 1; }
                }
            }

            Console.Clear();
            MuestraDespedida();

            Console.WriteLine("Pulsa intro para salir del programa");
            Console.Write(">");
            Console.ReadLine();

        }



    }
}