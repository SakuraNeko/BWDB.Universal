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
            var BackgroundColor = ((SolidColorBrush)Application.Current.Resources["BackgroundAccentBrush"]).Color;

            //设置标题栏
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            var coreViewTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;

            appTitleBar.BackgroundColor = BackgroundColor;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appTitleBar.InactiveBackgroundColor = BackgroundColor;
            appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            appTitleBar.ForegroundColor = Colors.White;
            appTitleBar.ButtonForegroundColor = Colors.White;
            appTitleBar.InactiveForegroundColor = Colors.White;
            appTitleBar.ButtonInactiveForegroundColor = Colors.White;

            coreViewTitleBar.ExtendViewIntoTitleBar = true;

            // 返回键事件
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;


            //设置手机状态栏
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                System.Diagnostics.Debug.WriteLine("OK");
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = BackgroundColor;
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 1;
            }

            //加载BuildPage
            LeftPageFrame.Navigate(typeof(BuildPage));
            
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var navigationView = SystemNavigationManager.GetForCurrentView();

            if (MainPageFrame.Visibility == Visibility.Visible)
            {
                MainPageFrame.Visibility = Visibility.Collapsed;
                LeftPageFrame.Visibility = Visibility.Visible;

                navigationView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                e.Handled = true;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var navigationView = SystemNavigationManager.GetForCurrentView();
            
            var isPhoneUI = (ActualWidth < 640);
            var isMainPageVisible = (MainPageFrame.Visibility == Visibility.Visible);

            if (isPhoneUI && isMainPageVisible)
            {
                LeftPageFrame.Visibility = Visibility.Collapsed;
                navigationView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                LeftPageFrame.Visibility = Visibility.Visible;
                navigationView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            
        }
    }
}
