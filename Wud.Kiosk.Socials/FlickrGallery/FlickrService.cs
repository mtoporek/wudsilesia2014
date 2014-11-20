using System;
using System.IO;
using System.Threading.Tasks;

using FlickrNet;

namespace Wud.Kiosk.Socials.FlickrGallery
{
    public class FlickrService : IFlickrService
    {
        private OAuthRequestToken requestToken;

        public void Authenticate()
        {
            Flickr flickr = FlickrManager.GetInstance();
            requestToken = flickr.OAuthGetRequestToken("oob");

            string url = flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);
            System.Diagnostics.Process.Start(url);
        }

        public bool CompleteAuthentication(string code)
        {
            Flickr flickr = FlickrManager.GetInstance();

            try
            {
                var accessToken = flickr.OAuthGetAccessToken(this.requestToken, code);
                FlickrManager.OAuthToken = accessToken;
                return true;
            }
            catch (FlickrApiException)
            {
                return false;
            }
        }

        public void Upload(Stream stream, string fileName, string title, string description, string tags)
        {
            if (FlickrManager.OAuthToken == null)
            {
                return;
            }

            Flickr flickr = FlickrManager.GetAuthInstance();

            var task = new Task<string>(
                () =>
                    {
                        string photoId = flickr.UploadPicture(fileName, title, description, tags);
                        return photoId;
                    });

            task.Start();
        }
    }
}