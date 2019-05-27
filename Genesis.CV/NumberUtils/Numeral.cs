using System;

namespace Genesis.CV.NumberUtils
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
        /// признак множителя
        /// </summary>
        /// <remarks>
        /// тысяча, миллион и прочие числительные, умножаемые на коэффициент перед ними
        /// </remarks>
        public bool IsMultiplier { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="level"> уровень </param>
        /// <param name="isMultiplier"> признак множителя </param>
        public Numeral(long value, int level, bool isMultiplier) => (Value, Level, IsMultiplier) = (value, level, isMultiplier);

        /// <summary>
        /// деконструктор
        /// </summary>
        /// <param name="value"> значение </param>
        /// <param name="level"> уровень </param>
        /// <param name="isMultiplier"> признак множителя </param>
        public void Deconstruct(out long value, out int level, out bool isMultiplier) => (value, level, isMultiplier) = (Value, Level, IsMultiplier);
    }
}
