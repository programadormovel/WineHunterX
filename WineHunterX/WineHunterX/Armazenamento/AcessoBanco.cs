using System;
using System.Collections.Generic;
using SQLite;
using System.Text;
using WineHunterX.Model;
using Xamarin.Forms;
using System.Linq;

namespace WineHunterX.Armazenamento
{
    public class AcessoBanco
    {
        private SQLiteConnection _conexao;

        public AcessoBanco()
        {
            var dep = DependencyService.Get<ICaminho>();
            string caminho = dep.ObterCaminho("database.sqlite");

            _conexao = new SQLiteConnection(caminho);
            _conexao.CreateTable<Termo>();
            _conexao.CreateTable<VinhoLoja>();
        }

        //Consultar
        public List<Termo> Consultar()
        {
            return _conexao.Table<Termo>().ToList();
        }
        public List<VinhoLoja> ConsultarVinhoLoja()
        {
            return _conexao.Table<VinhoLoja>().ToList();
        }
        //Pesquisar
        public List<Termo> Pesquisar(string palavra)
        {
            return _conexao.Table<Termo>().Where(a => a.Descricao.Contains(palavra.ToUpper())).ToList();
        }
        public List<VinhoLoja> PesquisarVinhoLoja(string palavra)
        {
            return _conexao.Table<VinhoLoja>().Where(a => a.Descricao.Contains(palavra.ToUpper())).ToList();
        }
        //Obter[Tabela]PorId
        public Termo ObterTermoPorId(int id)
        {
            return _conexao.Table<Termo>().Where(a => a.Id == id).FirstOrDefault();
        }
        public VinhoLoja ObterVinhoLojaPorId(int id)
        {
            return _conexao.Table<VinhoLoja>().Where(a => a.Id == id).FirstOrDefault();
        }
        //Cadastro
        public void Cadastro(Termo termo)
        {
            _conexao.Insert(termo);
        }
        public void CadastroVinhoLoja(VinhoLoja vinhoLoja)
        {
            _conexao.Insert(vinhoLoja);
        }
        //Atualizacao
        public void Atualizacao(Termo termo)
        {
            _conexao.Update(termo);
        }
        public void AtualizacaoVinhoLoja(VinhoLoja vinhoLoja)
        {
            _conexao.Update(vinhoLoja);
        }
        //Exclusao
        public void Exclusao(Termo termo)
        {
            _conexao.Delete(termo);
        }
        public void ExclusaoVinhoLoja(VinhoLoja vinhoLoja)
        {
            _conexao.Delete(vinhoLoja);
        }


    }
}
