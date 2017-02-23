using Xamarin.Forms;

namespace OpenDocs
{
	public partial class OpenDocsPage : ContentPage
	{
		public OpenDocsPage()
		{
			InitializeComponent();
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

			DependencyService.Get<IDocumentViewer>().ShowDocumentFile(filepath, mimeType);
		}
	}
}
