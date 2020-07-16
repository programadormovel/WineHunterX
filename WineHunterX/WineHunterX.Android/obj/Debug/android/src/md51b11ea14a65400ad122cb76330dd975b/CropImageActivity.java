package md51b11ea14a65400ad122cb76330dd975b;


public class CropImageActivity
	extends md51b11ea14a65400ad122cb76330dd975b.MonitoredActivity
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
		mono.android.Runtime.register ("Xamarians.CropImage.Droid.CropImageActivity, Xamarians.CropImage.Droid", CropImageActivity.class, __md_methods);
	}


	public CropImageActivity ()
	{
		super ();
		if (getClass () == CropImageActivity.class)
			mono.android.TypeManager.Activate ("Xamarians.CropImage.Droid.CropImageActivity, Xamarians.CropImage.Droid", "", this, new java.lang.Object[] {  });
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
