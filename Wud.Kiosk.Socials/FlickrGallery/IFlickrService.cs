using System.IO;

namespace Wud.Kiosk.Socials.FlickrGallery
{
    public interface IFlickrService
    {
        void Authenticate();

        bool CompleteAuthentication(string code);

        void Upload(Stream stream, string fileName, string title, string description, string tags);
    }
}
