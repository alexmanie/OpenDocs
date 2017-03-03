using System;
using System.IO;

using Android.Content;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using OpenDocs.Droid;
using Xamarin.Forms;
using Android.App;


[assembly: Dependency(typeof(DocumentViewer_Droid))]
namespace OpenDocs.Droid
{
	public class DocumentViewer_Droid : IDocumentViewer
	{
		public void ShowDocumentFile(string filepath, string mimeType)
		{
			checkPermissions();

			var filename = filepath;
			string documentsPath = Android.OS.Environment.ExternalStorageDirectory.Path; // External folder
			var path = Path.Combine(documentsPath, filename);



			// This is where we copy the file
			Console.WriteLine("Path --->" + path);
			Console.WriteLine("Filename --->" + filename);

			if (!File.Exists(path))
			{

				var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.slides);  // RESOURCE NAME ###

				// create a write stream
				FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				// write to the stream
				ReadWriteStream(s, writeStream);
			}

			if (!File.Exists(path))
			{
				// ERROR
			}

			var uri = Android.Net.Uri.Parse("file://" + path);
			//var uri = Android.Net.Uri.Parse("file://" + filepath);
			var intent = new Intent(Intent.ActionView);
			intent.SetDataAndType(uri, mimeType);
			intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

			try
			{


				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
			}
			catch (Exception ex)
			{
				//Let the user know when something went wrong
				Console.WriteLine(ex.Message);
			}
		}

		private void checkPermissions()
		{
			const string readPermission = Android.Manifest.Permission.ReadExternalStorage;
			const string writePermission = Android.Manifest.Permission.WriteExternalStorage;

			if (ContextCompat.CheckSelfPermission(Xamarin.Forms.Forms.Context, readPermission) != (int)PermissionChecker.PermissionGranted
			   || ContextCompat.CheckSelfPermission(Xamarin.Forms.Forms.Context, writePermission) != (int)PermissionChecker.PermissionGranted)
			{
				askForPermissions(readPermission);
				askForPermissions(writePermission); //Might not be necessary
			}

		}

		private void askForPermissions(String permission)
		{
			if (ActivityCompat.ShouldShowRequestPermissionRationale((Activity)Xamarin.Forms.Forms.Context, permission))
			{

				// Show an expanation to the user *asynchronously* -- don't block
				// this thread waiting for the user's response! After the user
				// sees the explanation, try again to request the permission
			}
			else
			{
				// No explanation needed, we can request the permission.
				ActivityCompat.RequestPermissions((Activity)Xamarin.Forms.Forms.Context, new string[] { permission }, 1);
			}

		}
		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
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
	}
}
