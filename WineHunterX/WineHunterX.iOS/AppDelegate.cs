using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Foundation;
using Tesseract;
using Tesseract.iOS;
using TinyIoC;
using UIKit;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;
using XLabs.Ioc.TinyIOC;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace WineHunterX.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();

            // Code for starting up the Xamarin Test Cloud Agent
            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            //FFImageLoading
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            //CropImage
            Xamarians.CropImage.iOS.CropImageServiceIOS.Initialize();

            //Tesseract OCR
            var containerBuilder = new Autofac.ContainerBuilder();

            containerBuilder.RegisterType<MediaPicker>().As<IMediaPicker>();
            containerBuilder.RegisterType<TesseractApi>().As<ITesseractApi>();

            Resolver.SetResolver(new AutofacResolver(containerBuilder.Build()));

            //Tesseract TinyIoC
            //var container = TinyIoCContainer.Current;
            //container.Register<IDevice>(AppleDevice.CurrentDevice);
            //container.Register<ITesseractApi>((cont, parameters) =>
            //{
            //    return new TesseractApi();
            //});
            //Resolver.SetResolver(new TinyResolver(container));

            LoadApplication(new App());

            //Inicializa o Zxing no iOS
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
