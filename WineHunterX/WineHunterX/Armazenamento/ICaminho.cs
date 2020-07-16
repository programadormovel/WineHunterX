using System;
using System.Collections.Generic;
using System.Text;

namespace WineHunterX.Armazenamento
{
    public interface ICaminho
    {
        string ObterCaminho(string NomeArquivoBanco);
    }
}
