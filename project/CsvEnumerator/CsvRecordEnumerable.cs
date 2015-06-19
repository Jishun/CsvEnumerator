using System.Collections;
using System.Collections.Generic;
using DotNetUtils.Interface;

namespace CsvEnumerator
{
    public class CsvRecordEnumerable : IEnumerable<string>
    {
        private readonly ISeekable _inputStream;
        private int _position;
        private CsvFieldEnumerator _currentEnumerator;
        public CsvRecordEnumerable(ISeekable inputStream)
        {
            _inputStream = inputStream;
            _position = inputStream.Position;
        }
        public IEnumerator<string> GetEnumerator()
        {
            _inputStream.Position = _position;
            return _currentEnumerator = new CsvFieldEnumerator(_inputStream);
        }

        public void SetNextStart()
        {
            while (_currentEnumerator != null && !_currentEnumerator.ReachedEnd)
            {
                _currentEnumerator.MoveNext();
            }
            _position = _inputStream.Position;
            _currentEnumerator = new CsvFieldEnumerator(_inputStream);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}