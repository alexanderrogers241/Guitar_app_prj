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
using SQLite;

namespace Packaged_Database
{
    [Table("C_Chords")]
    public class Chord
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull, Column("ID")]
        public int ID { get; set; }

        [NotNull, Column("Name")]
        public string Name { get; set; }

        [NotNull, Column("Position")]
        public int Position { get; set; }

        [NotNull, Column("Frets")]
        public string Frets { get; set; }


        [NotNull, Column("Frets_f")]
        public string Frets_f { get; set; }


        [NotNull, Column("SPECIAL")]
        public int SPECIAL{ get; set; }






    }
}