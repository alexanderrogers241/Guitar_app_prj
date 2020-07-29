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
using SQLite;

namespace Packaged_Database
{
    public class APK_SQlightDB_Class
    {

        static object locker = new object();

        private string mdbPath;
        private bool mexist = false;


        public APK_SQlightDB_Class(String APK_dbName)
        {
            // Constructor for databases within APK

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), APK_dbName); //not diretory i want
            mdbPath = dbPath;
            Console.WriteLine(dbPath);


            try
            {

                // Check if your DB has already been extracted.
                if (File.Exists(dbPath)) // not !
                {
                    using (BinaryReader br = new BinaryReader(Android.App.Application.Context.Assets.Open(APK_dbName)))
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int len = 0;
                            while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, len);
                            }
                        }
                    }
                }
            }

            catch(Exception e)
            {
                Console.WriteLine("{0}  exception caught.", e);
            }
        }

        public bool exist
        {
            get { return mexist; }
        }

        public string path
        {
            get { return mdbPath; }
            set { mdbPath = value; }
        }
        public IEnumerable<Chord> GetChords()
        {
            try
            {
                var db = new SQLiteConnection(mdbPath);

                lock (locker)
                {
                    return (from i in db.Table<Chord>() select i).ToList();
                }



            }
            catch( Exception e)
            {
                Console.WriteLine("{0}  exception caught.", e);
                    return null;
            }

        }
        public Chord GetChord(int id)
        {
            var db = new SQLiteConnection(mdbPath);

            lock (locker)
            {
                return db.Table<Chord>().FirstOrDefault(x => x.ID == id);
            }
        }


        public int SaveChord(Chord item)
        {

            var db = new SQLiteConnection(mdbPath);
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
            var db = new SQLiteConnection(mdbPath);


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