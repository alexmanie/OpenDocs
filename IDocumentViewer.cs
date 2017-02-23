using System;
namespace OpenDocs
{
	public interface IDocumentViewer
	{
		void ShowDocumentFile(string filepaht, string mimeType);
	}
}
