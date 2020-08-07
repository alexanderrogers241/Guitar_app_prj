using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;
using Android;
using Packaged_Database;
using Android.Views;

namespace Packaged_Database
{
	public class ChordListAdapter : BaseAdapter<Chord> {
		Activity context = null;
		IList<Chord> Chord = new List<Chord>();
		
		public ChordListAdapter (Activity context, IList<Chord> Chord) : base ()
		{
			this.context = context;
			this.Chord = Chord;
		}
		
		public override Chord this[int position]
		{
			get { return Chord[position]; }
		}
		
		public override long GetItemId (int position)
		{
			return position;
		}
		
		public override int Count
		{
			get { return Chord.Count; }
		}
		
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			 var chord_at_pos = Chord[position];
			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()

			View view = convertView;

			if (view == null)
            {
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, parent, false);

            }

			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = chord_at_pos.Name ;
			view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = ("Position: " + (chord_at_pos.Position + 1));

			//Finally return the view
			return view;
		}
	}
}