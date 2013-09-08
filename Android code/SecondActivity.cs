// Screen that displays list of UoA CS courses
// 
// Andrew Cairns Ettles 2013

using System;
using System.IO;
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

namespace CSac_android
{
	[Activity (Label = "Courses" )]			
	public class Courses : Activity
	{

		private ListView courseList;
		private Button returnButton;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Courses);
			string[] titles = GetCourses();


			courseList = (ListView)FindViewById (Resource.Id.courseList);

			courseList.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, titles);


			returnButton = (Button)FindViewById (Resource.Id.returnButton); //button that returns to main screen

			returnButton.Click += (Sender, e) => {
				var main = new Intent(this, typeof(MainActivity));


				StartActivity(main);

			};

		}

		public string[] GetCourses(){ //returns list of strings containing details for each course on UoA CS website
			Console.WriteLine ("--- Retriveing Courses ---");
			XmlDocument coursesxml = new XmlDocument ();
			coursesxml.Load ("http://redsox.tcs.auckland.ac.nz/CSS/CSService.svc/courses"); //import courses feed

			Console.WriteLine ("--- Result ---");
			XmlNodeList codeField = coursesxml.GetElementsByTagName ("codeField"); //this method is fucking amazing
			XmlNodeList semesterField = coursesxml.GetElementsByTagName ("semesterField");
			XmlNodeList titleField = coursesxml.GetElementsByTagName ("titleField");

			string[] s = new string[codeField.Count];//assuming every tag has same amount of fields

			for (int i = 0; i < codeField.Count; i++) {
				s [i] = codeField [i].InnerText + " \n" + titleField[i].InnerText + " \n" + semesterField[i].InnerText; //dat string
			}

			return s;

		}

	}		
				
}

