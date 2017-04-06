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
        SpriteVisual topSprite;

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
            Window.Current.SetTitleBar(TitleBarGrid);

            // 返回键事件
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            if (DeviceTypeState.CurrentState == Phone)
            {
                //设置手机状态栏
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = BackgroundColor;
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 1;
            }

            //加载BuildPage
            LeftPageFrame.Navigate(typeof(BuildPage));

            

            //获取系统版本号
            //sv是版本号字符串 十六进制下四位分组得到 A.B.C.D 的格式 如 10.0.15063.0
            var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;

            //RS2透明效果测试
            if (v3 >= 15031 && DeviceTypeState.CurrentState == Desktop)
            {
                compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

                //创建spriteVisual
                topSprite = compositor.CreateSpriteVisual();
                //spriteVisual的控制范围
                topSprite.Size = new System.Numerics.Vector2((float)PanelGrid.ActualWidth, (float)PanelGrid.ActualHeight);
                ElementCompositionPreview.SetElementChildVisual(PanelGrid, topSprite);

                /*
                var blurEffect = new GaussianBlurEffect
                {
                    Name = "Blur",
                    BlurAmount = 9.0f,
                    Source = new CompositionEffectSourceParameter("source")
                };

                //染色
                var colorEffect = new ColorSourceEffect
                {
                    Name = "Tint",
                    Color = BackgroundColor
                };

                //将染色特效和brush合成的“特效”
                var blendEffect = new BlendEffect
                {
                    Background = blurEffect,   //这里定义了一个参数 待会儿可以向这个参数传数据
                    Foreground = colorEffect,
                    Mode = BlendEffectMode.Overlay
                };

                //factory将特效整合
                var factory = compositor.CreateEffectFactory(blendEffect);
                var topBrush = factory.CreateBrush();

                var hostBackdropBrush = compositor.CreateHostBackdropBrush();
                topBrush.SetSourceParameter("source", hostBackdropBrush);
                */
                topSprite.Brush = compositor.CreateHostBackdropBrush();
                
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

            var isPhoneUI = (AdaptiveState.CurrentState == PhoneUI);
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

            if (topSprite !=null)
            {
                topSprite.Size = new System.Numerics.Vector2((float)PanelGrid.ActualWidth, (float)PanelGrid.ActualHeight);
            }
            
        }
    }
}
