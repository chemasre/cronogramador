using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cronograma
{
    class Program
    {
        class Asignatura
        {
            Dictionary<DayOfWeek, int> horasPorDiaSemana;
            Dictionary<int, int> horasPorUF;

            public Asignatura(int[] _horasPorDiaSemana, int[] _horasPorUF)
            {
                horasPorDiaSemana = new Dictionary<DayOfWeek, int>();
                horasPorUF = new Dictionary<int, int>();

                for (int i = 0; i < _horasPorDiaSemana.Length; i++) { horasPorDiaSemana.Add(DayOfWeek.Monday + i, _horasPorDiaSemana[i]); }
                for (int i = 0; i < _horasPorUF.Length; i++) { horasPorUF.Add(i, _horasPorUF[i]); }

            }

            public void ImprimeCronograma(DateTime[] _festivos, DateTime _diaInicio, bool _empezarUFsEnDiaNuevo)
            {
                FileStream file = new FileStream("cronograma.csv", FileMode.Create);
                StreamWriter writer = new StreamWriter(file, Encoding.UTF8);

                HashSet<DateTime> festivos = new HashSet<DateTime>();

                for (int i = 0; i < _festivos.Length; i++) { festivos.Add(_festivos[i]); }

                int totalUFs = horasPorUF.Count;

                DateTime dia = _diaInicio;

                int uF = 0;
                int horasNoAsignadasUF = horasPorUF[0];

                while(uF < totalUFs)
                {
                    int horasDia;


                    writer.Write(String.Format("{0},{1:00}/{2:00}/{3:0000}", TraduceDiaSemana(dia.DayOfWeek), dia.Month, dia.Day, dia.Year)); ;

                    if (dia.DayOfWeek == DayOfWeek.Saturday || dia.DayOfWeek == DayOfWeek.Sunday)
                    {
                        writer.Write(",FINDE");
                        horasDia = 0;
                    }
                    else if(festivos.Contains(dia))
                    {
                        writer.Write(",FESTIVO");
                        horasDia = 0;
                    }
                    else
                    {
                        writer.Write(",LECTIVO");
                        horasDia = horasPorDiaSemana[dia.DayOfWeek];
                    }

                    while (horasDia > 0 && uF < totalUFs)
                    {
                        Console.WriteLine("Dia " + TraduceDiaSemana(dia.DayOfWeek) + " UF " + (uF + 1) + " horasDia " + horasDia + " horasNoAsignadasUF " + horasNoAsignadasUF);

                        bool avanzarUF = false;

                        if (horasNoAsignadasUF < horasDia)
                        {
                            writer.Write(",UF" + (uF + 1) + " (" + horasNoAsignadasUF + " h)");

                            if(_empezarUFsEnDiaNuevo)
                            {
                                horasDia = 0;
                            }
                            else
                            {
                                horasDia -= horasNoAsignadasUF;
                            }

                            horasNoAsignadasUF = 0;

                            avanzarUF = true;

                        }
                        else
                        {
                            writer.Write(",UF" + (uF + 1) + " (" + horasDia + " h)");
                            horasNoAsignadasUF -= horasDia;
                            horasDia = 0;

                            if(horasNoAsignadasUF == 0)
                            {
                                avanzarUF = true;
                            }
                        }

                        if(avanzarUF)
                        {
                            uF++;
                            if (uF < totalUFs)
                            {
                                horasNoAsignadasUF = horasPorUF[uF];
                            }

                        }

                    }

                    writer.WriteLine();

                    dia = dia.AddDays(1);

                        

                }

                writer.Close();

            }

            string TraduceDiaSemana(DayOfWeek d)
            {
                if (d == DayOfWeek.Monday) { return "Lunes"; }
                else if (d == DayOfWeek.Tuesday) { return "Martes"; }
                else if (d == DayOfWeek.Wednesday) { return "Miércoles"; }
                else if (d == DayOfWeek.Thursday) { return "Jueves"; }
                else if (d == DayOfWeek.Friday) { return "Viernes"; }
                else if (d == DayOfWeek.Saturday) { return "Sábado"; }
                else // d == DayOfWeek.Sunday
                { return "Domingo"; }
            }

        };



        static void Main(string[] args)
        {
            DateTime[] festivos = new DateTime[] {
                                                    new DateTime(2022,  9, 26),
                                                    new DateTime(2022, 10, 12),
                                                    new DateTime(2022, 10, 31),
                                                    new DateTime(2022, 11,  1),
                                                    new DateTime(2022, 12,  6),
                                                    new DateTime(2022, 12,  8),
                                                    new DateTime(2022, 12, 22),
                                                    new DateTime(2022, 12, 23),
                                                    new DateTime(2022, 12, 24),
                                                    new DateTime(2022, 12, 25),
                                                    new DateTime(2022, 12, 26),
                                                    new DateTime(2022, 12, 27),
                                                    new DateTime(2022, 12, 28),
                                                    new DateTime(2022, 12, 29),
                                                    new DateTime(2022, 12, 30),
                                                    new DateTime(2022, 12, 31),
                                                    new DateTime(2023,  1,  1),
                                                    new DateTime(2023,  1,  2),
                                                    new DateTime(2023,  1,  3),
                                                    new DateTime(2023,  1,  4),
                                                    new DateTime(2023,  1,  5),
                                                    new DateTime(2023,  1,  6),
                                                    new DateTime(2023,  1,  7),
                                                    new DateTime(2023,  1,  8),
                                                    new DateTime(2023,  2, 20),
                                                    new DateTime(2023,  4,  3),
                                                    new DateTime(2023,  4,  4),
                                                    new DateTime(2023,  4,  5),
                                                    new DateTime(2023,  4,  6),
                                                    new DateTime(2023,  4,  7),
                                                    new DateTime(2023,  4,  8),
                                                    new DateTime(2023,  4,  9),
                                                    new DateTime(2023,  4, 10),
                                                    new DateTime(2023,  5,  1),
                                                    new DateTime(2023,  6,  2),
                                                    new DateTime(2023,  6,  5)

                                                 };
            
            DateTime diaInicio = new DateTime(2022, 9, 12);

            int[] horasPorUF = new int[] { 17, 17, 17, 17 };

            int[] horasPorDiaSemana = new int[] { 0, 0, 0, 0, 2 };

            Asignatura asignatura = new Asignatura(horasPorDiaSemana, horasPorUF);

            asignatura.ImprimeCronograma(festivos, diaInicio, true);
        }
    }
}