using System;
using System.Collections.Generic;
using System.Linq;

namespace Genesis.CV.NumberUtils
{
    /// <summary>
    /// класс для преобразования чисел в текст и наоборот
    /// </summary>
    public static partial class RussianNumber
    {
        /// <summary>
        /// преобразовать текст в число
        /// </summary>
        /// <param name="text"> текст </param>
        /// <param name="options"> настройки </param>
        /// <returns></returns>
        public static TextToNumberParserResult Parse(string text, RussianNumberParserOptions options = default)
        {
            if (text == default) throw new ArgumentNullException(nameof(text));
            text = text.Trim().ToLowerInvariant();
            if (text.Length == 0) return TextToNumberParserResult.Failed;

            if (options == default) options = RussianNumberParserOptions.Default;

            // массив для рассчета расстояния Левенштейна
            double[,] D = new double[2, MAX_TOKEN_LENGTH];

            // разбиваем текст на токены
            var stringTokens = _rgSplitter.Split(text);
            var tokens = new List<NumericToken>(stringTokens.Length);

            // обрабатываем токены
            foreach (var item in stringTokens)
            {
                tokens.AddRange(ParseTokens(item, options, ref D, 0));
            }

            // вспомогательные переменные
            long? globalLevel = default, localLevel = default;
            long? globalValue = default, localValue = default;
            bool wasCriticalError = false;

            // цикл по токенам
            int n = tokens.Count;
            for (int i = 0; i < n; i++)
            {
                var token = tokens[i];
                if (token.Error > options.MaxTokenError) continue;

                var (value, level, complex) = token.Value;
                if (complex)
                {
                    // сложное числительное
                    if (!globalLevel.HasValue || globalLevel.Value > level)
                    {
                        globalValue = globalValue.GetValueOrDefault() + (localValue ?? 1) * value;
                        globalLevel = level;
                        localValue = default;
                        localLevel = default;

                        token.IsSignificant = true;
                    }
                    else
                    {
                        // ошибка несоответствия уровней
                        token.Error = 1;
                        token.IsSignificant = true;
                        wasCriticalError = true;
                    }
                }
                else
                {
                    // простое числительное
                    if (!localLevel.HasValue || localLevel.Value > level)
                    {
                        localValue = localValue.GetValueOrDefault() + value;
                        localLevel = level;

                        token.IsSignificant = true;
                    }
                    else
                    {
                        // ошибка несоответствия уровней
                        token.Error = 1;
                        token.IsSignificant = true;
                        wasCriticalError = true;
                    }
                }
            }

            // считаем общий уровень ошибки
            var totalError = tokens.Where(e => e.IsSignificant).Select(e => e.Error).DefaultIfEmpty(1).Average();

            if (wasCriticalError)
            {
                // имело место критическая ошибка
                if (totalError >= 0.5) totalError = 1;
                else totalError *= 2;
            }

            return new TextToNumberParserResult(globalValue.GetValueOrDefault() + localValue.GetValueOrDefault(), totalError);
        }

        /// <summary>
        /// распознать токены
        /// </summary>
        /// <param name="str"> строковое представление </param>
        /// <param name="options"> настройки </param>
        /// <param name="D"> матрица </param>
        /// <param name="level"> уровень рекурсии </param>
        /// <returns></returns>
        private static IEnumerable<NumericToken> ParseTokens(string str, RussianNumberParserOptions options, ref double[,] D, int level)
        {
            if (TOKENS.TryGetValue(str, out var numeral))
            {
                return new NumericToken[] { new NumericToken(numeral) };
            }

            int length = str.Length;
            if (length < 2)
            {
                // слишком короткая строка
                return EMPTY_TOKEN_ARRAY;
            }
            else
            {
                // строка не найдена => просчитываем варианты
                bool complexParsing = length >= 6 && level <= 2;
                var variants = complexParsing ? new List<IEnumerable<NumericToken>>() : default;

                // вспомогательные переменные
                double minimalError, error;

                ////////////////////////////////////////////////////////////////
                // односложная фраза
                ////////////////////////////////////////////////////////////////

                if (length <= MAX_TOKEN_LENGTH)
                {
                    // пытаемся распознать с помощью расстояния Левенштейна
                    minimalError = double.PositiveInfinity;

                    foreach (var (token, data) in TOKENS)
                    {
                        error = NumeralLevenshtein.CompareStrings(str, token, ref D, true);
                        if (error < minimalError)
                        {
                            numeral = data;
                            minimalError = error;
                        }
                    }

                    if (minimalError <= options.MaxTokenError)
                    {
                        if (complexParsing)
                        {
                            // могут быть другие варианты
                            variants.Add(new NumericToken[] { new NumericToken(numeral, minimalError) });
                        }
                        else
                        {
                            return new NumericToken[] { new NumericToken(numeral, minimalError) };
                        }
                    }
                    else if (!complexParsing)
                    {
                        if (level == 0)
                        {
                            // на первом уровне игнорируем плохие токены
                            return EMPTY_TOKEN_ARRAY;
                        }
                        else
                        {
                            // в рекурсии возвращаем плохие токены, чтобы они влияли на принятие решения
                            return new NumericToken[] { new NumericToken(numeral, minimalError) };
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////
                // составная фраза
                ////////////////////////////////////////////////////////////////

                // строки длиной меньше шести смысла делить нет
                if (complexParsing)
                {
                    for (int i = 3; i <= length - 3; i++)
                    {
                        var left = ParseTokens(str.Substring(0, i), options, ref D, level + 1);
                        var right = ParseTokens(str.Substring(i), options, ref D, level + 1);

                        var union = Enumerable.Union(left, right).ToList();
                        if (union.Count > 0)
                        {
                            if (union.Sum(e => e.Error) != 0)
                            {
                                // ухудшаем общий результат на некую величину
                                union.ForEach(e => e.Error += options.SplitErrorValue / union.Count);
                            }

                            // объединяем результат
                            variants.Add(union);
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////
                // выбираем лучший вариант
                ////////////////////////////////////////////////////////////////

                if (variants.Count == 0)
                {
                    return EMPTY_TOKEN_ARRAY;
                }
                else
                {
                    IEnumerable<NumericToken> best = default;

                    minimalError = double.PositiveInfinity;
                    foreach (var item in variants)
                    {
                        error = item.Sum(e => e.Error);
                        if (error < minimalError)
                        {
                            best = item;
                            minimalError = error;
                        }
                    }

                    return best ?? EMPTY_TOKEN_ARRAY;
                }
            }
        }
    }
}
