using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WineHunterX.Model;
using WineHunterX.Servico;
using WineHunterX.View.Util;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace WineHunterX.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SamplePage : ContentPage
    {
        private ImageSource AvatarImageSource;
        private readonly IDevice _device;
        private Image img;
        private Button photo;
        private ActivityIndicator Indicador;
        public SamplePage()
        {
            _device = Resolver.Resolve<IDevice>();

            img = new Image()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Gray,
            };
            photo = new Button()
            {
                Text = "Clique Aqui Para Tirar Foto de um Novo Rótulo",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromHex("#853643")
            };
            Indicador = new ActivityIndicator()
            {
                Color = Color.LightGoldenrodYellow,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            photo.Clicked += (object sender, EventArgs e) =>
            {
                BtnPhotoClicked(sender, e);
            };

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { photo, Indicador, img },
                BackgroundColor = Color.White
            };

            ScrollView scroll = new ScrollView()
            {
                Content = stack
            };

            Content = scroll;
            //Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Padding = new Thickness(0, 20, 0, 0);
                    break;
                case Device.Android:
                    Padding = new Thickness(0, 0, 0, 0);
                    break;
                case Device.UWP:
                    Padding = new Thickness(0, 0, 0, 0);
                    break;
            }

            try
            {
                BtnPhotoClicked(this, new EventArgs());
            }
            catch (Exception e)
            {
                DisplayAlert("Erro Camera", "Erro ao carregar camera: " + e.Message, "Cancel");
            }
        }

        private void ChamadaBotaoAsync()
        {
            BtnPhotoClicked(this, new EventArgs());
        }

        public async void BtnPhotoClicked(object sender, EventArgs args)
        {
            try
            {
                IList<String> buttons = new List<String>();
                buttons.Add(ChooseImageFrom.Galeria.ToString());
                buttons.Add(ChooseImageFrom.Camera.ToString());

                var action = await DisplayActionSheet("Escolha da foto de ", "Cancel", null, buttons.ToArray());

                if (action == ChooseImageFrom.Camera.ToString())
                {
                    photo.IsVisible = false;
                    Indicador.IsRunning = true;
                    await CrossMedia.Current.Initialize();
                    Indicador.IsRunning = false;
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        return;
                    }
                    //var mediaFile = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);
                    Plugin.Media.Abstractions.MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    //XLabs.Platform.Services.Media.MediaFile file = await _device.MediaPicker.TakePhotoAsync(new CameraMediaStorageOptions
                    {
                        SaveToAlbum = true,
                        DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                        Name = "OCRImage",
                        AllowCropping = true,
                        RotateImage = true
                    });

                    if (file == null)
                    {
                        return;
                    }

                    byte[] cropedBytes = await CrossXMethod.Current.CropImageFromOriginalToBytes(file.Path);
                    Indicador.IsRunning = true;
                    if (cropedBytes != null)
                    {
                        AvatarImageSource = ImageSource.FromStream(() =>
                        {
                            var cropedImage = new MemoryStream(cropedBytes);
                            file.Dispose();
                            return cropedImage;
                        });
                        img.Source = AvatarImageSource;

                        Termo t = await OCR.ReconhecimentoAsync(cropedBytes);
                        Indicador.IsRunning = false;
                        await Navigation.PushAsync(new ListaVinho(t));
                    }
                    else
                    {
                        file.Dispose();
                    }
                }
                else
                {
                    photo.IsVisible = false;
                    Indicador.IsRunning = true;
                    await CrossMedia.Current.Initialize();
                    Indicador.IsRunning = false;
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        return;
                    }
                    Plugin.Media.Abstractions.MediaFile file = await CrossMedia.Current.PickPhotoAsync();

                    if (file == null)
                    {
                        return;
                    }
                   
                    byte[] cropedBytes = await CrossXMethod.Current.CropImageFromOriginalToBytes(file.Path);
                    Indicador.IsRunning = true;
                    if (cropedBytes != null)
                    {
                        AvatarImageSource = ImageSource.FromStream(() =>
                        {
                            var cropedImage = new MemoryStream(cropedBytes);
                            file.Dispose();
                            return cropedImage;
                        });

                        img.Source = AvatarImageSource;
                        Termo t = new Termo();
                        switch (Device.RuntimePlatform)
                        {
                            case Device.iOS:
                                //t = await OCR.ReconhecimentoIOSAsync(file);
                                t = await OCR.ReconhecimentoAsync(cropedBytes);
                                break;
                            case Device.Android:
                                t = await OCR.ReconhecimentoAsync(cropedBytes);
                                break;
                            case Device.UWP:
                                t = await OCR.ReconhecimentoAsync(cropedBytes);
                                break;
                        }

                        Indicador.IsRunning = false;
                        await Navigation.PushAsync(new ListaVinho(t));
                    }
                    else
                    {
                        file.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Erro Camera", "Erro na captura ou carregamento da foto: " + e.Message, "Cancel");
            }
            finally
            {
                photo.IsVisible = true;
            }
        }
    }

    public enum ChooseImageFrom
    {
        Camera,
        Galeria
    }
}
