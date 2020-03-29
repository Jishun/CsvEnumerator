using System;
using System.Collections.Generic;
using CsvEnumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var file = @"col1, col2, col3
v1, v2, v3";
            var logs = new List<string>();
            var str = new SeekableString(file);
            var csv = str.ValidateCsv(logs, true, false);
            if (logs.Count == 0)
            {
                foreach (var line in csv)
                {
                    foreach (var cell in line)
                    {
                        //do someting with the string list
                    }
                }
            }
            else
            {
                //csv is not valid
            }
        }
    }
}
