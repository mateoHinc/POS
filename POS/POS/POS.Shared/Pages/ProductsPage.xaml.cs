using POS.Components;
using POS.Dialogs;
using POS.Helpers;
using POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace POS.Pages
{
    public sealed partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<Product> Products { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadProductsAsync();
        }

        private async void LoadProductsAsync()
        {
            Loader loader = new Loader("Por Favor Espere...");
            loader.Show();
            Response response = await ApiService.GetListAsync<Product>("products", MainPage.GetInstance().TokenResponse.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Product> products = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private void RefreshList()
        {
            ProductsListView.ItemsSource = null;
            ProductsListView.Items.Clear();
            ProductsListView.ItemsSource = Products;
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            ProductDialog dialog = new ProductDialog(product);
            await dialog.ShowAsync();
            if (!product.WasSaved)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync("Products", product, MainPage.GetInstance().TokenResponse.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Product newProduct = (Product)response.Result;
            Products.Add(newProduct);
            RefreshList();
        }
    }
}
