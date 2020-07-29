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

namespace Packaged_Database
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public APK_SQlightDB_Class db_obj;
		public Button Button_obj;
		public SKCanvasView skiaView_obj;
		public string dbfilename;
		protected override void OnCreate(Bundle savedInstanceState)
		{


			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);
			dbfilename = Resources.GetString(Resource.String.database_name);
			// Set up objects
			db_obj = new APK_SQlightDB_Class(dbfilename); //database
			skiaView_obj = FindViewById<SKCanvasView>(Resource.Id.SKIAVIEW_MAIN);
			Button_obj = FindViewById<Button>(Resource.Id.BUTTON_CHORDLIST);
			// Set up events

			Button_obj.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(ChordListActivity));
				StartActivity(intent);
			};

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
			var paint = new SKPaint
			{
				Color = SKColors.Black,
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				TextAlign = SKTextAlign.Center,
				TextSize = 24
			};

			var paint_red = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.OrangeRed,
				StrokeWidth = 25
			};

			var paint_blue = new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = SKColors.Blue,
				StrokeWidth = 25

			};
			var coord = new SKPoint(scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2);
			var coord2 = coord;
			coord2.Y -= 200;
			canvas.DrawText("SkiaSharp", coord, paint);
			canvas.DrawCircle(coord2, scaledSize.Width / 4, paint_red);
			canvas.DrawCircle(coord2, scaledSize.Width / 4, paint_blue);
		}

	}
}