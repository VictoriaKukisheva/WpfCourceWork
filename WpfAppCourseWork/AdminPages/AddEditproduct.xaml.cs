using Microsoft.Win32;
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
using WpfAppCourseWork.ApplicationData;

namespace WpfAppCourseWork.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AddEditproduct.xaml
    /// </summary>
    public partial class AddEditproduct : Page
    {
        //Объект класса, с которым будем работать
        private Product _product = new Product();

        /// <summary>
        /// Проверяем пустой ли он и устанавливает фильтры
        /// </summary>
        /// <param name="product"></param>
        public AddEditproduct(Product product)
        {
            InitializeComponent();

            SetFilterSupplierItems();
            SetFilterTypeItems();
            if (product != null)
            {
                _product = product;
                FindFilterSupplierId();
                FindFilterTypeId();
            }
            else
            {
                FilterSupplier.SelectedIndex = 0;
                DeleteData.Visibility = Visibility.Hidden;
                AddData.Content = "Добавить продукт";
            }

            DataContext = _product;
        }

        private void FindFilterTypeId()
        {
            var item = AppConnect.ModelOdb.Type.FirstOrDefault(x => x.Id == _product.IdType);

            FilterType.SelectedItem = item.Name;
        }

        private void SetFilterTypeItems()
        {
            foreach (var item in AppConnect.ModelOdb.Type)
            {
                FilterType.Items.Add(item.Name);
            }
        }

        /// <summary>
        /// Ищем в бд тот элемент, который передали
        /// </summary>
        private void FindFilterSupplierId()
        {
            var supplierFurniture = AppConnect.ModelOdb.Supplier.FirstOrDefault(x => x.Id == _product.IdSupplier);

            FilterSupplier.SelectedItem = supplierFurniture.Name;
        }

        /// <summary>
        /// Загружает элементы из бд
        /// </summary>
        private void SetFilterSupplierItems()
        {
            foreach (var producers in AppConnect.ModelOdb.Supplier)
            {
                FilterSupplier.Items.Add(producers.Name);
            }
        }

        private void FindFilterSupplierItem()
        {
            var supplierFirniture = AppConnect.ModelOdb.Supplier.FirstOrDefault(x => x.Name == FilterSupplier.SelectedItem.ToString());
            
            _product.IdSupplier = supplierFirniture.Id;
        }

        /// <summary>
        /// Ввод только букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на пустые поля
            TextBox[] textBox = { NameProduct, Costtxt, Existancetxt, Descriptiontxt };
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(NameProduct.Text))
                errors.AppendLine("Не заполнено название");
            if (string.IsNullOrEmpty(Costtxt.Text))
                errors.AppendLine("Не заполнена цена");
            if (string.IsNullOrEmpty(Existancetxt.Text))
                errors.AppendLine("Не заполнено наличие");

            //Вывод ошибок
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Попытка добавления
            try
            {
                FindFilterSupplierItem();
                FindFilterTypeItem();

                if (_product.Id == 0)
                {
                    Entities.GetContext().Product.Add(_product);
                }

                Entities.GetContext().SaveChanges();

                AppConnect.ModelOdb.SaveChanges();
                MessageBox.Show("Данные успешно добавлены!");

                AppFrame.fraimMain.GoBack();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindFilterTypeItem()
        {
            var item = AppConnect.ModelOdb.Type.FirstOrDefault(x => x.Name == FilterType.SelectedItem.ToString());

            _product.IdType = item.Id;
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Показываем диалоговое окно подтверждения
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем товар из базы данных
                    AppConnect.ModelOdb.Product.Remove(_product);
                    AppConnect.ModelOdb.SaveChanges();

                    // Возвращаемся назад в навигации
                    NavigationService.GoBack();
                }
                else
                {
                    // Пользователь отменил удаление товара
                    MessageBox.Show("Удаление товара отменено.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Возращает на таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        /// <summary>
        /// Ввод только цифр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Existancetxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9,]"))
            {
                e.Handled = true;
            }
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                if (!(bool)openFileDialog1.ShowDialog())
                {
                    return;
                }

                openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                openFileDialog1.DefaultExt = ".jpg";

                // Присваиваем путь к файлу изображения объекту productOld.Image
                _product.Image = openFileDialog1.FileName;

                // Загружаем изображение и отображаем его в элементе Image в XAML
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(openFileDialog1.FileName);
                bitmapImage.EndInit();

                // Предполагается, что у вас есть элемент Image с именем myImage в XAML
                image.Source = bitmapImage;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
