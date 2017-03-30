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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BWDB.Universal
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ProductPage : Page
    {
        public ProductPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            GetProductList();
        }

        private void GetProductList()
            /* 
               index
               0 Year
               1 Product Family
               2 Product Line
            */
        {
            if (ZoomInListView == null) return;

            var OrigProducts = App.OSInformation.GetProducts();

            

            IEnumerable<IGrouping <string,Product>> Products = null;

            if (RadioButton_Year.IsChecked == true)
            {
                Products = OrigProducts.OrderBy(p => p.Year).GroupBy(p => p.Year.ToString());
            }
            else if (RadioButton_ProductLine.IsChecked == true)
            {
                Products = OrigProducts.OrderBy(p => p.TagID).ThenBy(p => p.ProductID).GroupBy(p => p.Tag);
            }
            else if (RadioButton_ProductFamily.IsChecked == true)
            {
                Products = OrigProducts.OrderBy(p => p.FamilyID).ThenBy(p => p.ProductID).GroupBy(p => p.Family);
            }

            
            

            var CollectionVS = new CollectionViewSource()
            {
                Source = Products,
                IsSourceGrouped = true
            };
            ZoomInListView.ItemsSource = CollectionVS.View;
            ZoomOutListView.ItemsSource = CollectionVS.View.CollectionGroups;

        }
        
        private void RadioButton_Year_Checked(object sender, RoutedEventArgs e)
        {
            GetProductList();
        }

        private void ZoomInListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var Product = ZoomInListView.SelectedItem as Product;
            MainPage.CurrentPage.LeftPageFrame.SetValue(Grid.ColumnProperty, 0);
            MainPage.CurrentPage.LeftPageFrame.SetValue(Grid.ColumnSpanProperty, 2);
            if (Product != null) MainPage.CurrentPage.LeftPageFrame.Navigate(typeof(BuildPage), Product.ProductID);
        }
        
    }
}