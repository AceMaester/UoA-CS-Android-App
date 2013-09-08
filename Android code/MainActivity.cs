//Main screen for Android application of the University of Auckland Computer Science Website
//By Andrew Cairns Ettles, aett072, 5335926, 2013

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net;
using System.IO;
using System.Net;
using System.Drawing;
using Android.Graphics;


namespace CSac_android
{
	[Activity (Label = "University of Auckland", MainLauncher = true, Icon = "@drawable/au_icon")]
	public class MainActivity : Activity
	{

		private Button coursesButton;
		private Button newsButton; 
		private Button eventsButton; 
		private Button seminarsButton;
		private Button peopleButton;
		private ImageView aView;
		private ImageView headerView;
		private ImageView logoView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			coursesButton = (Button)FindViewById (Resource.Id.courseButton); //Main screen buttons
			newsButton = (Button)FindViewById (Resource.Id.newsButton);
			eventsButton = (Button)FindViewById (Resource.Id.eventsButton);
			seminarsButton = (Button)FindViewById (Resource.Id.seminarsButton);
			peopleButton = (Button)FindViewById (Resource.Id.peopleButton);


			aView = (ImageView)FindViewById (Resource.Id.homeImage); //Displays home image from CS website

			MemoryStream imagestream = new MemoryStream (new WebClient ().DownloadData ("http://redsox.tcs.auckland.ac.nz/CSS/CSService.svc/home_image"));
			var bitmap = Android.Graphics.BitmapFactory.DecodeStream ((Stream)imagestream);
			aView.SetImageBitmap (bitmap);
			aView.SetAdjustViewBounds (true);


			headerView = (ImageView)FindViewById (Resource.Id.headerImage); //Cloud image from CS website

			MemoryStream headerImagestream = new MemoryStream (new WebClient ().DownloadData ("http://www.cs.auckland.ac.nz/global/images/clouds.jpg"));
			var headerBitmap = Android.Graphics.BitmapFactory.DecodeStream ((Stream)headerImagestream);
			headerView.SetImageBitmap (headerBitmap);
			headerView.SetScaleType (ImageView.ScaleType.FitXy);

			logoView = (ImageView)FindViewById (Resource.Id.logoImage); //UoA logo from CS website

			MemoryStream logoImagestream = new MemoryStream (new WebClient ().DownloadData ("http://www.cs.auckland.ac.nz/global/images/logo.png"));
			var logoBitmap = Android.Graphics.BitmapFactory.DecodeStream ((Stream)logoImagestream);
			logoView.SetImageBitmap (logoBitmap);
			logoView.SetAdjustViewBounds (true);



			coursesButton.Click += (Sender, e) => {           //Action events for buttons
				var second = new Intent(this, typeof(Courses));
				StartActivity(second);
			};
			newsButton.Click += (Sender, e) => {
				var second = new Intent(this, typeof(RSSActivity));
				second.PutExtra("FeedType", "News");

				StartActivity(second);
			};
			eventsButton.Click += (Sender, e) => {
				var second = new Intent(this, typeof(RSSActivity));
				second.PutExtra("FeedType", "Events");

				StartActivity(second);
			};
			seminarsButton.Click += (Sender, e) => {
				var second = new Intent(this, typeof(RSSActivity));
				second.PutExtra("FeedType", "Seminars");

				StartActivity(second);
			};
			peopleButton.Click += (Sender, e) => {          
				var second = new Intent(this, typeof(PeopleActivity));
				StartActivity(second);
			};

		}
	}
}


