using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Genesis.CV.NumberUtils
{
    /// <summary>
    /// реализация расстояния Левенштейна для числительных
    /// </summary>
    public static class NumeralLevenshtein
    {
        /// <summary>
        /// таблица стоимости вставки
        /// </summary>
        private static readonly Dictionary<char, double> _insert;

        /// <summary>
        /// таблица стоимости удаления
        /// </summary>
        private static readonly Dictionary<char, double> _delete;

        /// <summary>
        /// таблица стоимости замены
        /// </summary>
        private static readonly Dictionary<(char a, char b), double> _update;

        /// <summary>
        /// стоимость вставки нетабличного символа
        /// </summary>
        private static readonly double _insertNonTableChar;

        /// <summary>
        /// стоимость удаления нетабличного символа
        /// </summary>
        private static readonly double _deleteNonTableChar;

        /// <summary>
        /// стоимость замены нетабличного символа
        /// </summary>
        private static readonly double _updateNonTableChar;

        /// <summary>
        /// определить степень похожести строк (расстояние Левенштейна)
        /// </summary>
        /// <param name="s1"> строка 1 </param>
        /// <param name="s2"> строка 2 </param>
        /// <param name="relative"> указывает, что необходимо считать относительную похожесть </param>
        /// <returns></returns>
        public static double CompareStrings(string s1, string s2, bool relative = true)
        {
            s1 = s1.ToLowerInvariant();
            s2 = s2.ToLowerInvariant();

            int m = s1.Length, n = s2.Length;

            // вспомогательные переменные
            int i, j, a = 1, b = 0;
            char c1, c2;
            double[,] D = new double[2, n + 1];
            double costInsert, costDelete, costUpdate;

            for (i = 0; i <= m; i++)
            {
                for (j = 0; j <= n; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        if (i == 0)
                        {
                            // считаем стоимость вставки
                            if (!_insert.TryGetValue(s2[j - 1], out costInsert)) costInsert = _insertNonTableChar;
                            D[1, j] = D[1, j - 1] + costInsert;
                        }
                        else if (j == 0)
                        {
                            // считаем стоимость удаления
                            if (!_delete.TryGetValue(s1[i - 1], out costDelete)) costDelete = _deleteNonTableChar;
                            D[a, 0] = D[b, 0] + costDelete;
                        }
                        else
                        {
                            c1 = s1[i - 1];
                            c2 = s2[j - 1];
                            if (c1 != c2)
                            {
                                // считаем стоимость удаления
                                if (!_delete.TryGetValue(c1, out costDelete)) costDelete = _deleteNonTableChar;

                                // считаем стоимость вставки
                                if (!_insert.TryGetValue(c2, out costInsert)) costInsert = _insertNonTableChar;

                                // считаем стоимость замены
                                if (!_update.TryGetValue((c1, c2), out costUpdate)) costUpdate = _updateNonTableChar;

                                D[a, j] = Math.Min(Math.Min(
                                    D[b, j] + costDelete,
                                    D[a, j - 1] + costInsert),
                                    D[b, j - 1] + costUpdate
                                );
                            }
                            else
                            {
                                D[a, j] = D[b, j - 1];
                            }
                        }
                    }
                }
                (a, b) = (b, a);
            }

            return relative ? D[b, n] / n : D[b, n];
        }

        /// <summary>
        /// определить степень похожести строк (расстояние Левенштейна)
        /// </summary>
        /// <param name="s1"> строка 1 </param>
        /// <param name="s2"> строка 2 </param>
        /// <param name="D"> матрица </param>
        /// <param name="relative"> указывает, что необходимо считать относительную похожесть </param>
        /// <returns></returns>
        public static double CompareStrings(string s1, string s2, ref double[,] D, bool relative = true)
        {
            s1 = s1.ToLowerInvariant();
            s2 = s2.ToLowerInvariant();

            int m = s1.Length, n = s2.Length;

            // вспомогательные переменные
            int i, j, a = 1, b = 0;
            char c1, c2;
            if (D.GetLength(0) < 2 || D.GetLength(1) < n + 1) D = new double[2, n + 1];
            else
            {
                // очищаем массив
                for (j = 0; j <= n; j++) { D[0, j] = 0; D[1, j] = 0; }
            }
            double costInsert, costDelete, costUpdate;

            for (i = 0; i <= m; i++)
            {
                for (j = 0; j <= n; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        if (i == 0)
                        {
                            // считаем стоимость вставки
                            if (!_insert.TryGetValue(s2[j - 1], out costInsert)) costInsert = _insertNonTableChar;
                            D[1, j] = D[1, j - 1] + costInsert;
                        }
                        else if (j == 0)
                        {
                            // считаем стоимость удаления
                            if (!_delete.TryGetValue(s1[i - 1], out costDelete)) costDelete = _deleteNonTableChar;
                            D[a, 0] = D[b, 0] + costDelete;
                        }
                        else
                        {
                            c1 = s1[i - 1];
                            c2 = s2[j - 1];
                            if (c1 != c2)
                            {
                                // считаем стоимость удаления
                                if (!_delete.TryGetValue(c1, out costDelete)) costDelete = _deleteNonTableChar;

                                // считаем стоимость вставки
                                if (!_insert.TryGetValue(c2, out costInsert)) costInsert = _insertNonTableChar;

                                // считаем стоимость замены
                                if (!_update.TryGetValue((c1, c2), out costUpdate)) costUpdate = _updateNonTableChar;

                                D[a, j] = Math.Min(Math.Min(
                                    D[b, j] + costDelete,
                                    D[a, j - 1] + costInsert),
                                    D[b, j - 1] + costUpdate
                                );
                            }
                            else
                            {
                                D[a, j] = D[b, j - 1];
                            }
                        }
                    }
                }
                (a, b) = (b, a);
            }

            return relative ? D[b, n] / n : D[b, n];
        }

        /// <summary>
        /// статический конструктор
        /// </summary>
        static NumeralLevenshtein()
        {
            ////////////////////////////////////////////////////////////////
            // загружаем данные о символах
            ////////////////////////////////////////////////////////////////

            var data = new List<string[]>(64);
            Type type = typeof(NumeralLevenshtein);
            var name = $"{type.FullName}Data.txt";
            using (var stream = type.Assembly.GetManifestResourceStream(name))
            {
                using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        data.Add(reader.ReadLine().Trim(' ', '\r', '\n').Split('\t'));
                    }
                }
            }

            ////////////////////////////////////////////////////////////////
            // формируем таблицы
            ////////////////////////////////////////////////////////////////

            int n = data[0].Length - 1, row = 0;
            double minCost = double.PositiveInfinity, maxCost = 0;

            _insert = new Dictionary<char, double>(n);
            _delete = new Dictionary<char, double>(n);
            _update = new Dictionary<(char a, char b), double>(n * (n - 1));

            // читаем список символов
            char[] chars = data[row++].Skip(1).Select(e => e[0]).ToArray();

            // INSERT
            foreach (var (index, value) in data[row++].Skip(1).Select((e, index) => (index, value: Parse(e))))
            {
                _insert.Add(chars[index], value);
                if (value < minCost) minCost = value;
                if (value > maxCost) maxCost = value;
            }

            // DELETE
            foreach (var (index, value) in data[row++].Skip(1).Select((e, index) => (index, value: Parse(e))))
            {
                _delete.Add(chars[index], value);
                if (value < minCost) minCost = value;
                if (value > maxCost) maxCost = value;
            }

            // UPDATE
            for (int i = 0; i < n; i++)
            {
                foreach (var (index, value) in data[row++].Skip(1).Select((e, index) => (index, value: Parse(e))))
                {
                    if (i != index)
                    {
                        _update.Add((chars[i], chars[index]), value);
                        if (value < minCost) minCost = value;
                        if (value > maxCost) maxCost = value;
                    }
                }
            }

            // прочие показатели
            _insertNonTableChar = maxCost;
            _deleteNonTableChar = minCost;
            _updateNonTableChar = minCost;
        }

        /// <summary>
        /// распарсить значение из файла данных
        /// </summary>
        /// <param name="str"> строка </param>
        /// <param name="default"> значение по умолчанию </param>
        /// <returns></returns>
        private static double Parse(string str, double @default = 1)
        {
            if (str.Length == 0)
            {
                return @default;
            }
            else
            {
                return double.Parse(str, CultureInfo.InvariantCulture);
            }
        }
    }
}
