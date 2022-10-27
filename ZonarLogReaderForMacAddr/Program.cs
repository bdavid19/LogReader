using System;

namespace ZonarLogReaderForMacAddr
{
    class Program
    {
        static void Main(string[] args)
        {
            fileReading fileRead = new fileReading();
            fileRead.ReadFiles();

        }
    }
}
