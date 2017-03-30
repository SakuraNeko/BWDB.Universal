using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var View = ApplicationView.GetForCurrentView();
            var BackgroundColor = Color.FromArgb(255, 24, 70, 137);

            View.TitleBar.BackgroundColor = BackgroundColor;
            View.TitleBar.ButtonBackgroundColor = BackgroundColor;
            View.TitleBar.InactiveBackgroundColor = BackgroundColor;
            View.TitleBar.ButtonInactiveBackgroundColor = BackgroundColor;

            View.TitleBar.ForegroundColor = Colors.White;
            View.TitleBar.ButtonForegroundColor = Colors.White;
            View.TitleBar.InactiveForegroundColor = Colors.White;
            View.TitleBar.ButtonInactiveForegroundColor = Colors.White;

            PanelPageFrame.Navigate(typeof(ProductPage));
            MainPageFrame.Navigate(typeof(DetailPage));
        }
    }
}
