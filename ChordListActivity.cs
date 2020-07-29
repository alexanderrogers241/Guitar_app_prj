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
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Packaged_Database
{
    [Activity(Label = "ChordListActivity")]
    public class ChordListActivity : Activity
    {
        public IList<Chord> chord_list;
        public SQlightDB_Class db_obj;
        public ChordListAdapter chordlist_adp;
        public ListView ChordListView = null;
        public string dbfilename;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chordlist);
            dbfilename = Resources.GetString(Resource.String.database_name);
            //Set up objs
            db_obj = new SQlightDB_Class(dbfilename);
            chord_list = db_obj.GetChords().ToList();
            chordlist_adp = new ChordListAdapter(this, chord_list);
            ChordListView = FindViewById<ListView>(Resource.Id.CHORDLIST);
            ChordListView.Adapter = chordlist_adp;
            ChordListView.ItemClick += OnListItemClick;  // to be defined
            // Set up events

        }
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            Chord c = chord_list[e.Position];
            var intent = new Intent(this, typeof(DrawChordActivity));
            intent.PutExtra("Chord", c.Frets);
            intent.PutExtra("Chord_pos", c.Position.ToString());
            StartActivity(intent);

        }
    }
}