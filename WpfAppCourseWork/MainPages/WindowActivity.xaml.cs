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
using System.Windows.Shapes;
using WpfAppCourseWork.ApplicationData;
using WpfAppCourseWork.MainPages;

namespace WpfAppCourseWork
{
    /// <summary>
    /// Логика взаимодействия для WindowActivity.xaml
    /// </summary>
    public partial class WindowActivity : Window
    {
        public WindowActivity()
        {
            InitializeComponent();
            AppConnect.ModelOdb = new Entities();
            AppFrame.fraimMain = FrmMain;

            AppFrame.fraimMain.Navigate(new ProductMain());

            if (SelectedUser.user == null)
            {
                UserName.Text += "Гость";
            }
            else
            {
                UserName.Text += SelectedUser.user.Login.ToString();
            }
        }

        private void Qiut_Btn_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser.user = null;

            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
