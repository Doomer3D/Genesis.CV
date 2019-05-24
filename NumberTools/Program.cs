using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Genesis.IO;

using Elect.CV.NumberUtils;

namespace Elect.CV
{
    internal sealed class Program
    {
        private readonly ConsoleWriter writer = new ConsoleWriter();

        /// <summary>
        /// протестировать конвертацию числа в текст и наоборот
        /// </summary>
        public void TestConvertAndBack()
        {
            Console.WindowWidth = 100;

            long start = 0;
            long count = 1000;

            var parser = new TextToNumberParser();
            for (long i = start; i < start + count; i++)
            {
                var str = RussianNumber.Str(i);
                var parsed = parser.Parse(str);

                writer.Write("Значение: ");
                writer.Write(ConsoleColor.White, str);
                writer.Write(" (");
                writer.Write(ConsoleColor.White, i.ToString());
                writer.Write("), распознанное: ");
                writer.Write(ConsoleColor.White, parsed.Value.ToString());
                if (parsed.Error > 0)
                {
                    writer.Write(", ошибка: ");
                    writer.Write(ConsoleColor.Red, parsed.Error.ToString("0.00", CultureInfo.InvariantCulture));
                }
                writer.Write("... ");
                if (parsed == i)
                {
                    writer.WriteLineSuccess();
                }
                else
                {
                    writer.WriteLineError();
                }
            }
        }

        public void TestSamples()
        {
            void Print(string s1, string s2)
            {
                var d = NumeralLevenshtein.CompareStrings(s1, s2, false);
                writer.Write($"Расстояние между токенами [{s1}] и [{s2}]: ");
                writer.WriteLine(ConsoleColor.Red, $"{d:0.000}");
            }

            Print("бдин", "один");
            Print("адин", "один");
            Print("д8а", "два");
            Print("Двадиать", "двадцать");
            Print("Тридпать", "тридцать");
        }

        static void Main()
        {
            var program = new Program();
            //program.TestConvertAndBack();
            program.TestSamples();
        }

        #region Stats

        private void Stats()
        {
            var list = new List<string>();

            for (int i = 0; i < 99999; i++)
            {
                list.Add(RussianNumber.Str(i).ToLowerInvariant());
            }

            // список символов
            var chars = list.SelectMany(e => e).Distinct().Where(e => char.IsLetter(e)).OrderBy(e => e).ToList();

            // список токенов
            var tokens = list.SelectMany(e => e.Split()).Distinct().OrderBy(e => (e.Length, e)).ToList();

            var validChars = new string(chars.ToArray());
            var invalidChars = new string("абвгдеёжзийклмнопрстуфхцчшщъыьэюя".Except(chars).ToArray());

            var tokensList = string.Join(Environment.NewLine, tokens.ToArray());
        }

        #endregion
    }
}
