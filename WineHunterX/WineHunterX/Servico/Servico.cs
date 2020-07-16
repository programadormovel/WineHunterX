using AngleSharp;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using AngleSharp.Services.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WineHunterX.Model;
using AngleSharp.Dom;
using AngleSharp.Network;
using System.Threading;
using Plugin.Connectivity;

namespace WineHunterX.Servico
{
    public class Servico
    {
        public static readonly Encoding Utf8 = new UTF8Encoding(false);
        //private static string URLVinhos = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
        //private static string URLPesquisa = "https://www.google.com.br/search?q=preco+de+vinho+";
        //public static readonly string GOOGLE_SEARCH_URL = "https://www.google.com/search";
        public static String searchURL = null;
        public static IDocument conteudo = null;

        //public static List<VinhoBuscado> GetVinhos()
        //{
        //    WebClient wc = new WebClient();
        //    string conteudo = wc.DownloadString(URLVinhos);

        //    //return JsonConvert.DeserializeObject<List<Estado>>(conteudo);
        //    return null;

        //}

        //public static async Task<List<VinhoBuscado>> BuscarVinhosAsync(string mensagem)
        //{
        //    URLPesquisa = URLPesquisa + mensagem
        //    + "&safe=active&rlz=1C1CHBF_enBR797BR798&source=lnms&tbm=shop&sa=X&ved=0ahUKEwiitZy2r7TbAhVBjpAKHXu2DbgQ_AUICigB&biw=1366&bih=662";


        //    List<VinhoBuscado> listaVinhos = new List<VinhoBuscado>();

        //    var parser = new HtmlParser();

        //    // Setup the configuration to support document loading
        //    var config = Configuration.Default.WithDefaultLoader().WithLocaleBasedEncoding();
        //    // Asynchronously get the document in a new context using the configuration
        //    var document = await BrowsingContext.New(config).OpenAsync(URLPesquisa);
        //    // This CSS selector gets the desired content
        //    var cellSelector = "h3.r > a";
        //    // Perform the query to get all cells with the content
        //    var cells = document.QuerySelectorAll(cellSelector);
        //    // We are only interested in the text - select it with LINQ
        //    var titles = cells.Select(m => m.TextContent);
           
        //    //Alimentar lista
        //    for (int i = 0; i < titles.Count(); i++)
        //    {
        //        //VinhoBuscado vinho = (VinhoBuscado)titles.ElementAt(0);

        //        listaVinhos.Add(new VinhoBuscado { Descricao = titles.ElementAt(i).ToString() });
        //    }

        //    return listaVinhos.ToList();
        //}

        public static List<VinhoBuscado> BuscarVinhos(IDocument document)
        {
            List<VinhoBuscado> listaVinhos = new List<VinhoBuscado>();

            var parser = new HtmlParser();

            //var document = parser.Parse(conteudo);

            
            //NOME DO VINHO
            // This CSS selector gets the desired content
            var cellSelector = "h3.r > a";
            // Perform the query to get all cells with the content
            var cells = document.QuerySelectorAll(cellSelector);
            // We are only interested in the text - select it with LINQ
            var titles = cells.Select(m => m.TextContent);
            // IMAGENS
            var cellsI = document.QuerySelectorAll("div.g div.pslires div.psliimg img");
            var imagens = cellsI.Select(m => m.GetAttribute("src"));

            //Alimentar lista
            for (int i = 0; i < titles.Count(); i++)
            {
                //var cellImagem = document.Body.QuerySelector("div.g div.pslires div.psliimg img");
                //IMAGEM DO VINHO
                ////var cellImagem = cells.ElementAt(i).QuerySelector("div.g div.pslires div.psliimg img");
                //var imagem = cellImagem.GetAttribute("src").ToString();
                //var imagem = imagens.ElementAt(i).ToString();

                listaVinhos.Add(new VinhoBuscado { Descricao = titles.ElementAt(i).ToString(),
                                                    Imagem = imagens.ElementAt(i).ToString(),
                                                    Id =i});
            }

            return listaVinhos;
        }

