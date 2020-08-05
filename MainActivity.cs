using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Runtime.InteropServices;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Android;
using Android.Content;
using System.IO;

namespace Packaged_Database
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public SQlightDB_Class db_obj;
		public Button Button_obj;
		public SKCanvasView skiaView_obj;
		public string dbfilename;
		public string m_app_name;


		protected override void OnCreate(Bundle savedInstanceState)
		{


			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			// get  db file name from strings and get path
			m_app_name = Resources.GetString(Resource.String.app_name);
			dbfilename = Resources.GetString(Resource.String.database_name);
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



		
			// readStream is the stream you need to read
			// writeStream is the stream you want to write to
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);
			
			// Set up objects
			
			skiaView_obj = FindViewById<SKCanvasView>(Resource.Id.SKIAVIEW_MAIN);
			Button_obj = FindViewById<Button>(Resource.Id.BUTTON_CHORDLIST);
			// Set up events

			Button_obj.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(ChordListActivity0));
				StartActivity(intent);
			};

		}

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

		protected override void OnResume()
		{
			base.OnResume();

			skiaView_obj.PaintSurface += OnPaintSurface;
		}

		protected override void OnPause()
		{
			skiaView_obj.PaintSurface -= OnPaintSurface;

			base.OnPause();
		}
		// PlaceHolder Logo
		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			// the the canvas and properties
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = args.Surface.Canvas;

			// get the screen density for scaling
			var scale = Resources.DisplayMetrics.Density;
			var scaledSize = new SKSize(info.Width / scale, info.Height / scale);

			// handle the device screen density
			canvas.Scale(scale);

			// make sure the canvas is blank
			canvas.Clear(SKColors.White);

			// draw some text
			
			var textPaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				FakeBoldText = true,
				StrokeWidth = 2.2f,
				Color = SKColors.OrangeRed,
				IsAntialias = true,

			};
			var paint_red = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				StrokeWidth = 25
			};

			float textWidth = textPaint.MeasureText(m_app_name);
			textPaint.TextSize = 0.90f * scaledSize.Width *( textPaint.TextSize / textWidth);
			// Find the text bounds
			SKRect textBounds = new SKRect();
			textPaint.MeasureText(m_app_name, ref textBounds);

			// Calculate offsets to center the text on the screen
			float xText = scaledSize.Width / 2 - textBounds.MidX;
			float yText = scaledSize.Height/ 1.2f - textBounds.MidY;

			canvas.DrawText(m_app_name,xText, yText, textPaint);
			canvas.DrawCircle(scaledSize.Width / 2, scaledSize.Height / 2, scaledSize.Width / 4, paint_red);
		}

	}
}