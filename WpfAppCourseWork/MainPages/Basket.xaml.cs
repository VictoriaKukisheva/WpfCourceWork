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

namespace WpfAppCourseWork.MainPages
{
    
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Page
    {
        private List<Tuple<Product, int>> _products = new List<Tuple<Product, int>>();
        public Basket(List<Tuple<Product, int>> products)
        {
            InitializeComponent();
            _products = products;
            LbProduct.ItemsSource = _products;
        }
    }
}
