package crc64ed5fb2d2f81a5bb5;


public class ChordListActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Packaged_Database.ChordListActivity, Packaged_Database", ChordListActivity.class, __md_methods);
	}


	public ChordListActivity ()
	{
		super ();
		if (getClass () == ChordListActivity.class)
			mono.android.TypeManager.Activate ("Packaged_Database.ChordListActivity, Packaged_Database", "", this, new java.lang.Object[] {  });
	}

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
