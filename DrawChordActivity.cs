using Android.App;
using Android.OS;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Util;
using SkiaSharp;
using SkiaSharp.Views.Android;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Packaged_Database
{
	[Activity(Label = "DrawChordActivity")]
	public class DrawChordActivity : Activity
	{
		public SKCanvasView skiaView_obj;

		private List<int> chord_frets_parsed;
		private int ChordPos; // used for drawing chord diagram
		private int Dis_ChordPos; //display pos is 1 higher
		private string m_ChordName;


		// Drawing constants
		private float space;
		private float x_start;
		private float y_start;
		private float string_len;
		private float fret_len_space;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_draw);
			//set up objs
			skiaView_obj = FindViewById<SKCanvasView>(Resource.Id.SKIAVIEW_DRAW);




			// Get Chord from intent
			string chord_frets = Intent.GetStringExtra("Chord");
			ChordPos = Intent.GetIntExtra("Chord_pos", 0);
			Dis_ChordPos = ChordPos + 1;
			m_ChordName = Intent.GetStringExtra("Chord_name");
			//Parsing chord string into List
			chord_frets_parsed = new List<int>();
			string holder = null; //holds 2 digit fret numbers
			string db_fseperater = Resources.GetString(Resource.String.comma);
			char db_s = db_fseperater[0];//converts to char
			for (int i = 0; i < (int)chord_frets.Length; i++)
			{
				if (chord_frets[i] != db_s)
				{
					holder += chord_frets[i];

				}


				if (chord_frets[i] == db_s | i == (chord_frets.Length - 1))
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
			skiaView_obj.PaintSurface += OnPaintSurface;
		}

		protected override void OnPause()
		{


			base.OnPause();
			skiaView_obj.PaintSurface -= OnPaintSurface;

		}

		private SKPath DrawX(SKPoint rel_prt)
		{
			int dis_size = 20;
			var path = new SKPath();
			path.MoveTo(rel_prt.X - dis_size, rel_prt.Y - dis_size);
			path.LineTo(dis_size + rel_prt.X, dis_size + rel_prt.Y);
			path.MoveTo(dis_size + rel_prt.X, rel_prt.Y - dis_size);
			path.LineTo(rel_prt.X - dis_size, dis_size + rel_prt.Y);
			return path;
		}

		private void gen_note_str(out SKPoint[] str_notes, int string_number)
		{
			string_number = 6 - string_number; //backwards
			str_notes = new SKPoint[4];
			int k = 0;
			for (int j = string_number; j < (string_number + 1); j++)
			{
				float x = (x_start + (space * j));


				for (int i = 0; i < 4; i++)
				{
					float y = y_start + fret_len_space / 2 + (fret_len_space * i);
					str_notes[k] = new SKPoint(x, y);
					k++;
				}
			}

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
				StrokeWidth = 10, // in dp
				StrokeCap = SKStrokeCap.Butt,
			};

			var paint_blue = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Blue,
				StrokeWidth = 35, //in dp
				StrokeCap = SKStrokeCap.Round

			};
			// Create an SKPaint object to display the text
			var textPaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				FakeBoldText = true,
				StrokeWidth = 3f,
				Color = SKColors.Blue,
				IsAntialias = true,

			};

			/*
			 * Coordinates are specified relative to the upper-left corner of the display surface.
			 * X coordinates increase to the right and Y coordinates increase going down. 
			 * In discussion about graphics, often the mathematical notation (x, y) is used to denote a point. 
			 * The point (0, 0) is the upper-left corner of the display surface and is often called the origin.
			 */
			// Adjust TextSize property so text is 95% of screen width
			float textWidth = textPaint.MeasureText(m_ChordName);

			// 95% of width * (Height for width ratio) 
			textPaint.TextSize = 0.15f * scaledSize.Height;

			// Find the text bounds
			SKRect textBounds = new SKRect();
			textPaint.MeasureText(m_ChordName, ref textBounds);

			// Calculate offsets to center the text on the screen
			float xText = scaledSize.Width / 2 - textBounds.MidX;
			float yText = scaledSize.Height / 7.5f - textBounds.MidY;

			// And draw the text
			canvas.DrawText(m_ChordName, xText, yText, textPaint);


			// Adjust TextSize property so text is 95% of screen width
			textWidth = textPaint.MeasureText(Dis_ChordPos.ToString());

			// 10& of height * (Height for width ratio) 
			textPaint.TextSize = 0.1f * scaledSize.Height;

			// Find the text bounds
			textBounds = new SKRect();
			textPaint.MeasureText(Dis_ChordPos.ToString(), ref textBounds);

			// Calculate offsets to center the text on the screen
			xText = scaledSize.Width / 9 - textBounds.MidX;
			yText = scaledSize.Height / 2.5f - textBounds.MidY;

			// And draw the text
			textPaint.Color = SKColors.Black;
			textPaint.Style = SKPaintStyle.StrokeAndFill;
			canvas.DrawText(Dis_ChordPos.ToString(), xText, yText, textPaint);


			// divide the scaled size width into 7 spaces 
			space = ((scaledSize.Width) / 7);
			x_start = space + 25;
			y_start = 200;
			string_len = (scaledSize.Height - y_start / 2);
			fret_len_space = (string_len - 110) / 4;





			// Strings
			SKPoint[] string_points = new SKPoint[12];
			for (int i = 0; i < 2; i++)
			{
				float y = y_start + (string_len * i) - (fret_len_space * i);

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

			// 6th string E note positions
			gen_note_str(out str_6, 6);

			// 5th string A note positions
			gen_note_str(out str_5, 5);

			// 4th string D
			gen_note_str(out str_4, 4);

			// 3rd string G
			gen_note_str(out str_3, 3);

			// 2nd string B
			gen_note_str(out str_2, 2);

			// 1st string E
			gen_note_str(out str_1, 1);



			// Chord Notes ::::
			// Converts the chords frets to actual positions on screen
			// subtracts position - 1 to get relative position 
			// then adds the relative position to an array

			SKPoint[] chord_prts = new SKPoint[6];
			for (int i = 0; i < chord_frets_parsed.Count; i++)
			{
				// error with larger chords tried ChordPos to solve problem
				if (!(chord_frets_parsed[i] >= 100 | chord_frets_parsed[i] <= 0))//100 represents "^"
				{
					int parse_temp = chord_frets_parsed[i];
					parse_temp = parse_temp - 1 - ChordPos; // -1 for array

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
					SKPath x_path = new SKPath();


					switch (i)
					{
						case 0:
							temp_adj = str_6[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red);
							break;
						case 1:
							temp_adj = str_5[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red);
							break;
						case 2:
							temp_adj = str_4[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2; ;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red); ;
							break;
						case 3:
							temp_adj = str_3[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2; ;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red);
							break;
						case 4:
							temp_adj = str_2[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2; ;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red);
							break;
						case 5:
							temp_adj = str_1[0];
							temp_adj.Y = temp_adj.Y - fret_len_space / 2; ;
							x_path = DrawX(temp_adj);
							canvas.DrawPath(x_path, paint_red);
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