using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Elect.CV.NumberUtils
{
    /// <summary>
    /// преобразователь текста в число
    /// </summary>
    public partial class TextToNumberParser
    {
        protected readonly Regex _rgSplitter = new Regex(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// преобразовать текст в число
        /// </summary>
        /// <param name="text"> текст </param>
        /// <param name="options"> настройки </param>
        /// <returns></returns>
        public virtual TextToNumberParserResult Parse(string text, TextToNumberParserOptions options = default)
        {
            if (text == default) throw new ArgumentNullException(nameof(text));
            text = text.Trim().ToLowerInvariant();
            if (text.Length == 0) return TextToNumberParserResult.Failed;

            if (options == default) options = TextToNumberParserOptions.Default;

            // разбиваем текст на токены
            var stringTokens = _rgSplitter.Split(text);
            var tokens = new List<NumericToken>(stringTokens.Length);

            // обрабатываем токены
            foreach (var item in stringTokens)
            {
                tokens.AddRange(ParseTokens(item));
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
            var totalError = tokens.Where(e => e.IsSignificant).Average(e => e.Error);

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
        /// <returns></returns>
        protected IEnumerable<NumericToken> ParseTokens(string str)
        {
            if (TOKENS.TryGetValue(str, out var numeral))
            {
                yield return new NumericToken(numeral);
            }
            else
            {
            }
        }
    }
}
