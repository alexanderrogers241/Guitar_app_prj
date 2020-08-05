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
using System.Diagnostics;
using System.Threading.Tasks;

namespace Packaged_Database
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : Activity
	{

		// global android obj
		public SQlightDB_Class db_obj;
		public Button Button_obj;
		public SKCanvasView skiaView_obj;
		public string dbfilename;
		public string m_app_name;

		// for animation

		const double cycleTime = 1000;       // in milliseconds
		public Stopwatch stopwatch = new Stopwatch();
		public bool pageIsActive;
		public float t;
		public float scale;
		public float scale2;
		public float scale_slow;
		public bool m_str_color_growing = true;
		public bool m_growing = true;

		// string constants

		public float m_x_start_pad;
		public float m_x_str_space;
		public float m_y_start_pad;
		public float m_y_str_len;

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

			pageIsActive = true;
			AnimationLoop();

		}

		async Task AnimationLoop()
		{
			stopwatch.Start();

			while (pageIsActive)
			{
				double cycleTime = 15;

				// t varies from 0 to 1. time % cycletime(5) /cycletime(5) ->  (1 to 5)/ cycletime(5) -> 0 to 1 
				double t = stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
				double t_s = stopwatch.Elapsed.TotalSeconds % (cycleTime * cycleTime) / (cycleTime + cycleTime);
				// b/c of modifications the sin wave only varies from 0 to 1
				scale = (1 + (float)System.Math.Sin((2 * System.Math.PI * t) - (System.Math.PI / 2))) / 2;
				scale2 = (1 + (float)System.Math.Cos((2 * System.Math.PI * t))) / 2;
				scale_slow = (1 + (float)System.Math.Sin((2 * System.Math.PI * t_s) - (System.Math.PI / 2))) / 2;
				skiaView_obj.Invalidate();
				await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
			}

			stopwatch.Stop();
		}

		private void gen_strs(out SKPath[] strs) // might want to add this to chorddraw activity. Its different.
		{

			strs = new SKPath[6];

			int k = 0;
			for (int j = 0; j < 6; j++)
			{
				float x = (m_x_start_pad + (m_x_str_space * j));


				for (int i = 0; i < 2; i++)
				{
					float y = m_y_start_pad + ((m_y_str_len - m_y_start_pad) * i);

					if (i == 1)
					{
						strs[k].LineTo(x, y);
					}
					else
					{
						strs[k] = new SKPath();
						strs[k].MoveTo(x, y);
					}


				}
				k++;
			}


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

			var scrn_scale = Resources.DisplayMetrics.Density;

			// get the scaled size of the screen in density-indep pixals
			// pixals = (dp pixals) * DisplayMetrics.Density
			// (dp pixals) = pixals / DisplayMetrics.Density
			var scaledSize = new SKSize(info.Width / scrn_scale, info.Height / scrn_scale);

			// handle the device screen density
			canvas.Scale(scrn_scale);

			// make sure the canvas is blank
			canvas.Clear(SKColors.White);


			var paint_red = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				StrokeWidth = 8, // in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Butt
			};

			var paint_red_g = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed.WithAlpha((byte)(255 * scale * .4f)),
				StrokeWidth = 8, // in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Butt
			};

			var paint_black = new SKPaint
			{
				Color = new SKColor(0, 0, 0, (byte)(255 * scale2)),
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 5, //in dp
				IsAntialias = true,
			};

			var paint_black_ng = new SKPaint
			{
				Color = SKColors.Black,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 5, //in dp
				IsAntialias = true,
			};

			SKPoint center = new SKPoint(scaledSize.Width / 2, scaledSize.Height / 2);

			//const for strs
			m_x_start_pad = center.X / 1.3f;
			m_x_str_space = (scaledSize.Width - (2 * m_x_start_pad)) / 5;
			m_y_start_pad = center.Y / 6;
			m_y_str_len = (scaledSize.Height / 1.2f) - m_y_start_pad;

			//calculate and generate str paths
			SKPath[] str_6 = new SKPath[6];

			gen_strs(out str_6);

			// Caculate circle and draw

			float baseRadius = System.Math.Min(scaledSize.Width, scaledSize.Height) / 16;

			// controlls whether to draw a static circle or not
			if (scale > .999)
			{
				m_growing = false;
			}


			for (int circle = 0; circle < 4; circle++)
			{
				float radius = baseRadius * (circle + t);


				if (m_growing)
				{
					canvas.DrawCircle(center.X, center.Y * scale, radius * scale, paint_red);
				}
				else
				{
					canvas.DrawCircle(center.X, center.Y, radius, paint_red);
				}




			}



			//draw strings

			// tells strs_color to stop fading in and out
			if (scale2 > .999)
			{
				m_str_color_growing = false;
			}

			if (m_growing == false)
			{

				if (m_str_color_growing)
				{
					foreach (var str in str_6)
					{
						canvas.DrawPath(str, paint_black);

					}
				}
				else
				{
					foreach (var str in str_6)
					{
						canvas.DrawPath(str, paint_black_ng);

					}
				}


			}


			if (m_str_color_growing == false)
			{
				if (scale_slow <= 0.17f) //select str 6
				{
					canvas.DrawPath(str_6[0], paint_red_g);
				}
				else if (scale_slow <= 0.33f & scale_slow > 0.17f) //select str5
				{
					canvas.DrawPath(str_6[5], paint_red_g);
				}
				else if (scale_slow <= 0.50f & scale_slow > 0.33f) // so on....
				{
					canvas.DrawPath(str_6[2], paint_red_g);
				}
				else if (scale_slow <= 0.66f & scale_slow > 0.50f)
				{
					canvas.DrawPath(str_6[1], paint_red_g);
				}
				else if (scale_slow <= 0.83f & scale_slow > 0.66f)
				{
					canvas.DrawPath(str_6[4], paint_red_g);
				}
				else if (scale_slow > 0.83)
				{
					canvas.DrawPath(str_6[3], paint_red_g);

				}
			}

			// app name

			var textPaint = new SKPaint
			{
				Style = SKPaintStyle.StrokeAndFill,
				FakeBoldText = true,
				StrokeWidth = 2.2f,
				Color = SKColors.OrangeRed,
				IsAntialias = true,
			};
			float textWidth = textPaint.MeasureText(m_app_name);
			textPaint.TextSize = 0.90f * scaledSize.Width * (textPaint.TextSize / textWidth);
			// Find the text bounds
			SKRect textBounds = new SKRect();
			textPaint.MeasureText(m_app_name, ref textBounds);
			// Calculate offsets to center the text on the screen
			float xText = scaledSize.Width / 2 - textBounds.MidX;
			float yText = scaledSize.Height / 1.2f - textBounds.MidY;
			canvas.DrawText(m_app_name, xText, yText, textPaint);





		}
	}

}
