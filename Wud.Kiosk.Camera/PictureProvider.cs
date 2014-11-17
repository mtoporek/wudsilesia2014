using System.Collections.Generic;
using System.IO;

namespace Wud.Kiosk.Camera
{
    public class PictureProvider
    {
        public IList<string> GetFileNames(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("directory");
            }

            return Directory.GetFiles(directory);
        }
    }
}