        public static List<VinhoLoja> BuscarLojas(IDocument document)
        {
            List<VinhoLoja> listaLojas = new List<VinhoLoja>();

            var parser = new HtmlParser();

            //var document = parser.Parse(conteudo);

            //NOME DO VINHO
            // This CSS selector gets the desired content
            var cellSelector = "h3.r > a";
            // Perform the query to get all cells with the content
            var cells = document.QuerySelectorAll(cellSelector);
            // We are only interested in the text - select it with LINQ
            var titles = cells.Select(m => m.TextContent);
            // LINKS
            var links = cells.Select(m => m.GetAttribute("href"));
            // IMAGENS
            var cellsI = document.QuerySelectorAll("div.g div.pslires div.psliimg img");
            var imagens = cellsI.Select(m => m.GetAttribute("src"));
            // LOJAS
            var cellsL = document.QuerySelectorAll("div.g div.pslires div.A8OWCb");
            var lojas = cellsL.Select(m => m.TextContent);
            // VALORES
            var cellsV = document.QuerySelectorAll("div.g div.pslires div.A8OWCb div b");
            var valores = cellsV.Select(m => m.TextContent);

            //Alimentar lista
            for (int i = 0; i < titles.Count(); i++)
            {
                //var cellImagem = document.Body.QuerySelector("div.g div.pslires div.psliimg img");
                //IMAGEM DO VINHO
                ////var cellImagem = cells.ElementAt(i).QuerySelector("div.g div.pslires div.psliimg img");
                //var imagem = cellImagem.GetAttribute("src").ToString();
                //var imagem = imagens.ElementAt(i).ToString();

                var loja = lojas.ElementAt(i).ToString()
                    .Replace("R", "").Replace("$", "").Replace(",", "")
                        .Replace("[0-9]", "").ToString().Trim();
                string lojaSemNumero = null;
                int x = 0;
                while (loja.ElementAt(x).ToString().Equals("0") ||
                    loja.ElementAt(x).ToString().Equals("1") ||
                    loja.ElementAt(x).ToString().Equals("2") ||
                    loja.ElementAt(x).ToString().Equals("3") ||
                    loja.ElementAt(x).ToString().Equals("4") ||
                    loja.ElementAt(x).ToString().Equals("5") ||
                    loja.ElementAt(x).ToString().Equals("6") ||
                    loja.ElementAt(x).ToString().Equals("7") ||
                    loja.ElementAt(x).ToString().Equals("8") ||
                    loja.ElementAt(x).ToString().Equals("9") ||
                    loja.ElementAt(x).ToString().Equals(".") )
                {
                    lojaSemNumero = loja.Substring(x + 1);
                    x += 1;
                }

                if (!lojaSemNumero.Substring(0, 2).ToString().Equals("em"))
                {
                    listaLojas.Add(new VinhoLoja
                    {
                        Descricao = titles.ElementAt(i).ToString(),
                        //Descricao = valores.ElementAt(i).ToString().Replace("&nbsp;", "").Replace(",", ".").Replace("<b>", "").Replace("</b>", "").Replace("R$", "").Replace("[a-z],[A-Z]", ""),
                        Imagem = imagens.ElementAt(i).ToString(),
                        Loja = lojaSemNumero,
                        //.Replace("&nbsp;", "").Replace(",", ".")
                        //        .Replace("<b>", "").Replace("</b>", "").Replace("R$", "")
                        //        .Replace(".", ",").Replace("[0-9],[0-9]", "").Replace("0", "")
                        //        .Substring(4).Replace(",","."),
                        Valor = valores.ElementAt(i).ToString().Replace("&nbsp;", "")
                    .Replace(",", ".").Replace("<b>", "").Replace("</b>", "")
                    .Replace("R$", "").Replace("[a-z],[A-Z]", "").Replace("?", ""),
                        Link = links.ElementAt(i).ToString(),
                        Id = i
                    });
                }
            }

            return listaLojas;
        }

        public static async Task<List<VinhoBuscado>> MyAsyncTaskAsync(string url)
        {
            var parser = new HtmlParser();
            HttpClient cliente = new HttpClient()
            {
                MaxResponseContentBufferSize = 999999999999999999
            };
            string resultado = await cliente.GetStringAsync(url);

            IDocument doc = await parser.ParseAsync(resultado);
            //IDocument doc = await GetStringAsync(url);

            return DisplayResults(doc);
        }

        //public static async Task<string> GetStringAsync(string url)
        public static async Task<IDocument> GetStringAsyncMANUAL(string url)
        {
            // The downloaded resource ends up in the variable named content.  
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL.  
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for  
            // the response.                  
            using (WebResponse response = await webReq.GetResponseAsync())

            // The previous statement abbreviates the following two statements.  

            //Task<WebResponse> responseTask = webReq.GetResponseAsync();  
            //using (WebResponse response = await responseTask)  
            {
                // Get the data stream that is associated with the specified url.  
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.   
                    await responseStream.CopyToAsync(content);

                    // The previous statement abbreviates the following two statements.  

                    // CopyToAsync returns a Task, not a Task<T>.  
                    //Task copyTask = responseStream.CopyToAsync(content);  

                    // When copyTask is completed, content contains a copy of  
                    // responseStream.  
                    //await copyTask;  
                }
            }

            
            // Setup the configuration to support document loading
            //var config = Configuration.Default.WithDefaultLoader();

