using System;
using System.Collections.Generic;
using System.Text;
using Tesseract;
using WineHunterX.Model;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;
using XLabs.Platform.Device;
//using Appson.Tesseract.ApiModel;
using Plugin.Media.Abstractions;

namespace WineHunterX.Servico
{
    public class OCR
    {
        

        private static Termo termo;
        public OCR()
        {
            
        }

        public static async System.Threading.Tasks.Task<Termo> ReconhecimentoAsync(byte[] img)
        {
            ITesseractApi tess = Resolver.Resolve<ITesseractApi>();

            var listaVinhos = "abcdefghijklmnopqrstuvxwyz ABCDEFGHIJKLMNOPQRSTUVXWYZ 0123456789";

            if (!tess.Initialized){
                await tess.Init("por");
                tess.SetWhitelist(listaVinhos);
            }

            var imageBytes = new byte[img.Length];
            //img.Position = 0;
            img.CopyTo(imageBytes, 0); // (int)img.Length);
            //img.Position = 0;

            //var tessResult = await _tesseractApi.SetImage(imageBytes);
            var tessResult = tess.SetImage(imageBytes);

            if (await tessResult)
            {

                ////_takenImage.Source = ImageSource.FromStream(() => photo.Source);
                string resposta = tess.Text;
                //_recognizedTextLabel.Text = tessResult.ToString();
                termo = new Termo { Descricao = resposta };
                
            }

            return termo;
        }

        public static async System.Threading.Tasks.Task<Termo> ReconhecimentoIOSAsync(byte[] img)
        {
            ITesseractApi api = Resolver.Resolve<ITesseractApi>();

            bool initialised = await api.Init("por");

            if(initialised==true){
                var imageBytes = new byte[img.Length];
                //img.Position = 0;
                img.CopyTo(imageBytes, 0); // (int)img.Length);
                                           //img.Position = 0;



                //Android e iOS
                bool success = await api.SetImage(imageBytes);

                //IOS
                //bool success = await api.SetImage(img.Path);

                if (success)
                {
                    string resposta = api.Text;
                    termo = new Termo { Descricao = resposta };

                }
            }

            return termo;
        }


    }
}
