using System;

namespace Genesis.CV.NumberUtils
{
    /// <summary>
    /// результат преобразования текста в число
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Value: {Value}; Error: {Error}")]
    public class RussianNumberParserResult
    {
        /// <summary>
        /// значение
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// ошибка распознавания
        /// </summary>
        public double Error { get; private set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        public RussianNumberParserResult(long value) => (Value, Error) = (value, 0);

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="error"> ошибка распознавания </param>
        public RussianNumberParserResult(long value, double error) => (Value, Error) = (value, error);

        public override string ToString() => Value.ToString();

        /// <summary>
        /// ошибочный результат
        /// </summary>
        public static RussianNumberParserResult Failed { get; private set; }

        /// <summary>
        /// статический конструктор
        /// </summary>
        static RussianNumberParserResult()
        {
            Failed = new RussianNumberParserResult(0, 1);
        }

        public static implicit operator int(RussianNumberParserResult value) => (int)value.Value;
        public static implicit operator long(RussianNumberParserResult value) => value.Value;
    }
}
