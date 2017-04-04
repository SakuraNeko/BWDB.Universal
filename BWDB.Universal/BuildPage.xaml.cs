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
using System.Collections.ObjectModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BuildPage : Page
    {
        static ObservableProduct currentProduct = new ObservableProduct();
        public static ObservableProduct CurrentProduct { get => currentProduct; set => currentProduct = value; }

        public BuildPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProductList();

            //第一次启动的时候强制刷新一下SelectItem
            if (ProductZoomInListView != null)
            {
                ProductZoomInListView.SelectedItem = ProductZoomInListView.Items[0];
            }
        }

        //获取BuildList
        public void GetBuildList(int ProductID)
        {
            var Builds = App.OSInformation.GetBuildsInProduct(ProductID);
            var groupedBuilds = Builds.OrderBy(p => p.BuildID).GroupBy(p => p.Stage);

            var CollectionViewSource = new CollectionViewSource();
            CollectionViewSource.Source = groupedBuilds;
            CollectionViewSource.IsSourceGrouped = true;

            BuildZoomInListView.ItemsSource = CollectionViewSource.View;
            BuildZoomOutListView.ItemsSource = CollectionViewSource.View.CollectionGroups;

            BuildZoomInListView.SelectedItem = null;
        }

        //获取ProductList
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
        
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            GetProductList();
        }
        
        private void ProductZoomInListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentProduct.Product = ProductZoomInListView.SelectedItem as Product;

            if (CurrentProduct.Product != null)
            {
                GetBuildList(CurrentProduct.Product.ProductID);

                var isPhoneUI = (MainPage.CurrentPage.ActualWidth < 640);
                if (!isPhoneUI && BuildZoomInListView.Items.Count>0)
                {
                    NavigateToBuild(BuildZoomInListView.Items[0] as Build);
                }

                SplitView.IsPaneOpen = false;
            }
        }

        private void BuildZoomInListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var Build = e.ClickedItem as Build;
            NavigateToBuild(Build);
          
        }
        private void NavigateToBuild(Build build)
        {
            if (build != null)
            {
                var View = SystemNavigationManager.GetForCurrentView();

                var isPhoneUI = (MainPage.CurrentPage.ActualWidth < 640);
                if (isPhoneUI)
                {
                    MainPage.CurrentPage.LeftPageFrame.Visibility = Visibility.Collapsed;
                    View.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }

                MainPage.CurrentPage.MainPageFrame.Navigate(typeof(DetailPage), build);
                MainPage.CurrentPage.MainPageFrame.Visibility = Visibility.Visible;
            }

        }
    }
}
