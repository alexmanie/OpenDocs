using System;
using Xamarin.Forms;
using OpenDocs.iOS;
using System.IO;
using QuickLook;
using UIKit;
using Foundation;
using System.Linq;

[assembly: Dependency(typeof(DocumentViewer_iOS))]
namespace OpenDocs.iOS
{
	public class DocumentViewer_iOS : IDocumentViewer
	{
		public void ShowDocumentFile(string filepath, string mimeType)
		{

			var filename = filepath;
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine(libraryPath, filename);

			// This is where we copy in the prepopulated database
			Console.WriteLine(path);

			if (!File.Exists(path))
			{
				File.Copy(filename, path);
			}


			var fileinfo = new FileInfo(path);
			var previewController = new QLPreviewController();
			previewController.DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name);

			//For iOS 10.X onward & for iPhone u can use[[self.navigationController toolbar]
			//setHidden:YES]; This is objective C code u can do similar in Your case.I am able to hide share/action button which appears at bottom using this line of Code.

			//var items = previewController.ToolbarItems;
			//previewController.NavigationItem.RightBarButtonItem =null;

			//UINavigationController controller = FindNavigationController();

			//if (controller != null)
			//{
			//	controller.PresentViewController((UIViewController)previewController, true, (Action)null);
			//}

			// You can present the controller directly...
			//this.Window.RootViewController = new UINavigationController(preview);

			// ...or add its view to your own view hierarchy.
			//this.Window.RootViewController = new UIViewController();
			//this.Window.RootViewController.View.AddSubview(preview.View);


			UIApplication.SharedApplication.KeyWindow.RootViewController.View.AddSubview(previewController.View);
			UIApplication.SharedApplication.KeyWindow.RootViewController.ShowViewController(previewController, null);

			//UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(previewController, true, null);
		}

		private UINavigationController FindNavigationController()
		{
			foreach (var window in UIApplication.SharedApplication.Windows)
			{
				if (window.RootViewController.NavigationController != null)
				{
					return window.RootViewController.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(window.RootViewController.ChildViewControllers);
					if (value != null)
					{
						return value;
					}
				}
			}

			return null;
		}

		private UINavigationController CheckSubs(UIViewController[] controllers)
		{
			foreach (var controller in controllers)
			{
				if (controller.NavigationController != null)
				{
					return controller.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(controller.ChildViewControllers);

					if (value != null)
					{
						return value;
					}
				}

				return null;
			}

			return null;
		}
	}

	public class DocumentItem : QLPreviewItem
	{
		private string _title;
		private string _uri;

		public DocumentItem(string title, string uri)
		{
			_title = title;
			_uri = uri;
		}

		public override string ItemTitle
		{ get { return _title; } }

		public override NSUrl ItemUrl
		{ get { return NSUrl.FromFilename(_uri); } }
	}

	public class PreviewControllerDataSource : QLPreviewControllerDataSource
	{
		private string _url;
		private string _filename;

		public PreviewControllerDataSource(string url, string filename)
		{
			_url = url;
			_filename = filename;
		}

		public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
		{
			return (IQLPreviewItem)new DocumentItem(_filename, _url);
		}

		public override nint PreviewItemCount(QLPreviewController controller)
		{ 
			return (nint)1; 
		}
	}
}
