/*******************************************************
 * Filename: CSVWriter.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/29/2015 5:35:43 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.CSV
{
    /// <summary>
    /// CSVWriter
    /// </summary>
    public class CSVWriter : IDisposable
    {
        #region Variables

        private const char DEFAULT_ESCAPE_CHARACTER = '"';
        private const char DEFAULT_SEPARATOR = ',';
        private const char DEFAULT_QUOTE_CHARACTER = '"';
        private const char NO_QUOTE_CHARACTER = '\u0000';
        private const char NO_ESCAPE_CHARACTER = '\u0000';
        private const string DEFAULT_LINE_END = "\r\n";

        private Stream rawStream;
        private StreamWriter writer;
        private char separator;
        private char quotechar;
        private char escapechar;
        private string lineEnd;
        private bool isNewLine = true;
        private bool needClose = false;

        #endregion

        #region Properties

        #endregion

        public CSVWriter(string filename, char separator = DEFAULT_SEPARATOR, char quotechar = DEFAULT_QUOTE_CHARACTER, char escapechar = DEFAULT_ESCAPE_CHARACTER, string lineEnd = DEFAULT_LINE_END)
            : this(File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write), separator, quotechar, escapechar, lineEnd)
        {
            needClose = true;
        }

        public CSVWriter(Stream stream, char separator = DEFAULT_SEPARATOR, char quotechar = DEFAULT_QUOTE_CHARACTER, char escapechar = DEFAULT_ESCAPE_CHARACTER, string lineEnd = DEFAULT_LINE_END)
        {
            this.rawStream = stream;
            this.writer = new StreamWriter(stream);
            this.separator = separator;
            this.quotechar = quotechar;
            this.escapechar = escapechar;
            this.lineEnd = lineEnd;
        }

        #region Methods

        public void Write(IEnumerable<IEnumerable<string>> allLines, bool applyQuotesToAll = true)
        {
            foreach (IEnumerable<string> line in allLines)
            {
                Write(line, applyQuotesToAll);
            }
        }

        public void Write(IEnumerable<string> line, bool applyQuotesToAll = true)
        {
            if (line == null || line.Count() == 0)
            {
                return;
            }

            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (string value in line)
            {
                if (i != 0)
                {
                    builder.Append(separator);
                }

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                bool stringContainsSpecialCharacters = StringContainsSpecialCharacters(value);

                if ((applyQuotesToAll || stringContainsSpecialCharacters) && quotechar != NO_QUOTE_CHARACTER)
                {
                    builder.Append(quotechar);
                }

                if (stringContainsSpecialCharacters)
                {
                    builder.Append(ProcessLine(value));
                }
                else
                {
                    builder.Append(value);
                }

                if ((applyQuotesToAll || stringContainsSpecialCharacters) && quotechar != NO_QUOTE_CHARACTER)
                {
                    builder.Append(quotechar);
                }

                i++;
            }

            writer.Write(builder.ToString());
            WriteNewLine();
        }

        public void Write(string value, bool applyQuote = true)
        {
            if (!isNewLine)
            {
                writer.Write(separator);
            }

            if (!string.IsNullOrEmpty(value))
            {
                bool stringContainsSpecialCharacters = StringContainsSpecialCharacters(value);

                if ((applyQuote || stringContainsSpecialCharacters) && quotechar != NO_QUOTE_CHARACTER)
                {
                    writer.Write(quotechar);
                }

                if (stringContainsSpecialCharacters)
                {
                    writer.Write(ProcessLine(value).ToString());
                }
                else
                {
                    writer.Write(value);
                }

                if ((applyQuote || stringContainsSpecialCharacters) && quotechar != NO_QUOTE_CHARACTER)
                {
                    writer.Write(quotechar);
                }
            }

            isNewLine = false;
        }

        public void WriteOriginal(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.Write(value);
                isNewLine = value.EndsWith(lineEnd);
            }
        }

        public void WriteNewLine()
        {
            writer.Write(lineEnd);
            isNewLine = true;
        }

        #endregion

        #region Helper

        private bool StringContainsSpecialCharacters(string line)
        {
            return line.IndexOf(quotechar) != -1 || line.IndexOf(escapechar) != -1 || line.IndexOf(separator) != -1 || line.Contains(DEFAULT_LINE_END) || line.Contains("\r");
        }

        private StringBuilder ProcessLine(string value)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < value.Length; j++)
            {
                char nextChar = value[j];
                ProcessCharacter(sb, nextChar);
            }

            return sb;
        }

        private void ProcessCharacter(StringBuilder sb, char nextChar)
        {
            if (escapechar != NO_ESCAPE_CHARACTER && (nextChar == quotechar || nextChar == escapechar))
            {
                sb.Append(escapechar).Append(nextChar);
            }
            else
            {
                sb.Append(nextChar);
            }
        }

        #endregion

        public void Flush()
        {
            writer.Flush();
        }

        public void Dispose()
        {
            writer.Flush();

            if (needClose)
                writer.Close();
        }
    }
}