            ////var context = BrowsingContext.New(Configuration.Default.SetCulture("pt-BR").WithLocaleBasedEncoding());
            // Asynchronously get the document in a new context using the configuration
            //var document = await BrowsingContext.New(config).OpenAsync(url);
            ////var document = await context.OpenAsync(url);

           

            var context = BrowsingContext.New(new Configuration().WithDefaultLoader().SetCulture("pt-BR").WithLocaleBasedEncoding());
            var document = await context.OpenAsync(Url.Create(url));

            //var context2 = BrowsingContext.New(Configuration.Default.SetCulture("pt-BR").WithLocaleBasedEncoding());
            //var document2 = await context2.OpenAsync(__resp => __resp.Header("Content-Type", "text/html; charset=utf-8")
            //        .Address(url).Content(document.ToHtml()));

            //var content = Helper.StreamFromBytes(raw);
            var config = Configuration.Default.SetCulture("pt-BR").WithLocaleBasedEncoding();
            var document2 = await BrowsingContext.New(config).OpenAsync(res =>
                res.Content(document.ToHtml()).
                    Header(HeaderNames.ContentType, "text/html; charset=utf-8"));



            //WebClient wc = new WebClient();
            //wc.Encoding = System.Text.Encoding.ASCII;
            //string content2 = wc.DownloadString(url);

            // Return the result as a byte array.  
            //htmlEditor.Text = content2.ToString();
            //return content2.ToString();
            return document2;
        }


        public static List<VinhoBuscado> DisplayResults(IDocument content)
        {
            List<VinhoBuscado> ListaVinhosEncontrados = BuscarVinhos(content);

            //htmlEditor.Text = ListaVinhosEncontrados.ElementAt(0).Imagem;

            //ListaVinhos.ItemsSource = ListaVinhosEncontrados;

            return ListaVinhosEncontrados;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////q

        public static async Task<List<VinhoLoja>> MyAsyncTaskAsync2(string url)
        {

            var parser = new HtmlParser();
            HttpClient cliente = new HttpClient();
            string resultado = await cliente.GetStringAsync(url);

            IDocument doc = await parser.ParseAsync(resultado);
            //IDocument doc = await GetStringAsync(url);

            return DisplayResults2(doc);

            //IDocument doc = await GetStringAsync2MANUAL(url);

            //return DisplayResults2(doc);
        }

        public static async Task<IDocument> GetStringAsync2MANUAL(string url)
        {
            //// The downloaded resource ends up in the variable named content.  
            //var content = new MemoryStream();

            //// Initialize an HttpWebRequest for the current URL.  
            //var webReq = (HttpWebRequest)WebRequest.Create(url);

            //// Send the request to the Internet resource and wait for  
            //// the response.                  
            //using (WebResponse response = await webReq.GetResponseAsync())

            //// The previous statement abbreviates the following two statements.  

            ////Task<WebResponse> responseTask = webReq.GetResponseAsync();  
            ////using (WebResponse response = await responseTask)  
            //{
            //    // Get the data stream that is associated with the specified url.  
            //    using (Stream responseStream = response.GetResponseStream())
            //    {
            //        // Read the bytes in responseStream and copy them to content.   
            //        await responseStream.CopyToAsync(content);

            //        // The previous statement abbreviates the following two statements.  

            //        // CopyToAsync returns a Task, not a Task<T>.  
            //        //Task copyTask = responseStream.CopyToAsync(content);  

            //        // When copyTask is completed, content contains a copy of  
            //        // responseStream.  
            //        //await copyTask;  
            //    }
            //}

            //// Setup the configuration to support document loading
            //var config = Configuration.Default.SetCulture("pt-BR").WithLocaleBasedEncoding();
            //// Asynchronously get the document in a new context using the configuration
            //var document = await BrowsingContext.New(config).OpenAsync(url);

            var context = BrowsingContext.New(new Configuration().WithDefaultLoader().SetCulture("pt-BR").WithLocaleBasedEncoding());
            var document = await context.OpenAsync(Url.Create(url));

            //WebClient wc = new WebClient();
            //wc.Encoding = System.Text.Encoding.ASCII;
            //string content2 = wc.DownloadString(url);

            // Return the result as a byte array.  
            //htmlEditor.Text = content2.ToString();
            //return content2.ToString();
            return document;
        }

        public static List<VinhoLoja> DisplayResults2(IDocument content)
        {

            List<VinhoLoja> loja = BuscarLojas(content);

            //loja = Servico.Servico.BuscarLojas(content);

            //htmlEditorLoja.Text = loja.ElementAt(0).Descricao;

            //ListaLojas.ItemsSource = loja;
            return loja;
        }

        public static int StatusInternet()
        {
            bool sucesso = CrossConnectivity.Current.IsConnected;
            return sucesso ? 1 : 0;
        }


    }

}
