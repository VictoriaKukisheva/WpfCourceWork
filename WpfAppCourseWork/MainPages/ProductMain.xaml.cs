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
using WpfAppCourseWork.AdminPages;
using WpfAppCourseWork.ApplicationData;

namespace WpfAppCourseWork.MainPages
{
    /// <summary>
    /// Логика взаимодействия для AptekaMain.xaml
    /// </summary>
    public partial class ProductMain : Page
    {
        /// <summary>
        /// Установка прав админа и комбобоксов
        /// </summary>
        public ProductMain()
        {
            InitializeComponent();

            ShowAdminFunctions();
            SetSortItems();
            SetFilterItems();

            LbProduct.ItemsSource = FindProduct();
        }

        /// <summary>
        /// Выгрузка элементов из бд и их сортировка
        /// </summary>
        /// <returns></returns>
        Product[] FindProduct()
        {
            List<Product> products = AppConnect.ModelOdb.Product.ToList();

            var productsAll = products;

            if (TbFinder.Text != null)
            {
                products = products.Where(x => x.Name.ToLower().Contains(TbFinder.Text.ToLower())).ToList();

                switch (CbSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.Cost).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }

            if (CbFilter.SelectedIndex > 0)
            {
                products = products.Where(x => x.Type.Name == CbFilter.SelectedItem.ToString()).ToList();
            }

            if (products.Count > 0)
            {
                TbFinderProduct.Text = "Найдено " + products.Count.ToString() + " из " + productsAll.Count().ToString();
            }
            else
            {
                TbFinderProduct.Text = "Элементы не найдены";
            }

            return products.ToArray();
        }

        /// <summary>
        /// Установка фильтров
        /// </summary>
        private void SetFilterItems()
        {
            CbFilter.Items.Add("< Тип мебели >");

            foreach (var item in AppConnect.ModelOdb.Type)
                CbFilter.Items.Add(item.Name);

            CbFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Установка фильтров
        /// </summary>
        private void SetSortItems()
        {
            CbSort.Items.Add("< Нет >");
            CbSort.Items.Add("Стоимость по возрастанию");
            CbSort.Items.Add("Стоимость по убыванию");

            CbSort.SelectedIndex = 0;
        }

        /// <summary>
        /// Проверка на админа
        /// </summary>
        private void ShowAdminFunctions()
        {
            if (SelectedUser.user == null || SelectedUser.user.IdRole != 1)
            {
                Addproduct.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Переход на страницу добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addproduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditproduct((sender as Button).DataContext as Product));
        }

        /// <summary>
        /// Изменение в текстовом поле считывает
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbProduct.ItemsSource = FindProduct();
        }

        /// <summary>
        /// То же самое , но в комбобоксе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbProduct.ItemsSource = FindProduct();
        }

        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbProduct.ItemsSource = FindProduct();
        }

        /// <summary>
        /// Изменение товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbProduct.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Role.Id == 1)
            {
                Product furniture = LbProduct.SelectedItem as Product;
                NavigationService.Navigate(new AddEditproduct(furniture));
            }
        }

        /// <summary>
        /// Перезагрузка таблицы, нужно, чтобы при добавлении или удалении таблица перезагружала данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelOdb.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            LbProduct.ItemsSource = FindProduct();
        }


        private List<Tuple<Product, int>> productsList = new List<Tuple<Product, int>>();

        public List<Tuple<Product, int>> GetProductsList()
        {
            return productsList;
        }
        //private void addToOrder_Click(object sender, RoutedEventArgs e)
        //{
        //    var product = LbProduct.SelectedItem as Product;
        //    // Проверяем, есть ли товар в списке
        //    var existingProduct = productsList.FirstOrDefault(p => p.Item1.Name == product.Name);

        //    if (existingProduct != null)
        //    {
        //        // Если товар уже есть, увеличиваем его количество
        //        int index = productsList.IndexOf(existingProduct);
        //        productsList[index] = new Tuple<Product, int>(existingProduct.Item1, existingProduct.Item2 + 1);
        //    }
        //    else
        //    {
        //        // Если товара нет, добавляем его в список с количеством 1
        //        productsList.Add(new Tuple<Product, int>(product, 1));
        //    }

        //    changeToOrderPage.Visibility = Visibility.Visible;
        //}

        //private void changeToOrderPage_Click(object sender, RoutedEventArgs e)
        //{
        //    AppFrame.fraimMain.Navigate(new Basket(productsList));

        //    if (productsList.Count == 0)
        //    {
        //        changeToOrderPage.Visibility = Visibility.Hidden;
        //    }
        //}
    }
}
