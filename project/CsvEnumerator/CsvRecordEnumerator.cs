using System.Collections;
using System.Collections.Generic;
using DotNetUtils.Interface;

namespace CsvEnumerator
{
    public class CsvRecordEnumerator : IEnumerator<CsvRecordEnumerable>
    {
        private readonly ISeekable _inputStream;
        private readonly int _position;

        public CsvRecordEnumerable Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        public CsvRecordEnumerator(ISeekable inputStream)
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
                return false;
            }
            if (Current == null)
            {
                Current = new CsvRecordEnumerable(_inputStream);
            }
            Current.SetNextStart();
            return true;
        }

        public void Reset()
        {
            _inputStream.Position = _position;
        }
    }
}
