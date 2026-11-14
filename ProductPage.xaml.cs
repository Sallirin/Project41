using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace Сытник41
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {

        
        public ProductPage()
        {
            InitializeComponent();
            var currentProducts = Сытник41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            ComboType.SelectedIndex = 0;
            int AllRecords = Сытник41Entities.GetContext().Product.ToList().Count;
            TBAllRecords.Text = " из " + AllRecords.ToString();
            UpdateProducts();
        }

        public ProductPage(User user)
        {
            InitializeComponent();
            FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            switch (user.UserRole)
            {
                case 1:
                    RoleTB.Text = "Клиент";break;
                case 2:
                    RoleTB.Text = "Менеджер";break;
                case 3:
                    RoleTB.Text = "Администратор";break;
            }
            var currentProducts = Сытник41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
        }
        public ProductPage(String role)
        {
            InitializeComponent();
            AuthTB.Text = "Вы не авторизованы";
            RoleTB.Text = role;
            var currentProducts = Сытник41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
        }
        private void UpdateProducts()
        {
            var currentProducts = Сытник41Entities.GetContext().Product.ToList();
            if (ComboType.SelectedIndex == 0)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100)).ToList();

            }
            if (ComboType.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100)).ToList();
            }
            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            if (RButtonDown.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            if (RButtonUp.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }
            ProductListView.ItemsSource = currentProducts;
            int currentrecords = currentProducts.Count;
            TBCount.Text = "Количество " + currentrecords.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void ShoeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
