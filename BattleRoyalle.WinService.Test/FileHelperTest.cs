using BattleRoyalle.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace BattleRoyalle.WinService.Test
{
    public class FileHelperTest
    {
        [Fact]
        public void deve_CriarArquivoEEscreverNele_Necessario()
        {
            string caminhoArquivoLog = AppDomain.CurrentDomain.BaseDirectory + @"log\";

            if (Directory.Exists(caminhoArquivoLog))
                Directory.Delete(caminhoArquivoLog, true);

            FileHelper.Write("teste", "TesteDeCriacao");

            Assert.True(Directory.Exists(caminhoArquivoLog));

        }

    }
}
