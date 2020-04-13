/*******************************************************
 * Filename: CSVReader.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/29/2015 5:35:32 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace HKH.CSV
{
    public enum CSVReaderNullFieldIndicator
    {
        EMPTY,
        EMPTY_DELIMITED,
        BOTH,
        NEITHER
    }

    /// <summary>
    /// CSVReader
    /// </summary>
    public class CSVReader : IEnumerable<IEnumerable<string>>, IEnumerable, IDisposable
    {
        #region Variables

        public const bool DEFAULT_KEEP_CR = false;
        public const bool DEFAULT_VERIFY_READER = true;
        public const int DEFAULT_SKIP_LINES = 0;

        private CSVParser parser;
        private int skipLines;
        private StreamReader reader;
        private LineReader lineReader;
        private bool hasNext = true;
        private bool linesSkiped;
        //private bool keepCR;
        private bool verifyReader;

        #endregion

        #region Properties

        #endregion

        public CSVReader(string filename, char separator = CSVParser.DEFAULT_SEPARATOR, char quotechar = CSVParser.DEFAULT_QUOTE_CHARACTER, char escape = CSVParser.DEFAULT_ESCAPE_CHARACTER,
            bool strictQuotes = CSVParser.DEFAULT_STRICT_QUOTES, bool ignoreLeadingWhiteSpace = CSVParser.DEFAULT_IGNORE_LEADING_WHITESPACE, int line = DEFAULT_SKIP_LINES, bool keepCR = DEFAULT_KEEP_CR, bool verifyReader = DEFAULT_VERIFY_READER)
            : this(File.OpenRead(filename), new CSVParser(separator, quotechar, escape, strictQuotes, ignoreLeadingWhiteSpace), line, keepCR, DEFAULT_VERIFY_READER)
        {
        }

        public CSVReader(Stream stream, char separator = CSVParser.DEFAULT_SEPARATOR, char quotechar = CSVParser.DEFAULT_QUOTE_CHARACTER, char escape = CSVParser.DEFAULT_ESCAPE_CHARACTER,
            bool strictQuotes = CSVParser.DEFAULT_STRICT_QUOTES, bool ignoreLeadingWhiteSpace = CSVParser.DEFAULT_IGNORE_LEADING_WHITESPACE, int line = DEFAULT_SKIP_LINES, bool keepCR = DEFAULT_KEEP_CR, bool verifyReader = DEFAULT_VERIFY_READER)
            : this(stream, new CSVParser(separator, quotechar, escape, strictQuotes, ignoreLeadingWhiteSpace), line, keepCR, DEFAULT_VERIFY_READER)
        {
        }

        internal CSVReader(Stream stream, CSVParser csvParser, int line = DEFAULT_SKIP_LINES, bool keepCR = DEFAULT_KEEP_CR, bool verifyReader = DEFAULT_VERIFY_READER)
        {
            this.reader = new StreamReader(stream);
            this.lineReader = new LineReader(reader, keepCR);
            this.skipLines = line;
            this.parser = csvParser;
            //this.keepCR = keepCR;
            this.verifyReader = verifyReader;
        }

        #region Methods
        public IEnumerable<IEnumerable<string>> ReadSample(int count)
        {
            return ReadCore(count);
        }

        public IEnumerable<IEnumerable<string>> ReadAll()
        {
            return ReadCore(-1);
        }
        public IEnumerable<string> ReadLine()
        {
            string[] result = null;
            do
            {
                string nextLine = NextLine();
                if (!hasNext)
                {
                    return result; // should throw if still pending?
                }
                string[] r = parser.ParseLine(nextLine, true);
                if (r.Length > 0)
                {
                    if (result == null)
                    {
                        result = r;
                    }
                    else
                    {
                        result = CombineResultsFromMultipleReads(result, r);
                    }
                }
            } while (parser.IsPending);

            return result;
        }
        public DataTable ReadTableSample(int count, bool fHeader = false)
        {
            return ReadTableCore(count, fHeader);
        }
        public DataTable ReadTable(bool fHeader = false)
        {
            return ReadTableCore(-1, fHeader);
        }

        private void AddRow(DataTable dt, IEnumerable<string> line)
        {
            if (line != null)
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < line.Count(); i++)
                {
                    row[i] = line.ElementAt(i);
                }
                dt.Rows.Add(row);
            }
        }

        #endregion

        #region Helper

        private string[] CombineResultsFromMultipleReads(string[] buffer, string[] lastRead)
        {
            string[] t = new string[buffer.Length + lastRead.Length];
            Array.Copy(buffer, 0, t, 0, buffer.Length);
            Array.Copy(lastRead, 0, t, buffer.Length, lastRead.Length);
            return t;
        }

        private IEnumerable<IEnumerable<string>> ReadCore(int count)
        {
            List<IEnumerable<string>> lines = new List<IEnumerable<string>>();
            int currentLineNumber = -1;

            while (hasNext && (count == -1 || currentLineNumber < count))
            {
                IEnumerable<string> nextLineAsTokens = ReadLine();
                if (nextLineAsTokens != null)
                {
                    lines.Add(nextLineAsTokens);
                    currentLineNumber++;
                }
            }
            return lines;
        }
        private DataTable ReadTableCore(int count, bool fHeader)
        {
            DataTable dt = new DataTable();
            int currentLineNumber = -1;

            IEnumerable<string> line = ReadLine();

            if (line != null)
            {
                if (fHeader)
                {
                    for (int i = 0; i < line.Count(); i++)
                    {
                        string colName = line.ElementAt(i);
                        dt.Columns.Add(GetColumnName(dt, string.IsNullOrEmpty(colName) ? string.Format("Column{0}", i + 1) : colName));
                    }
                }
                else
                {
                    for (int i = 0; i < line.Count(); i++)
                    {
                        dt.Columns.Add(GetColumnName(dt, string.Format("Column{0}", i + 1)));
                    }

                    //read first line if not header
                    AddRow(dt, line);

                    currentLineNumber++;
                }

                while (hasNext && (count == -1 || currentLineNumber < count))
                {
                    line = ReadLine();
                    AddRow(dt, line);

                    currentLineNumber++;
                }
            }

            return dt;
        }
        private string GetColumnName(DataTable dt, string baseName)
        {
            string colName = baseName;

            while (dt.Columns.Contains(colName))
                colName += "_1";

            return colName;
        }

        private string NextLine()
        {
            if (IsClosed())
            {
                hasNext = false;
                return null;
            }

            if (!this.linesSkiped)
            {
                for (int i = 0; i < skipLines; i++)
                {
                    lineReader.ReadLine();
                }
                this.linesSkiped = true;
            }
            string nextLine = lineReader.ReadLine();
            if (nextLine == null)
            {
                hasNext = false;
            }
            return hasNext ? nextLine : null;
        }

        private bool IsClosed()
        {
            if (!verifyReader)
            {
                return false;
            }
            return reader.EndOfStream;
        }

        #endregion

        public void Close()
        {
            reader.Close();
        }
        public void Dispose()
        {
            reader.Close();
        }

        public IEnumerator<IEnumerable<string>> GetEnumerator()
        {
            return new CSVEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

