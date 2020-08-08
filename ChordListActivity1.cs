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


        // objs
        public ArrayAdapter<String> m_ListAdapter;
        public ListView m_listview;

        // constants
        public string m_ChordGroup;
        public string[] m_Chords_1;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            m_ChordGroup = Intent.GetStringExtra("Chord");
            SetContentView(Resource.Layout.activity_chordlist1);

            // gets chord suffixes
            m_Chords_1 = Resources.GetStringArray(Resource.Array.Chords_1);


            // attaches them to the end of the note selected previously
            for (int i = 0; i < m_Chords_1.Length; i++)
            {
                m_Chords_1[i] = m_ChordGroup + m_Chords_1[i]; // adds the chord name selected to all suffixes

            }


            // passes list to next listview
            m_listview = FindViewById<ListView>(Resource.Id.CHORDLIST1);
            m_ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, m_Chords_1);
            m_listview.Adapter = m_ListAdapter;


            
            m_listview.ItemClick += OnListItemClick;



        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string c = m_Chords_1[e.Position];
            var intent = new Intent(this, typeof(ChordListActivity2));
            intent.PutExtra("Chord", c);
            // sends name to next listview
            StartActivity(intent);
        }
    }
}
