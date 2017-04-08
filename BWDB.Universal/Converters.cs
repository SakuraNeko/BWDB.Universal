using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BWDB.Universal
{
    public class MultiLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            ret = value as string;
            if (ret != null)
            {
                ret = ret.Replace("; ", ", ");
                ret = ret.Replace(", ", "\n");
            }
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
            if (ret != null)
            {
                ret = ret.Replace("); ", "), ");
                ret = ret.Replace("), ", ")\n");
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class MultiLineConverterForProductKey : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = "";
            ret = value as string;
            if (ret != null)
            {
                ret = ret.Replace(": ", ":\n");
                ret = ret.Replace("; ", ", ");
                ret = ret.Replace(", ", "\n\n");
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
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

    public class SplitViewThemeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ElementTheme theme;

            if((bool)value)
            {
                theme = ElementTheme.Light;
            }
            else
            {
                theme = ElementTheme.Dark;
            }

            return theme;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CodenameAndStageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var build = value as Core.Build;
            string str = "";

            if (build != null)
            {
                if (build.Codename == "" || build.Codename == "N/A")
                {
                    str = build.Stage;
                }
                else
                {
                    str = "Codename \"" + build.Codename + "\" - " + build.Stage;
                }
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class MoreButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string str = value as string;
            var ret = Visibility.Collapsed;

            if (str != null)
            {
                var c = new[] { '\n' };
                var subStrings = str.Split(c);
                if (subStrings.Count() > 5)
                {
                    ret = Visibility.Visible;
                }
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = Visibility.Collapsed;
            if (value != null)
            {
                bool a = (bool)value;
                if (a) ret = Visibility.Visible;
            }
            
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanReverseToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = Visibility.Visible;
            if (value != null)
            {
                bool a = (bool)value;
                if (a) ret = Visibility.Collapsed;
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
