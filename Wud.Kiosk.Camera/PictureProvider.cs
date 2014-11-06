using System.Collections.Generic;
using System.IO;

namespace Wud.Kiosk.Camera
{
    public class PictureProvider
    {
        public IList<string> GetFileNames(string directory)
        {
            // TODO: mtoporek: Dorobic extension method
            return Directory.GetFiles(directory);
        }
    }
}
