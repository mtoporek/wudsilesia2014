using FlickrNet;

namespace Wud.Kiosk.Socials.FlickrGallery
{
    public class FlickrManager
    {
        // API Key only for wudsilesia@yahoo.com account
        private const string ApiKey = "87f3a3732219b0ea7f5d183252a7c689";
        private const string SharedSecret = "a63923ca4ca1faa0";

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            var flickr = new Flickr(ApiKey, SharedSecret)
                        {
                            OAuthAccessToken = OAuthToken.Token,
                            OAuthAccessTokenSecret = OAuthToken.TokenSecret
                        };
            return flickr;
        }

        public static OAuthAccessToken OAuthToken { get; set; }
    }
}
