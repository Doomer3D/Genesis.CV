using System;
using System.Text;

namespace Elect.CV.NumberUtils
{
    /// <summary>
    /// класс для перевода чисел в текст
    /// </summary>
    public sealed class RussianNumber
    {
        /// <summary>
        /// Наименования сотен
        /// </summary>
        private static readonly string[] hunds =
        {
            "", "сто ", "двести ", "триста ", "четыреста ",
            "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "
        };

        /// <summary>
        /// Наименования десятков
        /// </summary>
        private static readonly string[] tens =
        {
            "", "десять ", "двадцать ", "тридцать ", "сорок ", "пятьдесят ",
            "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "
        };

        /// <summary>
        /// выбрать правильное падежноге окончание существительного
        /// </summary>
        /// <param name="value"> число </param>
        /// <param name="form1"> форма существительного в единственном числе </param>
        /// <param name="form2"> форма существительного от двух до четырёх </param>
        /// <param name="form3"> форма существительного от пяти и больше </param>
        /// <returns> возвращает существительное с падежным окончанием, которое соответсвует числу </returns>
        private static string WordForm(long value, string form1, string form2, string form3)
        {
            var t = (value % 100 > 20) ? value % 10 : value % 20;

            switch (t)
            {
                case 1: return form1;
                case 2:
                case 3:
                case 4: return form2;
                default: return form3;
            }
        }

        /// <summary>
        /// перевести в строку числа с учётом падежного окончания относящегося к числу существительного
        /// </summary>
        /// <param name="value"> число </param>
        /// <param name="male"> род существительного, которое относится к числу </param>
        /// <param name="form1"> форма существительного в единственном числе </param>
        /// <param name="form2"> форма существительного от двух до четырёх </param>
        /// <param name="form3"> форма существительного от пяти и больше </param>
        /// <returns></returns>
        private static string Str(long value, bool male, string form1, string form2, string form3)
        {
            string[] frac20 =
            {
                "", "один ", "два ", "три ", "четыре ", "пять ", "шесть ",
                "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
                "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
                "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
            };

            var n = value % 1000;
            if (n == 0) return "";
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(value), "Параметр не может быть отрицательным");
            if (!male)
            {
                frac20[1] = "одна ";
                frac20[2] = "две ";
            }

            var sb = new StringBuilder(hunds[n / 100]);

            if (n % 100 < 20)
            {
                sb.Append(frac20[n % 100]);
            }
            else
            {
                sb.Append(tens[n % 100 / 10]);
                sb.Append(frac20[n % 10]);
            }

            sb.Append(WordForm(n, form1, form2, form3));

            if (sb.Length != 0) sb.Append(" ");
            return sb.ToString();
        }

        /// <summary>
        /// перевести целое число в строку
        /// </summary>
        /// <param name="value"> число </param>
        /// <returns> возвращает строковую запись числа </returns>
        public static string Str(long value)
        {
            bool minus = false;
            if (value < 0) { value = -value; minus = true; }

            var n = value;

            var sb = new StringBuilder();

            if (n == 0) sb.Append("ноль ");
            if (n % 1000 != 0)
            {
                sb.Append(Str(n, true, "", "", ""));
            }

            n /= 1000;

            sb.Insert(0, Str(n, false, "тысяча", "тысячи", "тысяч"));
            n /= 1000;

            sb.Insert(0, Str(n, true, "миллион", "миллиона", "миллионов"));
            n /= 1000;

            sb.Insert(0, Str(n, true, "миллиард", "миллиарда", "миллиардов"));
            n /= 1000;

            sb.Insert(0, Str(n, true, "триллион", "триллиона", "триллионов"));
            n /= 1000;

            sb.Insert(0, Str(n, true, "триллиард", "триллиарда", "триллиардов"));
            if (minus) sb.Insert(0, "минус ");

            // делаем первую букву заглавной
            sb[0] = char.ToUpper(sb[0]);

            return sb.ToString().Trim();
        }
    }
}
