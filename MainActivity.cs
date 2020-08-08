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
	[Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : Activity
	{

		// global android obj
		public SQlightDB_Class m_db_obj;
		public Button m_Button_obj;
		public SKCanvasView m_skiaView_obj;
		public string m_dbfilename;
		public string m_app_name;
		public bool m_dbchanging;

		// for animation
		public string m_b_color_hex = "#fffafafa";
		public Stopwatch m_stopwatch; //animation
		public Stopwatch m_stopwatch_2; // string animation
		public bool m_pageIsActive;
		public float m_t;
		public float m_scale;
		public float m_scale2;
		public float m_scale_slow;
		public bool m_str_color_growing = true;
		public bool m_growing = true;
		public bool m_clock_set = false;

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
			m_dbfilename = Resources.GetString(Resource.String.database_name);
			// m_dbchanging lets us update the database. The program overwrites the database each time run. On release this will be false. 
            m_dbchanging = Resources.GetBoolean(Resource.Boolean.dbchanging);
			


			

			// creates the Path for the local android file system
			var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var dbFile = Path.Combine(docFolder, m_dbfilename); // FILE NAME TO USE WHEN COPIED


			if (!System.IO.File.Exists(dbFile) | m_dbchanging) // if file doesnt exist -> TRUE |OR| if m_dbchanging = true 
				// This should solve database not updating
			{
				var s = Resources.OpenRawResource(Resource.Raw.ChordDatabase2);  // DATA FILE RESOURCE ID
				FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
				ReadWriteStream(s, writeStream);
			}


			// readStream is the stream you need to read
			// writeStream is the stream you want to write to
			// Set our view from the "main" layout resource


			SetContentView(Resource.Layout.activity_main);
			
			// Set up objects
			
			m_skiaView_obj = FindViewById<SKCanvasView>(Resource.Id.SKIAVIEW_MAIN);
			m_Button_obj = FindViewById<Button>(Resource.Id.BUTTON_CHORDLIST);
			m_stopwatch = new Stopwatch();
			m_stopwatch_2 = new Stopwatch();
			m_pageIsActive = true;

			// Set up events
			// On click displays a list of notes that chords are based on
			m_Button_obj.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(ChordListActivity0));
				StartActivity(intent);
			};

			// Starts animation loop 
			AnimationLoop();

		}

		async Task AnimationLoop()
		{
			m_stopwatch.Start();

			while (m_pageIsActive)
			{

				// Bug when cycletime is increased. Makes animations mess up.
				double cycleTime = 15;
				
				// m_t varies from 0 to 1. time % cycletime(5) /cycletime(5) ->  (1 to 5)/ cycletime(5) -> 0 to 1 
				double m_t = m_stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
				double t_s = m_stopwatch.Elapsed.TotalSeconds % (cycleTime * cycleTime) / (cycleTime + cycleTime);
				// b/c of modifications the sin wave only varies from 0 to 1
				m_scale = (1 + (float)System.Math.Sin((2 * System.Math.PI * m_t) - (System.Math.PI / 2))) / 2;
				m_scale2 = (1 + (float)System.Math.Cos((2 * System.Math.PI * m_t))) / 2;
				// For debugging
				System.Console.WriteLine("Total seconds {0} Sin {1} Cos {2}", m_stopwatch.Elapsed.TotalSeconds, m_scale, m_scale2);
				m_scale_slow = (1 + (float)System.Math.Sin((2 * System.Math.PI * t_s) - (System.Math.PI / 2))) / 2;
				await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
				// makes the canvas redraw itself
				m_skiaView_obj.Invalidate();
				m_clock_set = true; //for a bug with m_scale 2 starting at 1 causing a later for loop to start early
			}

			m_stopwatch.Stop();
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
			m_skiaView_obj.PaintSurface += OnPaintSurface;
			
		}

		protected override void OnPause()
		{
			base.OnPause();
			m_skiaView_obj.PaintSurface -= OnPaintSurface;
			m_pageIsActive = false; // should kill animation loop
			
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

			// makes background colors match
			SKColor b_color = new SKColor();
			SKColor.TryParse(m_b_color_hex, out b_color);
			canvas.Clear(b_color);



			// define all paints
			var paint_red = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				StrokeWidth = 9.5f, // in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Butt
			};
			

			var paint_red2 = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				

				StrokeWidth = 4.5f, // in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Round
			};

			var paint_black = new SKPaint
			{
				Color = new SKColor(0, 0, 0, (byte)(255 * m_scale2)),
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 2.5f, //in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Round,
			};

			var paint_black_ng = new SKPaint
			{
				Color = SKColors.Black,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 2.5f, //in dp
				IsAntialias = true,
				StrokeCap = SKStrokeCap.Round,
			};

			SKPoint center = new SKPoint(scaledSize.Width / 2, scaledSize.Height / 2);

			//const for strs
			m_x_start_pad = center.X / 1.3f;
			m_x_str_space = (scaledSize.Width - (2 * m_x_start_pad)) / 5;
			m_y_start_pad = center.Y / 2;
			m_y_str_len = (scaledSize.Height/1.3f) ;

			//calculate and generate str paths
			SKPath[] str_6 = new SKPath[6];

			gen_strs(out str_6);

			// Caculate circle 

			float baseRadius = System.Math.Min(scaledSize.Width, scaledSize.Height) / 16;

			// controlls whether to draw a static circle or not
			if (m_scale > .999)
			{
				m_growing = false;
			}

			// draw circle
			for (int circle = 0; circle < 2; circle++)
			{
				float radius = baseRadius * (4*(circle + m_t));


				if (m_growing)
				{

					//animated circle
					canvas.DrawCircle(center.X, center.Y * m_scale, radius * m_scale, paint_red);
				}
				else
				{

					// static circle
					canvas.DrawCircle(center.X, center.Y, radius, paint_red);
				}




			}



			

			// tells strs_color to stop fading in and out
			if (m_scale2 > .999 & m_clock_set == true)
			{
				m_str_color_growing = false;
			}
				//draw strings
			if (m_growing == false)
			{

				if (m_str_color_growing)
				{
					foreach (var str in str_6)
					{
						//animated strings
						canvas.DrawPath(str, paint_black);

					}
				}
				else
				{
					foreach (var str in str_6)
					{
						//static strings
						canvas.DrawPath(str, paint_black_ng);

					}
				}


			}


			// making the strings change color

			if (m_str_color_growing == false)
			{


				m_stopwatch_2.Start();




				// string changes color at increments of 1/2 s

                if (m_stopwatch_2.Elapsed.TotalSeconds <= .5) 
                {
					for (int i = 0; i < 1; i++)
                    {
					 canvas.DrawPath(str_6[i], paint_red2);
					}
                    
                }
                else if (m_stopwatch_2.Elapsed.TotalSeconds > .5 & m_stopwatch_2.Elapsed.TotalSeconds <= 1.0f) 
                {
					for (int i = 0; i < 2; i++)
					{
						canvas.DrawPath(str_6[i], paint_red2);
					}
				}
                else if (m_stopwatch_2.Elapsed.TotalSeconds > 1.0f & m_stopwatch_2.Elapsed.TotalSeconds <= 1.5f) 
                {
					for (int i = 0; i < 3; i++)
					{
						canvas.DrawPath(str_6[i], paint_red2);
					}
				}
                else if (m_stopwatch_2.Elapsed.TotalSeconds > 1.5f & m_stopwatch_2.Elapsed.TotalSeconds <= 2.0f)
                {
					for(int i = 0; i < 4; i++)
					{
						canvas.DrawPath(str_6[i], paint_red2);
					}
				}
                else if (m_stopwatch_2.Elapsed.TotalSeconds > 2.0f & m_stopwatch_2.Elapsed.TotalSeconds <= 2.5f)
                {
					for (int i = 0; i < 5; i++)
					{
						canvas.DrawPath(str_6[i], paint_red2);
					}
				}
                else if (m_stopwatch_2.Elapsed.TotalSeconds > 2.5f)
                {
					for (int i = 0; i < 6; i++)
					{
						canvas.DrawPath(str_6[i], paint_red2);
					}

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
			// .9 * width * ratio(height/width)   
			// gives you the height for a given width
			textPaint.TextSize = 0.90f * scaledSize.Width * (textPaint.TextSize / textWidth);
			// Find the text bounds
			SKRect textBounds = new SKRect();
			textPaint.MeasureText(m_app_name, ref textBounds);
			// Calculate offsets to center the text on the screen
			float xText = scaledSize.Width / 2 - textBounds.MidX;
			float yText = scaledSize.Height / 1.2f - textBounds.MidY;
			canvas.DrawText(m_app_name, xText, yText, textPaint);
			System.Console.WriteLine(m_scale2);





		}
	}

}
