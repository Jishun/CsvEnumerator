using System.Collections;
using System.Collections.Generic;
using DotNetUtils;

namespace CsvEnumerator
{
    public class CsvFieldEnumerator : IEnumerator<string>
    {
        private readonly ISeekable _inputStream;
        private readonly int _position;

        public string Current { get; private set; }
        public bool ReachedEnd { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        public CsvFieldEnumerator(ISeekable inputStream)
        {
            _inputStream = inputStream;
            _position = inputStream.Position;
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (_inputStream.Eof)
            {
                ReachedEnd = true;
                return false;
            }
            var ret = _inputStream.DecodeCsvField(false);
            if (null == ret)
            {
                ReachedEnd = true;
                return false;
            }
            Current = ret;
            return true;
        }

        public void Reset()
        {
            _inputStream.Position = _position;
            Current = null;
        }
    }
}