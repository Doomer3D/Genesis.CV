using System;

namespace Genesis.CV.NumberUtils
{
    /// <summary>
    /// настройки парсера
    /// </summary>
    public class RussianNumberParserOptions
    {
        /// <summary>
        /// предельное значение ошибки в токене, чтобы его можно было использовать для распознавания
        /// </summary>
        public double MaxTokenError { get; set; } = 0.67;

        /// <summary>
        /// величина, на которую ухудшается результат после деления подстроки на более мелкие части
        /// </summary>
        public double SplitErrorValue { get; set; } = 0.1;

        /// <summary>
        /// настройки по умолчанию
        /// </summary>
        private static RussianNumberParserOptions _default;

        /// <summary>
        /// настройки по умолчанию
        /// </summary>
        public static RussianNumberParserOptions Default
        {
            get
            {
                return _default ?? (_default = new RussianNumberParserOptions());
            }
        }
    }
}
