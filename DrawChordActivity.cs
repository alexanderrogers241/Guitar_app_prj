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
	[Activity(Label = "DrawChordActivity")]
	public class DrawChordActivity : Activity
	{
		public SKCanvasView skiaView_obj;

		private List<int> chord_frets_parsed;
		private int ChordPos;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_draw);
			//set up objs
			skiaView_obj = FindViewById<SKCanvasView>(Resource.Id.SKIAVIEW_DRAW);




			// Get Chord from intent
			string chord_frets = Intent.Extras.GetString("Chord");
			int.TryParse(Intent.Extras.GetString("Chord_pos"), out ChordPos);
			//Parsing chord string into List
			chord_frets_parsed = new List<int>();
			string holder = null; //holds 2 digit fret numbers
			for (int i = 0; i < (int)chord_frets.Length; i++)
			{
				if (chord_frets[i] != '^')
				{
					holder += chord_frets[i];

				}


				if (chord_frets[i] == '^' | i == (chord_frets.Length - 1))
				{

					if (holder.Contains("X") | holder.Contains("x"))
                    {


						chord_frets_parsed.Add(100); // 100 represents X


					}
					else
                    {
						int holder_int;
						int.TryParse(holder, out holder_int);
						chord_frets_parsed.Add(holder_int);
				
					}

					holder = null;//empty for next go around
				}
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			int.TryParse(Intent.Extras.GetString("Chord_pos"), out ChordPos);
			skiaView_obj.PaintSurface += OnPaintSurface;
		}

		protected override void OnPause()
		{
			

			base.OnPause();
			skiaView_obj.PaintSurface -= OnPaintSurface;
			
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			// the the canvas and properties
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = args.Surface.Canvas;

			// get the screen density for scaling
			var scale = Resources.DisplayMetrics.Density;

			// get the scaled size of the screen in density-indep pixals
			// pixals = (dp pixals) * DisplayMetrics.Density
			// (dp pixals) = pixals / DisplayMetrics.Density
			var scaledSize = new SKSize(info.Width / scale, info.Height / scale);

			// handle the device screen density
			canvas.Scale(scale);

			// make sure the canvas is blank
			canvas.Clear(SKColors.White);

			// draw some text
			var paint = new SKPaint
			{
				Color = SKColors.Black,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 10, //in dp
			};

			var paint_red = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				StrokeWidth = 25, // in dp
				StrokeCap = SKStrokeCap.Round
			};

			var paint_blue = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Blue,
				StrokeWidth = 35, //in dp
				StrokeCap = SKStrokeCap.Round

			};

			/*
			 * Coordinates are specified relative to the upper-left corner of the display surface.
			 * X coordinates increase to the right and Y coordinates increase going down. 
			 * In discussion about graphics, often the mathematical notation (x, y) is used to denote a point. 
			 * The point (0, 0) is the upper-left corner of the display surface and is often called the origin.
			 */

			// point in the middle
			SKPoint coordcenter = new SKPoint(scaledSize.Width / 2, (scaledSize.Height) / 2);


			// divide the scaled size width into 7 spaces 
			float space = ((scaledSize.Width) / 7);
			float string_len = (scaledSize.Height - 60);
			float fret_len_space = (string_len - 100) / 4;
			float x_start = space + 25;
			float y_start = 50;


			// Strings
			SKPoint[] string_points = new SKPoint[12];
			for (int i = 0; i < 2; i++)
			{
				float y = y_start + (string_len * i) - (y_start * i);

				for (int j = 0; j < 6; j++)
				{
					float x = x_start + space * j;
					string_points[2 * j + i] = new SKPoint(x, y);
				}
			}
			// Render the points by calling DrawPoints
			SKPointMode pointMode = SKPointMode.Lines;
			canvas.DrawPoints(pointMode, string_points, paint);


			//	Frets
			SKPoint[] fret_points = new SKPoint[10];
			for (int i = 0; i < 2; i++)
			{
				float x = (x_start + (5 * space * i));

				for (int j = 0; j < 5; j++)
				{
					float y = y_start + fret_len_space * j;
					fret_points[2 * j + i] = new SKPoint(x, y);
				}
			}

			// Render the points by calling DrawPoints
			pointMode = SKPointMode.Lines;
			canvas.DrawPoints(pointMode, fret_points, paint);


			// Notes on each string
			SKPoint[] str_6 = new SKPoint[4];
			SKPoint[] str_5 = new SKPoint[4];
			SKPoint[] str_4 = new SKPoint[4];
			SKPoint[] str_3 = new SKPoint[4];
			SKPoint[] str_2 = new SKPoint[4];
			SKPoint[] str_1 = new SKPoint[4];

			// array iterater
			int k = 0;


			// 6th string E note positions
			for (int j = 0; j < 1; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_6[k] = new SKPoint(x, y);
					k++;
				}
			}
			k = 0;
			// 5th string A note positions
			for (int j = 1; j < 2; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_5[k] = new SKPoint(x, y);
					k++;
				}
			}
			k = 0;
			// 4th string D
			for (int j = 2; j < 3; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_4[k] = new SKPoint(x, y);
					k++;
				}
			}
			k = 0;
			// 3rd string G
			for (int j = 3; j < 4; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_3[k] = new SKPoint(x, y);
					k++;
				}
			}
			k = 0;
			// 2nd string B
			for (int j = 4; j < 5; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_2[k] = new SKPoint(x, y);
					k++;
				}
			}
			k = 0;
			// 1st string E
			for (int j = 5; j < 6; j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + (y_start) + (fret_len_space * i);
					str_1[k] = new SKPoint(x, y);
					k++;
				}
			}
			// Chord Notes ::::
			// Converts the chords frets to actual positions on screen
			// subtracts position - 1 to get relative position 
			// then adds the relative position to an array
		
			SKPoint[] chord_prts = new SKPoint[6];
			for (int i = 0; i < chord_frets_parsed.Count; i++)
			{
				// error with larger chords tried ChordPos to solve problem
				if  (!( chord_frets_parsed[i] >= 100 | chord_frets_parsed[i] <= 0 ))//100 represents "^"
				{
					int parse_temp = chord_frets_parsed[i];
					parse_temp = parse_temp  - 1 - ChordPos; // -1 for array

					switch (i)
					{
						case 0:
							chord_prts[i] = str_6[parse_temp];
							break;
						case 1:
							chord_prts[i] = str_5[parse_temp];
							break;
						case 2:
							chord_prts[i] = str_4[parse_temp];
							break;
						case 3:
							chord_prts[i] = str_3[parse_temp];
							break;
						case 4:
							chord_prts[i] = str_2[parse_temp];
							break;
						case 5:
							chord_prts[i] = str_1[parse_temp];
							break;
						default:
							System.Console.WriteLine("Exep: More than 6 notes");
							break;
					}

				}
				else if (chord_frets_parsed[i] <= 100)
				{
					SKPoint temp_adj = new SKPoint();

					switch (i)
					{
						case 0:
							temp_adj = str_6[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						case 1:
							temp_adj = str_5[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						case 2:
							temp_adj = str_4[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						case 3:
							temp_adj = str_3[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						case 4:
							temp_adj = str_2[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						case 5:
							temp_adj = str_1[0];
							temp_adj.Y = temp_adj.Y - (y_start);
							canvas.DrawPoint(temp_adj, paint_red);
							break;
						default:
							System.Console.WriteLine("Exep: More than 6 notes");
							break;

					}
					chord_prts[i] = new SKPoint(-50, -50); // open str (0) = off screen
				}
				else
				{
					chord_prts[i] = new SKPoint(-50, -50); // open str (0) = off screen 
				}
			}
			canvas.DrawPoints(SKPointMode.Points, chord_prts, paint_blue);



		}
	}
}