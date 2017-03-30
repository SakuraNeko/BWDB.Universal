using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BWDB.Universal
{
    public class MultiLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            ret = value as string;
            ret = ret.Replace("; ", ", ");
            ret = ret.Replace(", ", "\n");
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class MultiLineConverterForLanguageField : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            ret = value as string;
            ret = ret.Replace("); ", "), ");
            ret = ret.Replace("), ", ")\n");
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class BuildTagStripConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            if (value is string)
            {
                char[] c = { '.' };
                var str = value.ToString();
                var split = str.Split(c);
                if (split.Count()>=6)
                {
                    ret = split[4];
                }
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class VersionStripConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            if (value is string)
            {
                char[] c = { ' ' };
                var str = value.ToString();
                var split = str.Split(c);
                if (split.Count() >= 1)
                {
                    char[] c1 = { '.' };
                    var split1 = split[0].Split(c1);
                    if (split1.Count() == 4)
                    {
                        ret = split[0];
                    }
                    else
                    {
                        ret = str;
                    }
                }
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

}
