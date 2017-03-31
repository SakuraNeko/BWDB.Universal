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
using Windows.UI.Core;
using Windows.Foundation.Metadata;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage CurrentPage;

        public MainPage()
        {
            this.InitializeComponent();
            CurrentPage = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var View = ApplicationView.GetForCurrentView();
            var BackgroundBrush = (SolidColorBrush)Application.Current.Resources["BackgroundAccentBrush"];

            View.TitleBar.BackgroundColor = BackgroundBrush.Color;
            View.TitleBar.ButtonBackgroundColor = BackgroundBrush.Color;
            View.TitleBar.InactiveBackgroundColor = BackgroundBrush.Color;
            View.TitleBar.ButtonInactiveBackgroundColor = BackgroundBrush.Color;

            View.TitleBar.ForegroundColor = Colors.White;
            View.TitleBar.ButtonForegroundColor = Colors.White;
            View.TitleBar.InactiveForegroundColor = Colors.White;
            View.TitleBar.ButtonInactiveForegroundColor = Colors.White;

            LeftPageFrame.Navigate(typeof(BuildPage));

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                System.Diagnostics.Debug.WriteLine("OK");
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = BackgroundBrush.Color;
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 1;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var a = ((int)LeftPageFrame.GetValue(Grid.ColumnSpanProperty) == 2);
            var b = (MainPageFrame.Visibility == Visibility.Visible);

            if (a && b)
            {
                e.Handled = true;
                LeftPageFrame.Visibility = Visibility.Visible ;
                MainPageGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var View = SystemNavigationManager.GetForCurrentView();

            //hack
            //a: 当前是手机UI
            //b: 当前在详情页
            //以后肯定找个更靠谱的方式！！！
            var a = ((int)LeftPageFrame.GetValue(Grid.ColumnSpanProperty) == 2);
            var b = (MainPageGrid.Visibility == Visibility.Visible);

            if ( a && b )
            {
                LeftPageFrame.Visibility = Visibility.Collapsed;
                View.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                LeftPageFrame.Visibility = Visibility.Visible;
                View.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
