using System;
using System.IO;
using Android.Graphics;

namespace WineHunterX.Droid
{
	public static class Extensions
	{
		public static byte[] BitmapToBytes(this Bitmap myBitmapImage)
		{
            try
            {
                var ms = new MemoryStream();
                myBitmapImage.Compress(Bitmap.CompressFormat.Png, 0, ms);
                var imageByteArray = ms.ToArray();
                return imageByteArray;
            }
            catch (Exception){
                return null;
            }
			
		}
	}
}