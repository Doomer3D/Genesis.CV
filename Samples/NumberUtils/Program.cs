using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Genesis.IO;
using Genesis.CV.NumberUtils;

namespace NumberUtils
{
    internal sealed class Program
    {
        private readonly ConsoleWriter writer = new ConsoleWriter();

        /// <summary>
        /// протестировать конвертацию числа в текст и наоборот
        /// </summary>
        public void TestConvertAndBack()
        {
            long start = 0L;
            long count = 50;

            for (long i = start; i < start + count; i++)
            {
                var str = RussianNumber.ToString(i);

                ParseNumber(str, i);
            }
        }

        /// <summary>
        /// протестировать на основе примеров из файла samples.txt
        /// </summary>
        public void TestSamples()
        {
            bool first = true;
            foreach (var line in File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "samples.txt"), System.Text.Encoding.UTF8))
            {
                var str = line.Trim();
                if (str.Length != 0)
                {
                    if (str.StartsWith("#"))
                    {
                        // комментарий
                        if (first) first = false; else writer.WriteLine();
                        writer.WriteLine("┌──────────────────────────────────────────────────────────────┐");
                        writer.WriteLine($"│ {str.Substring(1).TrimStart(),-60} │");
                        writer.WriteLine("└──────────────────────────────────────────────────────────────┘");
                        writer.WriteLine();
                    }
                    else
                    {
                        var data = str.Split(new char[] { ' ', '\t' }, 2);
                        if (int.TryParse(data[0], out var number))
                        {
                            ParseNumber(data[1].TrimStart(), number);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// распарсить строку и сравнить ее с числом
        /// </summary>
        /// <param name="str"> число прописью </param>
        /// <param name="number"> число-образец </param>
        private void ParseNumber(string str, long number)
        {
            var parsed = RussianNumber.Parse(str);
            bool good = parsed == number;

            writer.Write("Значение: ");
            writer.Write(ConsoleColor.White, str);
            writer.Write(" (");
            writer.Write(ConsoleColor.White, number.ToString());
            writer.Write("), распознанное: ");
            writer.Write(ConsoleColor.White, parsed.Value.ToString());
            if (parsed.Error > 0)
            {
                writer.Write(", ошибка: ");
                writer.Write(good ? ConsoleColor.DarkRed : ConsoleColor.Red, parsed.Error.ToString("0.000", CultureInfo.InvariantCulture));
            }
            writer.Write("... ");
            if (good)
            {
                writer.WriteLineSuccess();
            }
            else
            {
                writer.WriteLineError();
            }
        }

        /// <summary>
        /// протестировать расстояние Левенштейна
        /// </summary>
        public void TestLevenshtein()
        {
            double[,] D = new double[2, 100];

            void Print(string s1, string s2)
            {
                var d = NumeralLevenshtein.CompareStrings(s1, s2, ref D, false);
                writer.Write($"Расстояние между токенами [{s1}] и [{s2}]: ");
                writer.WriteLine(ConsoleColor.Red, $"{d:0.000}");
            }

            Print("а", "б");
            Print("б", "а");
            Print("", "а");
            Print("а", "");
            Print("", "б");
            Print("б", "");
            Print("а", "а");
            Print("ааа", "ввв");
            Print("бдин", "один");
            Print("адин", "один");
            Print("д8а", "два");
            Print("Одина", "одна");
            Print("Двадиать", "двадцать");
            Print("Тридпать", "тридцать");
        }

        static void Main()
        {
            Console.WindowWidth = Console.BufferWidth = Math.Min(Console.LargestWindowWidth, 100);
            Console.WindowHeight = Console.BufferHeight = Math.Min(Console.LargestWindowHeight, 80);

            var program = new Program();

            //// конвертация в текст и обратно
            //program.TestConvertAndBack();

            // прогон примеров из файла samplex.txt
            program.TestSamples();
        }

        #region Stats

        /// <summary>
        /// функция формирования статистики по используемым символам и токенам
        /// </summary>
        private void Stats()
        {
            var list = new List<string>();

            for (int i = 0; i < 99999; i++)
            {
                list.Add(RussianNumber.ToString(i).ToLowerInvariant());
            }

            // список символов
            var chars = list.SelectMany(e => e).Distinct().Where(e => char.IsLetter(e)).OrderBy(e => e).ToArray();

            // список токенов
            var tokens = list.SelectMany(e => e.Split()).Distinct().OrderBy(e => (e.Length, e)).ToArray();

            // используемые символы
            var validChars = new string(chars);

            // неиспользуемые символы
            var invalidChars = new string("абвгдеёжзийклмнопрстуфхцчшщъыьэюя".Except(chars).ToArray());

            // список токенов текстом
            var tokensList = string.Join(Environment.NewLine, tokens);
        }

        #endregion
    }
}
