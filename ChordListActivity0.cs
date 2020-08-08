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
        public string[] m_Chords_0;
        public ArrayAdapter<String> m_ListAdapter;
        public ListView m_listview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chordlist0);

            // get basic 7 notes chords are based off of
            m_Chords_0 = Resources.GetStringArray(Resource.Array.Chords_0);
            // pass list to listview
            m_listview = FindViewById<ListView>(Resource.Id.CHORDLIST0);
            m_ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, m_Chords_0);
            m_listview.Adapter = m_ListAdapter;


            m_listview.ItemClick += OnListItemClick;



        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string c = m_Chords_0[e.Position];
            var intent = new Intent(this, typeof(ChordListActivity1));
            intent.PutExtra("Chord", c);
            // passes note to next activity
            StartActivity(intent);
        }
    }
}