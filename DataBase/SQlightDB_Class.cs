using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SQLite;

namespace Packaged_Database
{
    public class SQlightDB_Class
    {


        static object locker = new object();

        private string mdbPath;

        public SQlightDB_Class(string dbName)
        {
            //Creating db if it doesnt already exist
            mdbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),dbName);
            var db = new SQLiteConnection(mdbPath);
            db.CreateTable<Chord> (); 
        }

        public string path
        {
            get { return mdbPath; }
            set { mdbPath = value; }
        }
        public IEnumerable<Chord> GetChords()
        {

            var db = new SQLiteConnection(path);

            lock (locker)
            {
                return (from i in db.Table<Chord>() select i).ToList();
            }
        }
        public Chord GetChord(int id)
        {
            var db = new SQLiteConnection(path);

            lock (locker)
            {
                return db.Table<Chord>().FirstOrDefault(x => x.ID == id);
            }
        }


        public int SaveChord(Chord item)
        {

            var db = new SQLiteConnection(path);
            lock (locker)
            {
                if (item.ID != 0)
                {
                    db.Update(item);
                    return item.ID;
                }
                else
                {
                    return db.Insert(item);
                }
            }

        }


        public int DeleteChord(Chord chord)
        {
            var db = new SQLiteConnection(path);
            

                lock (locker)
            {
                return db.Delete<Chord>(chord.ID);
            }
        }
        public void PrintDataBase()
        {


            lock (locker)
            {
                using (var db = new SQLite.SQLiteConnection(mdbPath))
                {
                    Console.WriteLine("PRINTING DATABASE IF POSSIBLE");
                    var table = db.Table<Chord>();

                    foreach (var s in table)
                    {
                        Console.WriteLine(s.ID + " " + s.Name);
                    }

                }
            }

        }

       














    }
}