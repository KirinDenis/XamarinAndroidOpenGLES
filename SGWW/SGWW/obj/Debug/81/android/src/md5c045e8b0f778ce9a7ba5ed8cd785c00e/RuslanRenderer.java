package md5c045e8b0f778ce9a7ba5ed8cd785c00e;


public class RuslanRenderer
	extends md5c045e8b0f778ce9a7ba5ed8cd785c00e.Renderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SGWW.RuslanRenderer, SGWW", RuslanRenderer.class, __md_methods);
	}


	public RuslanRenderer ()
	{
		super ();
		if (getClass () == RuslanRenderer.class)
			mono.android.TypeManager.Activate ("SGWW.RuslanRenderer, SGWW", "", this, new java.lang.Object[] {  });
	}

	public RuslanRenderer (android.content.Context p0)
	{
		super ();
		if (getClass () == RuslanRenderer.class)
			mono.android.TypeManager.Activate ("SGWW.RuslanRenderer, SGWW", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
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
