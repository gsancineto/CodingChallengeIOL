/*
 * Refactorear la clase para respetar principios de programación orientada a objetos. Qué pasa si debemos soportar un nuevo idioma para los reportes, o
 * agregar más formas geométricas?
 *
 * Se puede hacer cualquier cambio que se crea necesario tanto en el código como en los tests. La única condición es que los tests pasen OK.
 *
 * TODO: Implementar Trapecio/Rectangulo, agregar otro idioma a reporting.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingChallenge.Data.Classes
{
    public class FormaGeometrica
    {
        #region Formas

        public const int Cuadrado = 1;
        public const int Circulo = 2;
        public const int TrianguloEquilatero = 3;
        public const int Trapecio = 4;
        public const int Rectangulo = 5;

        public class Formas
        {
            public Formas(int tipo, int cantidad, decimal area, decimal perimetro)
            {
                Tipo = tipo;
                Cantidad = cantidad;
                Area = area;
                Perimetro = perimetro;
            }

            public int Tipo, Cantidad;
            public decimal Area, Perimetro;
        }

        #endregion

        #region Idiomas

        public const int Castellano = 1;
        public const int Ingles = 2;
        public const int Portugues = 3;

        #endregion

        private readonly decimal _base;
        private readonly decimal? _baseMenor;
        private readonly decimal? _altura;

        public int Tipo { get; set; }

        public FormaGeometrica(int tipo, decimal baseForma, decimal? baseMenor, decimal? altura)
        {
            Tipo = tipo;
            _base = baseForma;
            _baseMenor = baseMenor;
            _altura = altura;
        }

        public static string Imprimir(List<FormaGeometrica> formas, int idioma)
        {
            var sb = new StringBuilder();

            if (!formas.Any())
            {
                string[] emptyMessages = new string[] { "Lista vacía de formas!", "Empty list of shapes!", "Lista vazia de formas!" };
                sb.Append($"<h1>{emptyMessages[idioma - 1]}</h1>");
            }
            else
            {
                // Hay por lo menos una forma
                // HEADER
                string[] headers = new string[] { "Reporte de Formas", "Shapes report", "Relatório de formas" };
                sb.Append($"<h1>{headers[idioma - 1]}</h1>");

                List<Formas> formasList = new List<Formas> {
                    new Formas(tipo: Cuadrado, 0, 0m, 0m),
                    new Formas(tipo: Circulo, 0, 0m, 0m),
                    new Formas(tipo: TrianguloEquilatero, 0, 0m, 0m),
                    new Formas(tipo: Trapecio, 0, 0m, 0m),
                    new Formas(tipo: Rectangulo, 0, 0m, 0m)
                };

                int cantidadTotal = 0;
                decimal perimetroTotal = 0m;
                decimal areaTotal = 0m;

                for (var i = 0; i < formas.Count; i++)
                {
                    formasList[formas[i].Tipo - 1].Cantidad++;
                    formasList[formas[i].Tipo - 1].Area += formas[i].CalcularArea();
                    formasList[formas[i].Tipo - 1].Perimetro += formas[i].CalcularPerimetro();
                }

                foreach (var forma in formasList)
                {
                    cantidadTotal += forma.Cantidad;
                    perimetroTotal += forma.Perimetro;
                    areaTotal += forma.Area;

                    sb.Append(ObtenerLinea(forma, idioma));
                }


                // FOOTER
                sb.Append("TOTAL:<br/>");
                sb.Append(cantidadTotal + " " + (idioma == Ingles ? "shapes" : "formas") + " ");
                sb.Append((idioma == Ingles ? "Perimeter " : "Perimetro ") + (perimetroTotal).ToString("#.##") + " ");
                sb.Append("Area " + (areaTotal).ToString("#.##"));
            }

            return sb.ToString();
        }

        private static string ObtenerLinea(Formas forma, int idioma)
        {
            string linea = string.Empty;
            
            if (forma.Cantidad > 0)
            {
                string perimetro = idioma == 2 ? "Perimeter" : "Perimetro";
                linea += $"{forma.Cantidad} {TraducirForma(forma.Tipo, forma.Cantidad, idioma)} | Area {forma.Area:#.##} | {perimetro} {forma.Perimetro:#.##} <br/>";
            }

            return linea;
        }

        private static string TraducirForma(int tipo, int cantidad, int idioma)
        {
            string[,] formas = new string[5, 3] {
            {"Cuadrado", "Square", "Quadrado" },
            {"Círculo", "Circle", "Círculo" },
            {"Triángulo", "Triangle", "Triângulo" },
            {"Trapecio", "Trapezium", "Trapézio" },
            {"Rectángulo", "Rectangle", "Retângulo" }
            };

            string forma = formas[tipo - 1, idioma - 1];

            return cantidad > 1 ? $"{forma}s" : forma;
        }

        public decimal CalcularArea()
        {
            switch (Tipo)
            {
                case Cuadrado: return _base * _base;
                case Circulo: return (decimal)Math.PI * (_base / 2) * (_base / 2);
                case TrianguloEquilatero: return ((decimal)Math.Sqrt(3) / 4) * _base * _base;
                case Trapecio: return (decimal)(_altura * (_base + _baseMenor) /2);
                case Rectangulo: return (decimal)(_base * _altura);
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }

        public decimal CalcularPerimetro()
        {
            switch (Tipo)
            {
                case Cuadrado: return _base * 4;
                case Circulo: return (decimal)Math.PI * _base;
                case TrianguloEquilatero: return _base * 3;
                case Trapecio: return (decimal)(_base + _baseMenor + _altura * 2);
                case Rectangulo: return (decimal)(_base * 2 + _altura * 2);
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }
    }
}
