using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Packaged_Database
{
    [Application]
    public class YourAndroidApp : Application
    {
        public override void OnCreate()
        {
            base.OnCreate();
            // get  db file name from strings and get path
            string dbfilename = Resources.GetString(Resource.String.database_name);
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, dbfilename);

            //
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Console.WriteLine("Data path:" + path);
            var dbFile = Path.Combine(docFolder, dbfilename); // FILE NAME TO USE WHEN COPIED


            if (!System.IO.File.Exists(dbFile))
            {
                var s = Resources.OpenRawResource(Resource.Raw.ChordDatabase);  // DATA FILE RESOURCE ID
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
            }
        }
        // readStream is the stream you need to read
        // writeStream is the stream you want to write to
        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}