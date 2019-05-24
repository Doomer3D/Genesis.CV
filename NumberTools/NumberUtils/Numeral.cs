using System;

namespace Elect.CV.NumberUtils
{
    /// <summary>
    /// числительное
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Value: {Value}; Level: {Level}")]
    public class Numeral
    {
        /// <summary>
        /// значение
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// уровень
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// признак сложного числительного
        /// </summary>
        /// <remarks>
        /// тысяча, миллион и прочие числительные, умножаемые на коэффициент перед ними
        /// </remarks>
        public bool IsComplexNumeral { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="level"> уровень </param>
        /// <param name="isComplexNumeral"> признак сложного числительного </param>
        public Numeral(long value, int level, bool isComplexNumeral) => (Value, Level, IsComplexNumeral) = (value, level, isComplexNumeral);

        /// <summary>
        /// деконструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="level"> уровень </param>
        /// <param name="isComplexNumeral"> признак сложного числительного </param>
        public void Deconstruct(out long value, out int level, out bool isComplexNumeral) => (value, level, isComplexNumeral) = (Value, Level, IsComplexNumeral);
    }
}
