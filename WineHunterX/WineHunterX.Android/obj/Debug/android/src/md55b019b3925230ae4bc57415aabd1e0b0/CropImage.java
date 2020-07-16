package md55b019b3925230ae4bc57415aabd1e0b0;


public class CropImage
	extends md55b019b3925230ae4bc57415aabd1e0b0.MonitoredActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"";
		mono.android.Runtime.register ("WineHunterX.Droid.CropImage, WineHunterX.Android", CropImage.class, __md_methods);
	}


	public CropImage ()
	{
		super ();
		if (getClass () == CropImage.class)
			mono.android.TypeManager.Activate ("WineHunterX.Droid.CropImage, WineHunterX.Android", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
