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
    [Activity(Label = "ChordListActivity2")]
    public class ChordListActivity2 : Activity
    {
        // Second level ::: we actually quary the database for the chord obj

        public IList<Chord> m_chord_list;
        public SQlightDB_Class m_db_obj;
        public ChordListAdapter m_chordlist_adp;
        public ListView m_ChordListView = null;
        public string m_dbfilename;
        public string m_ChordGrpCat; // chord group category is withen the chord group
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chordlist2);
            //get the chord selected on previous listview
            m_ChordGrpCat = Intent.GetStringExtra("Chord");
            //set up datbase
            m_dbfilename = Resources.GetString(Resource.String.database_name);
            m_db_obj = new SQlightDB_Class(m_dbfilename);


            //get list from database matching our chord category
            m_chord_list = m_db_obj.GetChords(m_ChordGrpCat).ToList();



            // pass the list to the new listview
            m_chordlist_adp = new ChordListAdapter(this, m_chord_list);
            m_ChordListView = FindViewById<ListView>(Resource.Id.CHORDLIST2);
            m_ChordListView.Adapter = m_chordlist_adp;


            m_ChordListView.ItemClick += OnListItemClick;


        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            Chord c = m_chord_list[e.Position];
            var intent = new Intent(this, typeof(DrawChordActivity));
            intent.PutExtra("Chord", c.Frets);
            intent.PutExtra("Chord_pos", c.Position);
            intent.PutExtra("Chord_name", c.Name);
            // sends the selected chord information to be drawn
            StartActivity(intent);
        }

    }
}