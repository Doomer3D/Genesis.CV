using System;

namespace Elect.CV.NumberUtils
{
    /// <summary>
    /// настройки парсера
    /// </summary>
    public class TextToNumberParserOptions
    {
        /// <summary>
        /// предельное значение ошибки в токене, чтобы его можно было использовать для распознавания
        /// </summary>
        public double MaxTokenError { get; set; } = 0.67;

        #region Default

        /// <summary>
        /// настройки по умолчанию
        /// </summary>
        private static TextToNumberParserOptions _default;

        /// <summary>
        /// настройки по умолчанию
        /// </summary>
        public static TextToNumberParserOptions Default
        {
            get
            {
                return _default ?? (_default = new TextToNumberParserOptions());
            }
        }

        #endregion
    }
}
