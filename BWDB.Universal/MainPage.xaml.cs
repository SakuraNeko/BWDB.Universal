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
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Composition;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.Foundation.Metadata;
using Microsoft.Graphics.Canvas.Effects;
using Windows.System.Profile;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage CurrentPage;
        Compositor compositor;
        SpriteVisual sprite;

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

                TitleBarRow.Height = new GridLength (0);
            }

            //加载BuildPage
            LeftPageFrame.Navigate(typeof(BuildPage));



            //获取系统版本号
            //sv是版本号字符串 十六进制下四位分组得到 A.B.C.D 的格式 如 10.0.15063.0
            var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;

            var isDesktop = (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop");

            //RS2透明效果测试
            if (v3 >= 15031 && isDesktop)
            {
                compositor = ElementCompositionPreview.GetElementVisual(TitleBarGrid).Compositor;
                sprite = compositor.CreateSpriteVisual();

                //透明区的宽高
                sprite.Size = new System.Numerics.Vector2((float)TitleBarGrid.ActualWidth, (float)TitleBarGrid.ActualHeight);
                ElementCompositionPreview.SetElementChildVisual(TitleBarGrid, sprite);

                var colorEffect = new ColorSourceEffect
                {
                    Name = "Tint",
                    Color = BackgroundColor
                };

                var blendEffect = new BlendEffect
                {
                    Background = new CompositionEffectSourceParameter("source"),
                    Foreground = colorEffect,
                    Mode = BlendEffectMode.SoftLight
                };

                var factory = compositor.CreateEffectFactory(
                    blendEffect
                    );

                var brush = factory.CreateBrush();


                brush.SetSourceParameter("source", compositor.CreateHostBackdropBrush());

                sprite.Brush = brush;
            }


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

            if (sprite !=null)
            {
                sprite.Size = new System.Numerics.Vector2((float)TitleBarGrid.ActualWidth, (float)TitleBarGrid.ActualHeight);
            }
            
        }
    }
}
