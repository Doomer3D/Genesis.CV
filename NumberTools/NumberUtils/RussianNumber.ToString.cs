using System;
using System.Text;

namespace Genesis.CV.NumberUtils
{
    public static partial class RussianNumber
    {
        /// <summary>
        /// наименования односложных числительных (мужской род)
        /// </summary>
        private static readonly string[] _simple_units_male =
        {
            "", "один ", "два ", "три ", "четыре ", "пять ", "шесть ",
            "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
            "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
            "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
        };

        /// <summary>
        /// наименования односложных числительных (женский род)
        /// </summary>
        private static readonly string[] _simple_units_female =
        {
            "", "одна ", "две ", "три ", "четыре ", "пять ", "шесть ",
            "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
            "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
            "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
        };

        /// <summary>
        /// наименования десятков
        /// </summary>
        private static readonly string[] _ten_units =
        {
            "", "десять ", "двадцать ", "тридцать ", "сорок ", "пятьдесят ",
            "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "
        };

        /// <summary>
        /// наименования сотен
        /// </summary>
        private static readonly string[] _hundred_units =
        {
            "", "сто ", "двести ", "триста ", "четыреста ",
            "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "
        };

        /// <summary>
        /// выбрать правильное падежное окончание существительного
        /// </summary>
        /// <param name="value"> число </param>
        /// <param name="form1"> форма существительного в единственном числе </param>
        /// <param name="form2"> форма существительного от двух до четырёх </param>
        /// <param name="form3"> форма существительного от пяти и больше </param>
        /// <returns> возвращает существительное с падежным окончанием, которое соответсвует числу </returns>
        private static string WordForm(long value, string form1, string form2, string form3)
        {
            switch ((value % 100 > 20) ? value % 10 : value % 20)
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
        private static string ToString(long value, bool male, string form1, string form2, string form3)
        {
            var n = value % 1000;

            if (n == 0) return "";
            else if (n < 0) throw new ArgumentOutOfRangeException(nameof(value), "Параметр не может быть отрицательным");

            // выбираем единицы в зависимости от рода
            var units = male ? _simple_units_male : _simple_units_female;

            var sb = new StringBuilder(_hundred_units[n / 100]);

            var rem100 = n % 100;
            if (rem100 < 20)
            {
                sb.Append(units[rem100]);
            }
            else
            {
                sb.Append(_ten_units[rem100 / 10]);
                sb.Append(units[n % 10]);
            }

            sb.Append(WordForm(n, form1, form2, form3));

            if (sb.Length != 0) sb.Append(" ");
            return sb.ToString();
        }

        /// <summary>
        /// перевести целое число в строку
        /// </summary>
        /// <param name="value"> число </param>
        /// <param name="capitalize"> указывает, что необходимо сделать первую букву заглавной </param>
        /// <returns> возвращает строковую запись числа </returns>
        public static string ToString(long value, bool capitalize = true)
        {
            bool minus = value < 0;
            if (minus) value = -value;

            var n = value;

            var sb = new StringBuilder();

            if (n == 0) sb.Append("ноль ");
            if (n % 1000 != 0)
            {
                sb.Append(ToString(n, true, "", "", ""));
            }

            n /= 1000;

            sb.Insert(0, ToString(n, false, "тысяча", "тысячи", "тысяч"));
            n /= 1000;

            sb.Insert(0, ToString(n, true, "миллион", "миллиона", "миллионов"));
            n /= 1000;

            sb.Insert(0, ToString(n, true, "миллиард", "миллиарда", "миллиардов"));
            n /= 1000;

            sb.Insert(0, ToString(n, true, "триллион", "триллиона", "триллионов"));
            n /= 1000;

            sb.Insert(0, ToString(n, true, "квадриллион", "квадриллиона", "квадриллионов"));
            if (minus) sb.Insert(0, "минус ");

            // делаем первую букву заглавной
            if (capitalize) sb[0] = char.ToUpperInvariant(sb[0]);

            return sb.ToString().Trim();
        }
    }
}
