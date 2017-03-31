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
using BWDB.Core;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BuildPage : Page
    {
        public Product CurrentProduct;

        public BuildPage()
        {
            this.InitializeComponent();
        }

        public void GetBuildList(int ProductID)
        {
            BuildZoomInListView.SelectionChanged -= BuildZoomInListView_SelectionChanged;

            var Builds = App.OSInformation.GetBuildsInProduct(ProductID);
            var groupedBuilds = Builds.OrderBy(p => p.BuildID).GroupBy(p => p.Stage);

            var CollectionViewSource = new CollectionViewSource();
            CollectionViewSource.Source = groupedBuilds;
            CollectionViewSource.IsSourceGrouped = true;

            BuildZoomInListView.ItemsSource = CollectionViewSource.View;
            BuildZoomOutListView.ItemsSource = CollectionViewSource.View.CollectionGroups;

            BuildZoomInListView.SelectedItem = null;

            BuildZoomInListView.SelectionChanged += BuildZoomInListView_SelectionChanged;
        }

        private void GetProductList()
        {
            if (ProductZoomInListView == null) return;

            //hack，防止刷新ProductList时发生SelectionChanged 然后列表被隐藏
            ProductZoomInListView.SelectionChanged -= ProductZoomInListView_SelectionChanged;

            var Products = App.OSInformation.GetProducts();
            IEnumerable<IGrouping<string, Product>> groupedProducts = null;

            //判断分类模式
            if (RadioButton_Year.IsChecked == true)
            {
                groupedProducts = Products.OrderBy(p => p.Year).GroupBy(p => p.Year.ToString());
            }
            else if (RadioButton_ProductLine.IsChecked == true)
            {
                groupedProducts = Products.OrderBy(p => p.TagID).ThenBy(p => p.ProductID).GroupBy(p => p.Tag);
            }
            else if (RadioButton_ProductFamily.IsChecked == true)
            {
                groupedProducts = Products.OrderBy(p => p.FamilyID).ThenBy(p => p.ProductID).GroupBy(p => p.Family);
            }

            var CollectionVS = new CollectionViewSource()
            {
                Source = groupedProducts,
                IsSourceGrouped = true
            };
            ProductZoomInListView.ItemsSource = CollectionVS.View;
            ProductZoomOutListView.ItemsSource = CollectionVS.View.CollectionGroups;

            ProductZoomInListView.SelectedItem = null;

            //恢复SelectionChanged
            ProductZoomInListView.SelectionChanged += ProductZoomInListView_SelectionChanged;
        }
        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProductList();

            //第一次启动的时候强制刷新一下SelectItem
            if (ProductZoomInListView != null)
            {
                ProductZoomInListView.SelectedItem = ProductZoomInListView.Items[0];
            }

            //SystemNavigationManager.GetForCurrentView().BackRequested += BuildPage_BackRequested;
        }

        private void BuildPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var a = ((int)MainPage.CurrentPage.LeftPageFrame.GetValue(Grid.ColumnSpanProperty) == 2);
            var b = (MainPage.CurrentPage.MainPageFrame.Visibility == Visibility.Visible);

            if (a && b)
            {
                MainPage.CurrentPage.LeftPageFrame.Visibility = Visibility.Visible;
                MainPage.CurrentPage.MainPageGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            GetProductList();
        }

        private void BuildZoomInListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Build = BuildZoomInListView.SelectedItem as Build;
            if (Build != null)
            {
                var View = SystemNavigationManager.GetForCurrentView();

                if ((int)MainPage.CurrentPage.LeftPageFrame.GetValue(Grid.ColumnSpanProperty) == 2)
                {
                    MainPage.CurrentPage.LeftPageFrame.Visibility = Visibility.Collapsed;
                    View.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }

                MainPage.CurrentPage.MainPageFrame.Navigate(typeof(DetailPage), Build);
                MainPage.CurrentPage.MainPageGrid.Visibility = Visibility.Visible;
            }
        }

        private void ProductZoomInListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Product = ProductZoomInListView.SelectedItem as Product;
            if (Product != null)
            {
                GetBuildList(Product.ProductID);

                //值为1时是PC模式
                if ((int)MainPage.CurrentPage.LeftPageFrame.GetValue (Grid.ColumnSpanProperty) == 1)
                {
                    BuildZoomInListView.SelectedItem = BuildZoomInListView.Items[0];
                }

                SplitView.IsPaneOpen = false;
            }
        }

    }
}
