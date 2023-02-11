/*******************************************************
 * Filename: CSVParser.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/29/2015 5:41:20 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HKH.CSV
{
	/// <summary>
	/// CSVParser
	/// </summary>
	public class CSVParser
	{
		#region Variables

		internal const char NULL_CHARACTER = '\0';
		internal const char DEFAULT_SEPARATOR = ',';
		internal const char DEFAULT_QUOTE_CHARACTER = '"';
        internal const char DEFAULT_ESCAPE_CHARACTER = '\0';    //Default no escape char
		internal const bool DEFAULT_STRICT_QUOTES = false;
		internal const bool DEFAULT_IGNORE_LEADING_WHITESPACE = true;
		internal const bool DEFAULT_IGNORE_QUOTATIONS = false;
		internal const CSVReaderNullFieldIndicator DEFAULT_NULL_FIELD_INDICATOR = CSVReaderNullFieldIndicator.NEITHER;

		private char separator;
		private char quotechar;
		private char escape;
		private bool strictQuotes;
		private bool ignoreLeadingWhiteSpace;
		private bool ignoreQuotations;
		private CSVReaderNullFieldIndicator nullFieldIndicator;
		private string pending;
		private bool inField = false;

		#endregion

		#region Properties

		#endregion

		public CSVParser(char separator = DEFAULT_SEPARATOR, char quotechar = DEFAULT_QUOTE_CHARACTER, char escape = DEFAULT_ESCAPE_CHARACTER,
			bool strictQuotes = DEFAULT_STRICT_QUOTES, bool ignoreLeadingWhiteSpace = DEFAULT_IGNORE_LEADING_WHITESPACE,
				  bool ignoreQuotations = DEFAULT_IGNORE_QUOTATIONS, CSVReaderNullFieldIndicator nullFieldIndicator = DEFAULT_NULL_FIELD_INDICATOR)
		{
			if (AnyCharactersAreTheSame(separator, quotechar, escape))
			{
				throw new NotSupportedException("The separator, quote, and escape characters must be different!");
			}
			if (separator == NULL_CHARACTER)
			{
				throw new NotSupportedException("The separator character must be defined!");
			}
			this.separator = separator;
			this.quotechar = quotechar;
			this.escape = escape;
			this.strictQuotes = strictQuotes;
			this.ignoreLeadingWhiteSpace = ignoreLeadingWhiteSpace;
			this.ignoreQuotations = ignoreQuotations;
			this.nullFieldIndicator = nullFieldIndicator;
		}

		private bool AnyCharactersAreTheSame(char separator, char quotechar, char escape)
		{
			return IsSameCharacter(separator, quotechar) || IsSameCharacter(separator, escape) || IsSameCharacter(quotechar, escape);
		}

		private bool IsSameCharacter(char c1, char c2)
		{
			return c1 != NULL_CHARACTER && c1 == c2;
		}

		public bool IsPending
		{
			get { return pending != null; }
		}

		public string[] ParseLine(string nextLine, bool multi = false)
		{

			if (!multi && pending != null)
			{
				pending = null;
			}

			if (nextLine == null)
			{
				if (pending != null)
				{
					string s = pending;
					pending = null;
					return new string[] { s };
				}
				else
				{
					return null;
				}
			}

			List<string> tokensOnThisLine = new List<string>();
			StringBuilder sb = new StringBuilder();
			bool inQuotes = false;
			bool fromQuotedField = false;
			if (pending != null)
			{
				sb.Append(pending);
				pending = null;
				inQuotes = !this.ignoreQuotations;//true;
			}
			for (int i = 0; i < nextLine.Length; i++)
			{
				char c = nextLine[i];
				if (c == this.escape)
				{
					if (IsNextCharacterEscapable(nextLine, InQuotes(inQuotes), i))
					{
						i = AppendNextCharacterAndAdvanceLoop(nextLine, sb, i);
					}
				}
				else if (c == quotechar)
				{
					if (isNextCharacterEscapedQuote(nextLine, InQuotes(inQuotes), i))
					{
						i = AppendNextCharacterAndAdvanceLoop(nextLine, sb, i);
					}
					else
					{
						inQuotes = !inQuotes;
						if (AtStartOfField(sb))
						{
							fromQuotedField = true;
						}

						// the tricky case of an embedded quote in the middle: a,bc"d"ef,g
						if (!strictQuotes)
						{
							if (i > 2 //not on the beginning of the line
									&& nextLine[i - 1] != this.separator //not at the beginning of an escape sequence
									&& nextLine.Length > (i + 1) &&
									nextLine[i + 1] != this.separator //not at the	end of an escape sequence
									)
							{
								if (ignoreLeadingWhiteSpace && sb.Length > 0 && IsAllWhiteSpace(sb))
								{
									sb.Clear();
								}
								else
								{
									sb.Append(c);
								}
							}
						}
					}
					inField = !inField;
				}
				else if (c == separator && !(inQuotes && !ignoreQuotations))
				{
					tokensOnThisLine.Add(ConvertEmptyToNullIfNeeded(sb.ToString(), fromQuotedField));
					fromQuotedField = false;
					sb.Clear();
					inField = false;
				}
				else
				{
					if (!strictQuotes || (inQuotes && !ignoreQuotations))
					{
						sb.Append(c);
						inField = true;
						fromQuotedField = true;
					}
				}

			}
			// line is done - check status
			if ((inQuotes && !ignoreQuotations))
			{
				if (multi)
				{
					// continuing a quoted section, re-append newline
					sb.Append('\n');
					pending = sb.ToString();
					sb = null; // this partial content is not to be added to field list yet
				}
				else
				{
					throw new IOException("Un-terminated quoted field at end of CSV line");
				}
				if (inField)
				{
					fromQuotedField = true;
				}
			}
			else
			{
				inField = false;
			}

			if (sb != null)
			{
				tokensOnThisLine.Add(ConvertEmptyToNullIfNeeded(sb.ToString(), fromQuotedField));
				fromQuotedField = false;
			}

			return tokensOnThisLine.ToArray();
		}

		private bool AtStartOfField(StringBuilder sb)
		{
			return sb.Length == 0;
		}

		private string ConvertEmptyToNullIfNeeded(string s, bool fromQuotedField)
		{
			if (string.IsNullOrEmpty(s) && ShouldConvertEmptyToNull(fromQuotedField))
			{
				return null;
			}
			return s;
		}

		private bool ShouldConvertEmptyToNull(bool fromQuotedField)
		{
			switch (nullFieldIndicator)
			{
				case CSVReaderNullFieldIndicator.BOTH:
					return true;
				case CSVReaderNullFieldIndicator.EMPTY:
					return !fromQuotedField;
				case CSVReaderNullFieldIndicator.EMPTY_DELIMITED:
					return fromQuotedField;
				default:
					return false;
			}
		}

		private int AppendNextCharacterAndAdvanceLoop(string line, StringBuilder sb, int i)
		{
			sb.Append(line[i + 1]);
			i++;
			return i;
		}

		private bool InQuotes(bool inQuotes)
		{
			return (inQuotes && !ignoreQuotations) || inField;
		}

		private bool isNextCharacterEscapedQuote(string nextLine, bool inQuotes, int i)
		{
			return inQuotes  // we are in quotes, therefore there can be escaped quotes in here.
					&& nextLine.Length > (i + 1)  // there is indeed another character to check.
					&& IsCharacterQuoteCharacter(nextLine[i + 1]);
		}

		private bool IsCharacterQuoteCharacter(char c)
		{
			return c == quotechar;
		}

		private bool IsCharacterEscapeCharacter(char c)
		{
			return c == escape;
		}

		private bool IsCharacterEscapable(char c)
		{
			return IsCharacterQuoteCharacter(c) || IsCharacterEscapeCharacter(c);
		}

		private bool IsNextCharacterEscapable(string nextLine, bool inQuotes, int i)
		{
			return inQuotes  // we are in quotes, therefore there can be escaped quotes in here.
					&& nextLine.Length > (i + 1)  // there is indeed another character to check.
					&& IsCharacterEscapable(nextLine[i + 1]);
		}

		private bool IsAllWhiteSpace(StringBuilder sb)
		{
			return string.IsNullOrWhiteSpace(sb.ToString());
		}

		#region Methods

		#endregion

		#region Helper

		#endregion
	}
}