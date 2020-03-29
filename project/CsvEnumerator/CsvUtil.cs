using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvEnumerator
{
    public static class CsvUtil
    {
        public class CsvUnexpectedCharException : Exception
        {
            public string Content { get; private set; }

            public CsvUnexpectedCharException(string content, string message = null)
                : base(message ?? "Unexpected csv content")
            {
                Content = content;
            }
        }

        public static string EncodeCsvField(this string field, bool enforceQuote = false)
        {
            var sb = new StringBuilder();
            foreach (var c in field)
            {
                switch (c)
                {
                    case '"':
                        enforceQuote = true;
                        sb.Append("\"");
                        break;
                    case '\n':
                    case '\r':
                    case ',':
                        enforceQuote = true;
                        break;
                }
                sb.Append(c);
            }
            return enforceQuote ? String.Concat("\"", sb, "\"") : sb.ToString();
        }

        /// <summary>
        /// Decode a csv field from an inpu string
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="allowUnmatchedQuote">whether to ignore unmatched quote</param>
        /// <param name="inFile">indicates if this parsing is against a whole file to determine whether to return a null to indicate the end </param>
        /// <returns></returns>
        public static string DecodeCsvField(this string field, bool allowUnmatchedQuote, bool inFile = true)
        {
            return new SeekableString(field).DecodeCsvField(allowUnmatchedQuote, inFile);
        }

        /// <summary>
        /// Decode a csv field from an inpu string
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="allowUnmatchedQuote">whether to ignore unmatched quote</param>
        /// <param name="inFile">indicates if this parsing is against a whole file to determine whether to return a null to indicate the end </param>
        /// <returns></returns>
        public static string DecodeCsvField(this ISeekableString inputStream, bool allowUnmatchedQuote, bool inFile = true)
        {
            var count = 0;
            bool? quote = null;
            var sb = new StringBuilder();
            while (true)
            {
                var peek = inputStream.ReadChar();
                switch (peek)
                {
                    case -1:
                        return (inFile && count == 0) ? null : sb.ToString();
                    case '\n':
                    case '\r':
                        if (count == 0)
                        {
                            //line end
                            peek = inputStream.PeekChar();
                            while (peek == '\r' || peek == '\n')
                            {
                                inputStream.ReadChar();
                                peek = inputStream.PeekChar();
                            }
                            return null;
                        }
                        if (quote == true)
                        {
                            sb.Append((char)peek);
                            count++;
                        }
                        else
                        {
                            inputStream.Back();
                            return quote.HasValue ? sb.ToString() : sb.ToString().Trim();
                        }
                        break;
                    case ',':
                        if (quote == true)
                        {
                            sb.Append((char)peek);
                            count++;
                        }
                        else if (count == 0)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return quote.HasValue ? sb.ToString() : sb.ToString().Trim();
                        }
                        break;
                    case '"':
                        if (count == 0 && quote.HasValue != true)
                        {
                            quote = true;
                        }
                        else if (quote == true)
                        {
                            peek = inputStream.PeekChar();
                            if (peek == '"')
                            {
                                sb.Append((char)inputStream.ReadChar());
                                count++;
                            }
                            else
                            {
                                quote = false;
                            }
                        }
                        else if (count > 0)
                        {
                            if (allowUnmatchedQuote)
                            {
                                sb.Append((char)peek);
                                count++;
                            }
                            else
                            {
                                throw new CsvUnexpectedCharException("\"", "Quote must be encoded");
                            }
                        }
                        break;
                    case ' ':
                        if (count != 0 && quote != false)
                        {
                            sb.Append((char)peek);
                            count++;
                        }
                        break;
                    default:
                        sb.Append((char)peek);
                        count++;
                        break;
                }
            }
        }

        public static IList<IList<string>> ValidateCsv(this ISeekableString csvRefString, IList<string> errors, bool allowEmptyFields = true, bool alignColumnCounts = true)
        {
            var enumable = new CsvEnumerable(csvRefString);
            var dataSet = new List<IList<string>>();
            errors = errors ?? new List<string>() ;
            int? count = null;
            foreach (var record in enumable)
            {
                var recordFields = record.ToList();
                dataSet.Add(recordFields);
                if (alignColumnCounts && count.HasValue && recordFields.Count != count)
                {
                    errors.Add("Column count among rows are different");
                }
                else
                {
                    count = recordFields.Count;
                }
                if (!allowEmptyFields && recordFields.Any(String.IsNullOrEmpty))
                {
                    errors.Add("Empty record found ");
                }
            }
            return errors.Count == 0 ? dataSet : null;
        }
    }
}