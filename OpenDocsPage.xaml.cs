using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace OpenDocs
{
	public partial class OpenDocsPage : ContentPage
	{
		public OpenDocsPage()
		{
			InitializeComponent();

			this.lblVersion.Text = CrossDeviceInfo.Current.Version;
			this.lblPlatform.Text = CrossDeviceInfo.Current.Platform.ToString();

		}

		void HandlePDF_Clicked(object sender, System.EventArgs e)
		{
			string filepath = "slides.pdf";
			string mimeType = "application/pdf";

			DependencyService.Get<IDocumentViewer>().ShowDocumentFile(filepath, mimeType);
		}

		void HandleExcel_Clicked(object sender, System.EventArgs e)
		{
			string filepath = "ManageMyMoney1.xlsx";
			string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			//string mimeType = "application/vnd.ms-excel";

			DependencyService.Get<IDocumentViewer>().ShowDocumentFile(filepath, mimeType);
		}

		void HandleWord_Clicked(object sender, System.EventArgs e)
		{
			string filepath = "Document1.docx";
			string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
			//string mimeType = "application/msword";

			DependencyService.Get<IDocumentViewer>().ShowDocumentFile(filepath, mimeType);
		}
	}
}
