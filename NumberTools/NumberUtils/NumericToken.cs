using System;

namespace Genesis.CV.NumberUtils
{
    /// <summary>
    /// числовой токен
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Value: {Value.Value}; Error: {Error}")]
    public class NumericToken
    {
        /// <summary>
        /// значение
        /// </summary>
        public Numeral Value { get; private set; }

        /// <summary>
        /// ошибка распознавания
        /// </summary>
        public double Error { get; set; }

        /// <summary>
        /// указывает, что токен был важен для распознавания
        /// </summary>
        public bool IsSignificant { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        public NumericToken(Numeral value) => (Value, Error) = (value, 0);

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="error"> ошибка распознавания </param>
        public NumericToken(Numeral value, double error) => (Value, Error) = (value, error);
    }
}
