using System;
using System.Collections.Generic;
using System.IO;

namespace BattleRoyalle.File
{
    public class FileHelper
    {
        static string caminhoArquivoLog = AppDomain.CurrentDomain.BaseDirectory + @"log\";

        public static void Write(List<string> fileNameList, String log)
        {
            foreach (var fileName in fileNameList)
                Write(fileName, log);
        }
        public static void Write(string fileName, string log)
        {
            Directory.CreateDirectory(caminhoArquivoLog);
            string pathLogFile = caminhoArquivoLog + fileName + ".txt";

            var stw = new StreamWriter(pathLogFile, true);
            stw.Write(log);
            stw.Close();
        }
    }
}
