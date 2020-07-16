using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WineHunterX.Model
{
    public class VinhoBuscado
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }

        public static explicit operator VinhoBuscado(string v)
        {
            throw new NotImplementedException();
        }
    }
}
