using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Packaged_Database
{
    [Activity(Label = "ChordListActivity0")]
    public class ChordListActivity0 : Activity
    {
        public string[] Chords_0;
        public ArrayAdapter<String> ListAdapter;
        public ListView listview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_chordlist0);
            Chords_0 = Resources.GetStringArray(Resource.Array.Chords_0);

            listview = FindViewById<ListView>(Resource.Id.CHORDLIST0);
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Chords_0);
            listview.Adapter = ListAdapter;
            listview.ItemClick += OnListItemClick;



        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string c = Chords_0[e.Position];
            var intent = new Intent(this, typeof(ChordListActivity1));
            intent.PutExtra("Chord", c);
            StartActivity(intent);
        }
    }
}