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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Display;
using System.Threading.Tasks;

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
        public static DetailPage CurrentPage;

        public DetailPage()
        {
            this.InitializeComponent();
            //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            CurrentPage = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Build)
            {
                CurrentBuild = e.Parameter as Build;
                var screenshotList = await CurrentBuild.GetSceenshots(App.ScreenshotFolder);

                if (screenshotList != null && screenshotList.Count > 0)
                {
                    ScreenshotView.ItemsSource = screenshotList;
                    ScreenshotFlipView.ItemsSource = screenshotList;
                    ScreenshotFlipView.SelectedItem = null;
                }
                else
                {
                    ScreenshotView.Visibility = Visibility.Collapsed;
                }

            }
        }




        private async void More_Click(object sender, RoutedEventArgs e)
        {
            var HeaderBinding = new Binding();
            var ContentBinding = new Binding();

            if (sender == SKUMoreButton)
            {
                HeaderBinding.Source = SKULabel;
                ContentBinding.Source = SKUText;
            }
            else if (sender == LanguageMoreButton)
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
            if (sender == DetailDialogCloseButton)
            {
                DetailDialog.Hide();
            }
            else if (sender == InstallationDialogCloseButton)
            {
                InstallationDialog.Hide();
            }
            else if (sender == ScreenshotDialogCloseButton)
            {
                ScreenshotFlipView.SelectionChanged -= ScreenshotFlipView_SelectionChanged;
                ScreenshotFlipView.SelectedItem = null;
                ScreenshotDialog.Hide();
            }

        }

        private async void InstallInformation_Click(object sender, RoutedEventArgs e)
        {
            await InstallationDialog.ShowAsync();
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("a");
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            var bitmap = image.Source as BitmapImage;
            var view = VisualTreeHelper.GetParent(image);

            if (VisualTreeHelper.GetParent(view) is GridViewItem gridViewItem)
            {
                int width = (int)((120.0f / bitmap.PixelHeight) * bitmap.PixelWidth);

                gridViewItem.Width = width;
                gridViewItem.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, width + 10);
            }
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var image = sender as Image;
            //System.Diagnostics.Debug.WriteLine(image.GetValue(VariableSizedWrapGrid.ColumnSpanProperty));
        }

        private async void ScreenshotView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ScreenshotFlipView.SelectedItem = (Screenshot)(e.ClickedItem);

            await ScreenshotDialog.ShowAsync();
            

            var list = (List<Screenshot>)ScreenshotFlipView.ItemsSource;


            //ScreenshotFlipView.SelectionChanged += ScreenshotFlipView_SelectionChanged;
            //ScreenshotView.SelectedItem = null;

            //var screenshot = (Screenshot)(e.ClickedItem);
            //ScreenshotFlipView.SelectedIndex = list.IndexOf(screenshot);

            //UpdateLayout();


///AdjustSize();
        }

        private void zoom_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        private void image_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void image_ImageOpened_1(object sender, RoutedEventArgs e)
        {

        }

        private void AdjustSize()
        {
            /*
            if (VisualTreeHelper.GetChildrenCount(ScreenshotFlipView) == 0) return;
            if ((ScreenshotFlipView).SelectedItem == null) return;

            //FlipView下的Grid
            var grid = (Grid)VisualTreeHelper.GetChild(ScreenshotFlipView, 0);

            //FlipView中隐藏的ScrollViewer
            var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(grid, 0);
            var border = (Border)VisualTreeHelper.GetChild(scrollViewer, 0);
            grid = (Grid)VisualTreeHelper.GetChild(border, 0);
            var scrollContentPresenter = (ScrollContentPresenter)VisualTreeHelper.GetChild(grid, 0);

            //FlipView的Content和ItemsHost
            var itemsPresenter = (ItemsPresenter)VisualTreeHelper.GetChild(scrollContentPresenter, 0);
            var virtualizingStackPanel = (VirtualizingStackPanel)VisualTreeHelper.GetChild(itemsPresenter, 1);

            FlipViewItem selectedItem = null;
            foreach (FlipViewItem item in virtualizingStackPanel.Children)
            {
                if (item.IsSelected)
                {
                    selectedItem = item;
                }
            }

            if (selectedItem == null) return;
            //FlipViewItem中自定义的ScrollViewer
            var contentPresenter = (ContentPresenter)VisualTreeHelper.GetChild(selectedItem, 0);
            scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(contentPresenter, 0);
            border = (Border)VisualTreeHelper.GetChild(scrollViewer, 0);
            grid = (Grid)VisualTreeHelper.GetChild(border, 0);
            scrollContentPresenter = (ScrollContentPresenter)VisualTreeHelper.GetChild(grid, 0);


            scrollViewer.ChangeView(null, null, 1);

            //ScrollViewer中的图
            var image = (Image)VisualTreeHelper.GetChild(scrollContentPresenter, 0);
            //var flipViewItem = (FlipViewItem)VisualTreeHelper.GetChild(virtualizingStackPanel, ((FlipView)sender).SelectedIndex);
            var bitmap = (BitmapImage)image.Source;

            var parentWidth = scrollViewer.ActualWidth;
            var parentHeight = scrollViewer.ActualHeight;
            System.Diagnostics.Debug.WriteLine(parentHeight);
            var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            image.MaxHeight = parentHeight;
            image.MaxWidth = parentWidth;

            if (bitmap != null)
            {
                if (bitmap.PixelWidth / scale <= parentWidth && bitmap.PixelHeight / scale <= parentHeight)
                {
                    image.Width = bitmap.PixelWidth / scale;
                    image.Height = bitmap.PixelHeight / scale;
                }
            }
            */
        }
        private void ScreenshotFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdjustSize();

        }

        private void scrollViewer_KeyUp(object sender, KeyRoutedEventArgs e)
        {
        }
    }
}
