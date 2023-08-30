using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronograma
{
    class Config
    {
        public const int filaTituloAsignatura = 2;
        public const int columnaTituloAsignatura = 2;
        public const int tamanyoTituloAsignatura = 24;


        public const int filaInicioMeses = 4;
        public const int columnaInicioMeses = 3;

        public const int tamanyoTituloMes = 16;
        public static XlRgbColor colorDiaSemana = XlRgbColor.rgbLightSteelBlue;

        public static XlRgbColor[] coloresUFs = new XlRgbColor[] { XlRgbColor.rgbLightBlue,
                                                                   XlRgbColor.rgbLightGreen,
                                                                   XlRgbColor.rgbLightCyan,
                                                                   XlRgbColor.rgbLightYellow,
                                                                   XlRgbColor.rgbLightCoral,
                                                                   XlRgbColor.rgbPaleVioletRed,
                                                                   XlRgbColor.rgbLightSalmon,
                                                                   XlRgbColor.rgbLightSkyBlue,
                                                                 };
        public static XlRgbColor colorFestivos = XlRgbColor.rgbLightPink;
        public static XlRgbColor colorFinesDeSemana = XlRgbColor.rgbLightGray;

        public const int filaInicioUFs = 5;
        public const int columnaInicioUFs = 14;
    }
}
