using Android.App;
using Android.Content.PM;
using Android.OS;
using Tesseract.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Device;
using XLabs.Ioc;
using Autofac;
using Android.Content;
using Android.Util;
using Tesseract;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using FFImageLoading.Forms.Droid;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TinyIoC;
using XLabs.Ioc.TinyIOC;

//[assembly: UsesFeature("android.hardware.camera", Required = false)]
//[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
namespace WineHunterX.Droid
{
    [Activity(Label = "WineHunterX", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //FFImageLoading
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            //Plugin Media
            //await CrossMedia.Current.Initialize();
            //FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            //CropImage
            Xamarians.CropImage.Droid.CropImageServiceAndroid.Initialize(this);

            //Tesseract OCR
            //var containerBuilder = new ContainerBuilder();

            //containerBuilder.Register(c => this).As<Context>();
            //containerBuilder.RegisterType<MediaPicker>().As<IMediaPicker>();
            //containerBuilder.RegisterType<TesseractApi>()
            //    .As<ITesseractApi>()
            //    .WithParameter((pi, c) => pi.ParameterType == typeof(AssetsDeployment),
            //    (pi, c) => AssetsDeployment.OncePerInitialization);

            //Resolver.SetResolver(new AutofacResolver(containerBuilder.Build()));

            //Tesseracti TinyIoC
            var container = TinyIoCContainer.Current;
            container.Register<IDevice>(AndroidDevice.CurrentDevice);
            container.Register<ITesseractApi>((cont, parameters) =>
            {
                return new TesseractApi(ApplicationContext, Tesseract.Droid.AssetsDeployment.OncePerInitialization);
            });
            Resolver.SetResolver(new TinyResolver(container));

            

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, global::Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

