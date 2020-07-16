using System;
using System.Collections.Generic;
using System.Text;

namespace WineHunterX.Model
{
    public class VinhoLoja
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Loja { get; set; }
        public string Link { get; set; }
        public string Valor { get; set; }
        public string Imagem { get; set; }
        public double Avaliacao { get; set; }
        public string Opiniao { get; set; }
    }
}
