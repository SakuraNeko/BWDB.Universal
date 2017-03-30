using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BWDB.Core
{
    public class Build
    {
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
