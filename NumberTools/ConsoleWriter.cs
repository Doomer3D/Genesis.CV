using System;
using System.Text;

namespace Genesis.IO
{
    /// <summary>
    /// консольный писатель
    /// </summary>
    public class ConsoleWriter
    {
        private ConsoleColor _successColor;     // цвет сообщений для успешных операций
        private ConsoleColor _errorColor;       // цвет сообщений об ошибках
        private ConsoleColor _warningColor;     // цвет сообщений-предупреждений

        private string _successText;            // текст сообщений для успешных операций
        private string _errorText;              // текст сообщений об ошибках
        private string _warningText;            // текст сообщений-предупреждений

        /// <summary>
        /// цвет сообщений для успешных операций
        /// </summary>
        public ConsoleColor SuccessColor { get => _successColor; set => _successColor = value; }

        /// <summary>
        /// цвет сообщений об ошибках
        /// </summary>
        public ConsoleColor ErrorColor { get => _errorColor; set => _errorColor = value; }

        /// <summary>
        /// цвет сообщений-предупреждений
        /// </summary>
        public ConsoleColor WarningColor { get => _warningColor; set => _warningColor = value; }

        /// <summary>
        /// текст сообщений для успешных операций
        /// </summary>
        public string SuccessText { get => _successText; set => _successText = value; }

        /// <summary>
        /// текст сообщений об ошибках
        /// </summary>
        public string ErrorText { get => _errorText; set => _errorText = value; }

        /// <summary>
        /// текст сообщений-предупреждений
        /// </summary>
        public string WarningText { get => _warningText; set => _warningText = value; }

        /// <summary>
        /// цвет текста
        /// </summary>
        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }

        /// <summary>
        /// цвет фона
        /// </summary>
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }

        /// <summary>
        /// конструктор
        /// </summary>
        public ConsoleWriter()
        {
            _successColor = ConsoleColor.Green;
            _errorColor = ConsoleColor.Red;
            _warningColor = ConsoleColor.Yellow;

            _successText = "OK";
            _errorText = "ERROR";
            _warningText = "WARNING";
        }

        #region Write | WriteLine

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void Write(string value)
        {
            Console.Write(value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="color"> цвет сообщения </param>
        /// <param name="value"> сообщение </param>
        public void Write(ConsoleColor color, string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void Write(string format, params object[] args)
        {
            Console.Write(string.Format(format, args));
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="color"> цвет сообщения </param>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void Write(ConsoleColor color, string format, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(string.Format(format, args));
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// записать перевод строки в консоль
        /// </summary>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="color"> цвет сообщения </param>
        /// <param name="value"> сообщение </param>
        public void WriteLine(ConsoleColor color, string value)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="color"> цвет сообщения </param>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(string.Format(format, args));
            Console.ForegroundColor = oldColor;
        }

        #endregion
        #region WriteSuccess | WriteLineSuccess

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteSuccess()
        {
            Write(_successColor, _successText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteSuccess(string value)
        {
            Write(_successColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteSuccess(string format, params object[] args)
        {
            Write(_successColor, string.Format(format, args));
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteLineSuccess()
        {
            WriteLine(_successColor, _successText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteLineSuccess(string value)
        {
            WriteLine(_successColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteLineSuccess(string format, params object[] args)
        {
            WriteLine(_successColor, string.Format(format, args));
        }

        #endregion
        #region WriteError | WriteLineError

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteError()
        {
            Write(_errorColor, _errorText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteError(string value)
        {
            Write(_errorColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteError(string format, params object[] args)
        {
            Write(_errorColor, string.Format(format, args));
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteLineError()
        {
            WriteLine(_errorColor, _errorText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteLineError(string value)
        {
            WriteLine(_errorColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteLineError(string format, params object[] args)
        {
            WriteLine(_errorColor, string.Format(format, args));
        }

        #endregion
        #region WriteWarning | WriteLineWarning

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteWarning()
        {
            Write(_warningColor, _warningText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteWarning(string value)
        {
            Write(_warningColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteWarning(string format, params object[] args)
        {
            Write(_warningColor, string.Format(format, args));
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        public void WriteLineWarning()
        {
            WriteLine(_warningColor, _warningText);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="value"> сообщение </param>
        public void WriteLineWarning(string value)
        {
            WriteLine(_warningColor, value);
        }

        /// <summary>
        /// записать сообщение в консоль
        /// </summary>
        /// <param name="format"> строка формата </param>
        /// <param name="args"> аргументы </param>
        public void WriteLineWarning(string format, params object[] args)
        {
            WriteLine(_warningColor, string.Format(format, args));
        }

        #endregion

        /// <summary>
        /// записать исключение в лог
        /// </summary>
        /// <param name="exception"> исключение </param>
        public void WriteException(Exception exception)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Message:");
            for (var item = exception; item != null; item = item.InnerException)
            {
                sb.AppendLine(item.Message);
            }
            sb.AppendLine();
            sb.AppendLine("StackTrace:");
            sb.AppendLine(exception.StackTrace);

            WriteLineError(sb.ToString().Trim());
        }
    }
}
