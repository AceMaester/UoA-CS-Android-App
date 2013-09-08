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
using Android.Graphics;

namespace CSac_android
{
	public class VCard
	{
		public string phoneno;
		public string fullname;
		public string homepage;
		public string email;
		public Bitmap picture;
		private string picString;


		public VCard(string pn, string fn, string hp, string e, string pic){

			fullname = fn;
			homepage = hp;
			email = e;
			picString = pic;

			StringBuilder sb = new StringBuilder();
			char[] ca = pn.ToCharArray();

			for(int i = 0; i<ca.Length; i++){
				if((int)ca[i] < 48 || (int)ca[i] > 57){
					sb.Append(' ');
				}else{
					sb.Append(ca[i]);
				}
			}

			phoneno = sb.ToString();

			
			if (pic.Substring (0, 3) == "/9j") {

				byte[] byteBuffer = Convert.FromBase64String (pic);
				MemoryStream memoryStream = new MemoryStream (byteBuffer);

				memoryStream.Position = 0;


				picture = Android.Graphics.BitmapFactory.DecodeStream ((Stream)memoryStream);
				memoryStream.Close ();
			}
			



		}


		public void ToString()
		{

			Console.WriteLine("phoneno: " + phoneno);
			Console.WriteLine("fullname: " + fullname);
			Console.WriteLine("homepage: " + homepage);
			Console.WriteLine("email: " + email);
			//Console.WriteLine("pic: " + picture.Substring(0, 5));
		}

		public string WriteToString()
		{

			return phoneno + "xoxox" + fullname + "xoxox" + homepage + "xoxox" + email + "xoxox" + picString + "xoxox";

		}
	}
	
}

