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
using System.IO;
using System.Net;
using Android.Text;
using Android.Text.Method;
using System.Resources;
using Android.Text.Util;

namespace CSac_android
{
	[Activity (Label = "People")]			
	public class PeopleActivity : Activity
	{

		private Button returnButton;
		private ListView peopleList;
		public static VCard[] v; //array of V-cards


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.People);

			returnButton = (Button)FindViewById (Resource.Id.returnButton); //button that returns to main screen
			returnButton.Click += (Sender, e) => {
				var main = new Intent (this, typeof(MainActivity));
				StartActivity (main);
			};


		}

		protected override void OnStart(){


			base.OnStart ();
			peopleList = (ListView)FindViewById (Resource.Id.peopleList);

			//peopleList.Adapter = new PeopleListAdapter (this, null);

			if (v == null) {
				GetPeople ();
			}

			string[] names = new string[v.Length];
			for (int i = 0; i < v.Length; i++) {
				names [i] = v [i].fullname;
			}


			peopleList = (ListView)FindViewById (Resource.Id.peopleList);

			peopleList.Adapter = new PeopleListAdapter (this, names);




		}

		public void GetPeople() //consumes url to produce an array of v-cards for each person
		{
			Console.WriteLine ("--- Retriveing Upis ---");
			XmlDocument upisxml = new XmlDocument ();
			upisxml.Load ("http://redsox.tcs.auckland.ac.nz/CSS/CSService.svc/people");

			Console.WriteLine ("--- Result ---");
			XmlNodeList uPIField = upisxml.GetElementsByTagName ("uPIField");

			//Console.WriteLine ("UPIs: ");

			/* for (int i = 0; i < uPIField.Count; i++) //debug print all upi's
            {
                Console.WriteLine(uPIField[i].InnerText);
            }*/

			MemoryStream vcardstream;
			v = new VCard[uPIField.Count]; //array of v-cards




				for (int i = 0; i < uPIField.Count; i++) {
					vcardstream = new MemoryStream (new WebClient ().DownloadData (" http://www.cs.auckland.ac.nz/our_staff/vcard.php?upi=" + uPIField [i].InnerText));
					var sr = new StreamReader (vcardstream);
					var vcardstr = sr.ReadToEnd (); //get v-card as one string

					string[] vcardfields = vcardstr.Split (':');//: is delimiter in v-card

					for (int j = 0; j < vcardfields.Length; j++) {

						string[] temp = vcardfields [j].Split ((char)13); //carraige return is also a delimiter
						vcardfields [j] = temp [0];

					}

					if (vcardfields [11] == "") { //v card does not have a homepage

						v [i] = new VCard (vcardfields [3], vcardfields [6], "no homepage", vcardfields [9], vcardfields [12]);

					} else {

						v [i] = new VCard (vcardfields [3], vcardfields [6], vcardfields [11] + ":" + vcardfields [12], vcardfields [9], vcardfields [13]); //index 11 + 12 due to : in http://
					}

					v [i].ToString (); //debug, prints all v-cards
					
				}

		}

	}


	class PeopleListAdapter : BaseAdapter<string> { //apater for CourseListItems

		string[] values;
		Activity context;
		public PeopleListAdapter(Activity context, string[] values)
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
				view = context.LayoutInflater.Inflate (Resource.Layout.PeopleListItem, null);

			}

			if (PeopleActivity.v [position].picture == null) { //if perosn has no image display default image


				view.FindViewById<ImageView> (Resource.Id.personPic).SetImageResource (Resource.Drawable.blank_person);
				view.FindViewById<ImageView> (Resource.Id.personPic).SetAdjustViewBounds (true);
			} else { //displays person's image

				view.FindViewById<ImageView> (Resource.Id.personPic).SetImageBitmap (PeopleActivity.v [position].picture);
				view.FindViewById<ImageView> (Resource.Id.personPic).SetAdjustViewBounds (true);
			}

				string homepage = PeopleActivity.v [position].homepage;
				

			if (homepage != "no homepage") { //if person has a homepage make button that links to their homepage

				view.FindViewById<Button> (Resource.Id.homePageButton).Enabled = true;
				view.FindViewById<Button> (Resource.Id.homePageButton).Visibility = ViewStates.Visible;
				Button b = (Button)view.FindViewById (Resource.Id.homePageButton);
				if (b != null) {
					b.Click += (Sender, e) => {


						var main = Android.Content.Intent.ParseUri(homepage, IntentUriType.None);
						//Console.WriteLine(homepage);
						context.StartActivity (main);
					};
				}

			} else {
				view.FindViewById<Button> (Resource.Id.homePageButton).Enabled = false;
				view.FindViewById<Button> (Resource.Id.homePageButton).Visibility = ViewStates.Gone;
			}



			Linkify.AddLinks (view.FindViewById<TextView> (Resource.Id.phoneNo), MatchOptions.PhoneNumbers);
			Linkify.AddLinks (view.FindViewById<TextView> (Resource.Id.email), MatchOptions.EmailAddresses);
			view.FindViewById<TextView> (Resource.Id.phoneNo).Text = PeopleActivity.v [position].phoneno;
			view.FindViewById<TextView> (Resource.Id.email).Text = PeopleActivity.v [position].email;
			view.FindViewById<TextView> (Resource.Id.fullName).Text = values [position]; //persons name
			return view;
		}
	}


}

