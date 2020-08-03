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
    [Activity(Label = "ChordListActivity1")]
    public class ChordListActivity1 : Activity
    {
        public string[] Chords_1;
        public ArrayAdapter<String> ListAdapter;
        public ListView listview;
        public string m_ChordGroup;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            m_ChordGroup = Intent.GetStringExtra("Chord");
            SetContentView(Resource.Layout.activity_chordlist1);


            Chords_1 = Resources.GetStringArray(Resource.Array.Chords_1);

            for (int i = 0; i < Chords_1.Length; i++)
            {
                Chords_1[i] = m_ChordGroup + Chords_1[i]; // adds the chord name selected to all suffixes

            }

            listview = FindViewById<ListView>(Resource.Id.CHORDLIST1);
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Chords_1);
            listview.Adapter = ListAdapter;
            listview.ItemClick += OnListItemClick;



        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string c = Chords_1[e.Position];
            var intent = new Intent(this, typeof(ChordListActivity2));
            intent.PutExtra("Chord", c);
            StartActivity(intent);
        }
    }
}
