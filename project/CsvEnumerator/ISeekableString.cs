using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvEnumerator
{
    public interface ISeekableString
    {
        string Source { get; }
        int Position { get; set; }
        int Line { get; }
        int Column { get; }
        int PreviousPosition { get; }
        int PreviousLine { get; }
        int PreviousColumn { get; }
        bool Eof { get; }
        int Length { get; }
        string Left { get; }

        ISeekableString Substring(int startIndex, int length);
        ISeekableString Substring(int startIndex);
        bool StartsWith(string match);
        void Back(int count = 1);
        int PeekChar();
        int ReadChar();
        string ReadTo(string match, bool trimPattern = true);
        string ReadTo(bool trimPattern, string escape, params string[] terminitors);
        string ReadTo(bool trimPattern, out string matched, string escape, params string[] terminitors);
    }
}
