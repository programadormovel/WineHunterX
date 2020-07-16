using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WineHunterX.Armazenamento;
using Xamarin.Forms;
using System.IO;
using WineHunterX.UWP.Armazenamento;
using Windows.Storage;

[assembly: Dependency(typeof(Caminho))]
namespace WineHunterX.UWP.Armazenamento
{
    public class Caminho : ICaminho
    {
        public string ObterCaminho(string NomeArquivoBanco)
        {
            string caminhoDaPasta = ApplicationData.Current.LocalFolder.Path;

            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);

            return caminhoBanco;
        }
    }
}