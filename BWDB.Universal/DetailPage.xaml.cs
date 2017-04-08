using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using BWDB.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        //private Build currentBuild;
        public Build CurrentBuild { get; set; }

        public DetailPage()
        {
            this.InitializeComponent();
            //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Build)
            {
                CurrentBuild = e.Parameter as Build;
            }
        }

        private async void More_Click(object sender, RoutedEventArgs e)
        {
            var HeaderBinding = new Binding();
            var ContentBinding = new Binding();

            if (Equals(sender, SKUMoreButton))
            {
                HeaderBinding.Source = SKULabel;
                ContentBinding.Source = SKUText;
            }
            else if (Equals(sender, LanguageMoreButton))
            {
                HeaderBinding.Source = LanguageLabel;
                ContentBinding.Source = LanguageText;
            }
            else
            {
                HeaderBinding = null;
                ContentBinding = null;
            }

            HeaderBinding.Path = new PropertyPath("Text");
            ContentBinding.Path = new PropertyPath("Text");

            DetailDialogHeaderBlock.SetBinding(TextBlock.TextProperty, HeaderBinding);
            DetailDialogTextBlock.SetBinding(TextBlock.TextProperty, ContentBinding);

            await DetailDialog.ShowAsync();
        }


        private void DetailDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {/*
            var BackgroundColor = ((SolidColorBrush)Application.Current.Resources["BackgroundAccentBrush"]).Color;
            var coreViewTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            appTitleBar.ButtonBackgroundColor = BackgroundColor;
            appTitleBar.ButtonInactiveBackgroundColor = BackgroundColor;
            coreViewTitleBar.ExtendViewIntoTitleBar = false;*/

        }

        private void DetailDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            /*
            var coreViewTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            coreViewTitleBar.ExtendViewIntoTitleBar = true;*/
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DetailDialog.Hide();
        }


    }
}
