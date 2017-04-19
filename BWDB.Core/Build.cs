using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace BWDB.Core
{
    public class Screenshot
    {
        public string Author { get; set; }
        public string Description { get; set; }

        public BitmapImage Thumbnail { get; set; }
        public BitmapImage Image { get; set; }

        public Screenshot() { }
    }

    public class Build
    {
        public int ProductID { get; set; }
        public int BuildID { get; set; }
        public string Stage { get; set; }
        public string ProductName { get; set; }
        public string Codename { get; set; }
        public string Version { get; set; }
        public string Buildtag { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string SKU { get; set; }
        public string SerialNumber { get; set; }
        public string Fixes { get; set;}
        public string BIOSDate { get; set; }
        public int ScreenshotID { get; set; }

        public async Task<List<Screenshot>> GetSceenshots(StorageFolder screenshotFolder)
        {
                System.Diagnostics.Debug.WriteLine(ScreenshotID);

                StorageFolder folder;
                IReadOnlyList<StorageFile> filesList;
                try
                {
                    folder = await screenshotFolder.GetFolderAsync(ScreenshotID.ToString());
                    System.Diagnostics.Debug.WriteLine(folder.Path);
                    filesList = await folder.GetFilesAsync();
                }
                catch (Exception)
                {
                    return null;
                }
            
                var screenshotList = new List<Screenshot>();

            foreach (StorageFile file in filesList)
            {
                var stream = await file.OpenReadAsync();

                var thumbnail = new BitmapImage()
                {
                    DecodePixelHeight = 240,
                    //DecodePixelType = DecodePixelType.Logical
                };
                await thumbnail.SetSourceAsync(stream);

                
                var image = new BitmapImage();
                await image.SetSourceAsync(stream.CloneStream());

                var screenshot = new Screenshot()
                {
                    Author = "placeholder",
                    Description = file.DisplayName,
                    Thumbnail = thumbnail,
                    Image = image
                };

                //System.Diagnostics.Debug.WriteLine(screenshot.test);
                screenshotList.Add(screenshot);

                
                }

           
                return screenshotList;
            
        }

    }
    /*
    public class Build : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T field, T value, [CallerMemberName] string caller = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
            }
        }

        string productName;
        public string ProductName
        {
            get => productName;
            set => Set(ref productName, value);
        }

        string codename;
        public string Codename
        {
            get => codename;
            set => Set(ref codename, value);
        }

        string version;
        public string Version
        {
            get => version;
            set => Set(ref version, value);
        }

        string buildtag;
        public string Buildtag
        {
            get => buildtag;
            set => Set(ref buildtag, value);
        }

        DateTime biosdate;
        public DateTime BIOSDate
        {
            get => biosdate;
            set => Set(ref biosdate, value);
        }
    }
    */
}
