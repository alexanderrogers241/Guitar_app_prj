package crc64ed5fb2d2f81a5bb5;


public class ChordListActivity2
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Packaged_Database.ChordListActivity2, Packaged_Database", ChordListActivity2.class, __md_methods);
	}


	public ChordListActivity2 ()
	{
		super ();
		if (getClass () == ChordListActivity2.class)
			mono.android.TypeManager.Activate ("Packaged_Database.ChordListActivity2, Packaged_Database", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
