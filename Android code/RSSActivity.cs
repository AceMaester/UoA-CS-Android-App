// Homogeneous screen for display of News, Events and Seminars
// Consumes RSS feeds
// Andrew Cairns Ettles 2013

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
using System.Xml;
using Android.Text.Util;
using Android.Text;
using Android.Text.Method;

namespace CSac_android
{
	[Activity (Label = "RSSActivity")]

	public class RSSActivity : Activity
	{


		string[] titles;
		public static string[] links;
		public static string[] descriptions;
		public static string[] pubDates;
		private Button returnButton;
		private ListView rssList;
		private const int cancelDialog = 0;


		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.RSS);

			string feedType = Intent.GetStringExtra ("FeedType"); //type of rss feed news, events or seminars

			if (feedType == "News") {						//set title based on feedtype
				SetTitle (Resource.String.newsButtonText);
			} else if (feedType == "Events") {
				SetTitle (Resource.String.eventsButtonText);
			} else {
				SetTitle (Resource.String.seminarsButtonText);
			}




			GetRss(feedType); //consumes rss feeds

			rssList = (ListView)FindViewById (Resource.Id.rssList);
			rssList.Adapter = new CourseListAdapter (this, titles);


			if (rssList.Count == 0) {

				Console.WriteLine (rssList.Count);
				ShowDialog (cancelDialog);

			}


			returnButton = (Button)FindViewById (Resource.Id.returnButton); //button that returns to main screen
			returnButton.Click += (Sender, e) => {
				var main = new Intent(this, typeof(MainActivity));
				StartActivity(main);
			};





		}

	protected override Dialog OnCreateDialog(int id) //pop up dialog for when there are no items
		{
			switch (id) {
			case cancelDialog:
				var builder = new AlertDialog.Builder (this).SetMessage ("No items available").SetCancelable (false);


				builder.SetPositiveButton("Return", (s, e) => { 
					var main = new Intent(this, typeof(MainActivity));
					StartActivity(main); //
				});


				return builder.Create ();
			}

			return null;
		}




	public void GetRss(string feed){

		String url;

		if (feed == "Events")
		{

			url = "http://www.cs.auckland.ac.nz/uoa/home/template/events_feed.rss?category=other_events";
		}else if(feed == "News")
		{

			url = "http://www.cs.auckland.ac.nz/uoa/home/template/news_feed.rss?category=science_cs_news";
		}
		else if (feed == "Seminars")
		{

			url = "http://www.cs.auckland.ac.nz/uoa/home/template/events_feed.rss?category=seminars";
		}
		else
		{

			Console.WriteLine("Invalid rss feed");
				return;
		}

			Console.WriteLine("--- Retreiving " + feed + " ---");
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(url); //import news feed

			Console.WriteLine("--- Result ---");
			XmlNodeList item = xmlDoc.GetElementsByTagName("item");
			XmlDocument itemxml = new XmlDocument();
			String strXML = "";

			foreach (XmlNode node in item)
			{
				strXML += node.OuterXml;
			}

			strXML = "<root>" + strXML + "</root>";

			itemxml.LoadXml(strXML); //isolates xml only associated with each rss item


			XmlNodeList title = itemxml.GetElementsByTagName("title");
			XmlNodeList link = itemxml.GetElementsByTagName("link");
			XmlNodeList description = itemxml.GetElementsByTagName("description");
			XmlNodeList pubDate = itemxml.GetElementsByTagName("pubDate");

			string[] s = new string[title.Count];

			titles = new string[title.Count];
			links = new string[title.Count];
			descriptions = new string[title.Count];
			pubDates = new string[title.Count];

			for (int i = 0; i < title.Count; i++) {
				//s [i] = title [i].InnerText + " \n" + link[i].InnerText + " \n" + description[i].InnerText + " \n" + pubDate[i].InnerText;
				titles [i] = title [i].InnerText;
				links [i] = link [i].InnerText;
				descriptions [i] = description [i].InnerText;
				pubDates [i] = pubDate [i].InnerText;
			}

			/*for (int i = 0; i < link.Count; i++) { //for debugging purposes
				Console.WriteLine (title [i].InnerText);
				Console.WriteLine (link [i].InnerText);
				Console.WriteLine (pubDate [i].InnerText);
				Console.WriteLine (description [i].InnerText);
			}*/



			
	}
	}

	 class CourseListAdapter : BaseAdapter<string> {

		string[] values;
		Activity context;
		public CourseListAdapter(Activity context, string[] values)
			: base()
		{
			this.context = context;
			this.values = values;

		}

		public override long GetItemId(int position)
		{
			return position;
		}
		public override string this[int position]
		{
			get { return values[position]; }
		}
		public override int Count
		{
			get { return values.Length; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{

			View view = convertView;
			if (view == null) { // no view to re-use, create new
				view = context.LayoutInflater.Inflate (Resource.Layout.RSSListItem, null);

			}

			TextView link = view.FindViewById<TextView> (Resource.Id.rsstitle);
			String linkText = "<a href=\"" + RSSActivity.links [position] + "\">" + values [position] + "</a> ";

			link.TextFormatted = Html.FromHtml(linkText);
			link.MovementMethod = LinkMovementMethod.Instance;

			view.FindViewById<TextView>(Resource.Id.rssdescription).Text = RSSActivity.descriptions [position];
			view.FindViewById<TextView>(Resource.Id.rsspubdate).Text = RSSActivity.pubDates [position];
			return view;
		}
	}
}

