﻿using System.Collections;
using System.Collections.Generic;

namespace CsvEnumerator
{
    public class CsvEnumerable : IEnumerable<CsvRecordEnumerable>
    {
        private readonly int _position;
        private readonly ISeekableString _inputStream;
        public CsvEnumerable(ISeekableString inputStream)
        {
            _position = inputStream.Position;
            _inputStream = inputStream;
        }
        public IEnumerator<CsvRecordEnumerable> GetEnumerator()
        {
            _inputStream.Position = _position;
            return new CsvRecordEnumerator(_inputStream);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}