using System;
using System.Collections.Generic;
using System.Text;

namespace WineHunterX.Model
{
    public class Vinho
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Vinicula { get; set; }
        public string Fornecedor { get; set; }
        public string Link { get; set; }
        public int Ano { get; set; }
        public double Volume { get; set; }
        public string Valor { get; set; }
        public string CaminhoFoto { get; set; }
        public string FotoTexto { get; set; }
        public Byte[] Foto { get; set; }
        public double Avaliacao { get; set; }
        public double AvaliacaoGeral { get; set; }

    }
}
