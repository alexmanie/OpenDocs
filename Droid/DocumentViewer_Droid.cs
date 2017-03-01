using System;
using System.IO;
using Android.Content;
using Android.Support.V7.App;
using OpenDocs.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DocumentViewer_Droid))]
namespace OpenDocs.Droid
{
	public class DocumentViewer_Droid : IDocumentViewer
	{
		public void ShowDocumentFile(string filepath, string mimeType)
		{
			var filename = filepath;
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, filename);



			// This is where we copy the file
			Console.WriteLine(path);

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
				{ 
					new AlertDialog.Builder(Xamarin.Forms.Forms.Context)
					               .SetTitle("path")
					               .SetMessage(path)
					               .Show();
				}


				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
			}
			catch (Exception ex)
			{
				//Let the user know when something went wrong
				Console.WriteLine(ex.Message);
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
