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
		public static string[] titles;
		public static string[] codes;
		public static string[] semesters;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Courses);
			GetCourses();


			courseList = (ListView)FindViewById (Resource.Id.courseList);

			courseList.Adapter = new CourseListAdapter(this, titles);


		}

		public void GetCourses(){ //returns list of strings containing details for each course on UoA CS website
			Console.WriteLine ("--- Retriveing Courses ---");
			XmlDocument coursesxml = new XmlDocument ();
			coursesxml.Load ("http://redsox.tcs.auckland.ac.nz/CSS/CSService.svc/courses"); //import courses feed

			Console.WriteLine ("--- Result ---");
			XmlNodeList codeField = coursesxml.GetElementsByTagName ("codeField"); //this method is fucking amazing
			XmlNodeList semesterField = coursesxml.GetElementsByTagName ("semesterField");
			XmlNodeList titleField = coursesxml.GetElementsByTagName ("titleField");

			 titles = new string[codeField.Count];
			 codes = new string[codeField.Count];
			 semesters = new string[codeField.Count];//assuming every tag has same amount of fields

			for (int i = 0; i < codeField.Count; i++) {
				//s [i] = codeField [i].InnerText + " \n" + titleField[i].InnerText + " \n" + semesterField[i].InnerText; //dat string
				titles [i] = titleField [i].InnerText;
				codes [i] = codeField [i].InnerText;
				semesters [i] = semesterField [i].InnerText;
			
			}


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
				view = context.LayoutInflater.Inflate (Resource.Layout.CourseListItem, null);

			}

			view.FindViewById<TextView> (Resource.Id.courseTitle).Text = Courses.titles [position];
			view.FindViewById<TextView> (Resource.Id.courseCode).Text = Courses.codes [position];
			view.FindViewById<TextView>(Resource.Id.courseSemester).Text = Courses.semesters [position];
			return view;
		}
	}
				
}

