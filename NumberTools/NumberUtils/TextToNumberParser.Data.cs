#define USE_MILLIONS
#define USE_MILLIARDS
#define USE_TRILLIONS

#define USE_SLANG_WORDS

using System;
using System.Collections.Generic;

namespace Elect.CV.NumberUtils
{
    /// <summary>
    /// преобразователь текста в число
    /// </summary>
    public partial class TextToNumberParser
    {
        /// <summary>
        /// максимальная длина токена
        /// </summary>
        private const int MAX_TOKEN_LENGTH = 12;

        /// <summary>
        /// пустой массив токенов
        /// </summary>
        private static readonly NumericToken[] EMPTY_TOKEN_ARRAY = new NumericToken[] { };

        /// <summary>
        /// хеш токенов
        /// </summary>
        private static readonly Dictionary<string, Numeral> TOKENS = new Dictionary<string, Numeral>
        {
            { "ноль",         new Numeral(0   , 1, false ) },
            { "один",         new Numeral(1   , 1, false ) },
            { "одна",         new Numeral(1   , 1, false ) },
            { "два",          new Numeral(2   , 1, false ) },
            { "две",          new Numeral(2   , 1, false ) },
            { "три",          new Numeral(3   , 1, false ) },
            { "четыре",       new Numeral(4   , 1, false ) },
            { "пять",         new Numeral(5   , 1, false ) },
            { "шесть",        new Numeral(6   , 1, false ) },
            { "семь",         new Numeral(7   , 1, false ) },
            { "восемь",       new Numeral(8   , 1, false ) },
            { "девять",       new Numeral(9   , 1, false ) },
            { "десять",       new Numeral(10  , 1, false ) },
            { "одиннадцать",  new Numeral(11  , 1, false ) },
            { "двенадцать",   new Numeral(12  , 1, false ) },
            { "тринадцать",   new Numeral(13  , 1, false ) },
            { "четырнадцать", new Numeral(14  , 1, false ) },
            { "пятнадцать",   new Numeral(15  , 1, false ) },
            { "шестнадцать",  new Numeral(16  , 1, false ) },
            { "семнадцать",   new Numeral(17  , 1, false ) },
            { "восемнадцать", new Numeral(18  , 1, false ) },
            { "девятнадцать", new Numeral(19  , 1, false ) },
            { "двадцать",     new Numeral(20  , 2, false ) },
            { "тридцать",     new Numeral(30  , 2, false ) },
            { "сорок",        new Numeral(40  , 2, false ) },
            { "пятьдесят",    new Numeral(50  , 2, false ) },
            { "шестьдесят",   new Numeral(60  , 2, false ) },
            { "семьдесят",    new Numeral(70  , 2, false ) },
            { "восемьдесят",  new Numeral(80  , 2, false ) },
            { "девяносто",    new Numeral(90  , 2, false ) },
            { "сто",          new Numeral(100 , 3, false ) },
            { "двести",       new Numeral(200 , 3, false ) },
            { "триста",       new Numeral(300 , 3, false ) },
            { "четыреста",    new Numeral(400 , 3, false ) },
            { "пятьсот",      new Numeral(500 , 3, false ) },
            { "шестьсот",     new Numeral(600 , 3, false ) },
            { "семьсот",      new Numeral(700 , 3, false ) },
            { "восемьсот",    new Numeral(800 , 3, false ) },
            { "девятьсот",    new Numeral(900 , 3, false ) },
            { "тысяч",        new Numeral(1000, 4, true  ) },
            { "тысяча",       new Numeral(1000, 4, true  ) },
            { "тысячи",       new Numeral(1000, 4, true  ) },
#if USE_MILLIONS
            { "миллион",      new Numeral(1000000, 5, true  ) },
            { "миллиона",     new Numeral(1000000, 5, true  ) },
            { "миллионов",    new Numeral(1000000, 5, true  ) },
#endif
#if USE_MILLIARDS
            { "миллиард",     new Numeral(1000000000, 6, true  ) },
            { "миллиарда",    new Numeral(1000000000, 6, true  ) },
            { "миллиардов",   new Numeral(1000000000, 6, true  ) },
#endif
#if USE_TRILLIONS
            { "триллион",     new Numeral(1000000000000, 7, true  ) },
            { "триллиона",    new Numeral(1000000000000, 7, true  ) },
            { "триллионов",   new Numeral(1000000000000, 7, true  ) },
#endif
#if USE_SLANG_WORDS
            { "трешка",       new Numeral(3,    1, true  ) },
            { "трешки",       new Numeral(3,    1, true  ) },
            { "трешкек",      new Numeral(3,    1, true  ) },
            { "червонец",     new Numeral(10,   1, true  ) },
            { "червонца",     new Numeral(10,   1, true  ) },
            { "червонцев",    new Numeral(10,   1, true  ) },
            { "полтос",       new Numeral(50,   1, true  ) },
            { "полтоса",      new Numeral(50,   1, true  ) },
            { "полтосов",     new Numeral(50,   1, true  ) },
            { "полтинник",    new Numeral(50,   1, true  ) },
            { "полтинника",   new Numeral(50,   1, true  ) },
            { "полтинников",  new Numeral(50,   1, true  ) },
            { "сотка",        new Numeral(100,  3, true  ) },
            { "сотки",        new Numeral(100,  3, true  ) },
            { "соток",        new Numeral(100,  3, true  ) },
            { "стольник",     new Numeral(100,  3, true  ) },
            { "стольника",    new Numeral(100,  3, true  ) },
            { "стольников",   new Numeral(100,  3, true  ) },
            { "пятихатка",    new Numeral(500,  3, true  ) },
            { "пятихатки",    new Numeral(500,  3, true  ) },
            { "пятихаток",    new Numeral(500,  3, true  ) },
            { "тыща",         new Numeral(1000, 4, true  ) },
            { "тыщи",         new Numeral(1000, 4, true  ) },
            { "тыщ",          new Numeral(1000, 4, true  ) },
            { "штука",        new Numeral(1000, 4, true  ) },
            { "штуки",        new Numeral(1000, 4, true  ) },
            { "штук",         new Numeral(1000, 4, true  ) },
            { "косарь",       new Numeral(1000, 4, true  ) },
            { "косаря",       new Numeral(1000, 4, true  ) },
            { "косарей",      new Numeral(1000, 4, true  ) },
            { "кусок",        new Numeral(1000, 4, true  ) },
            { "куска",        new Numeral(1000, 4, true  ) },
            { "кусков",       new Numeral(1000, 4, true  ) },
            { "лимон",        new Numeral(1000000, 5, true  ) },
            { "лимона",       new Numeral(1000000, 5, true  ) },
            { "лимонов",      new Numeral(1000000, 5, true  ) },
#endif
        };
    }
}
