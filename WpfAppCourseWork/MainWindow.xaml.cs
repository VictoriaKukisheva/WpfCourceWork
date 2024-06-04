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
using WpfAppCourseWork.MainPages;

namespace WpfAppCourseWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.ModelOdb = new Entities();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //Обработка введенных данных в поля логина и пароля
            try
            {
                var userObj = AppConnect.ModelOdb.User.FirstOrDefault(x => x.Login == txtLogin.Text && x.Password == txtPassword.Password);

                //Если такого пользователя не найдено в бд - появляется капча
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Capcha capcha = new Capcha();
                    capcha.ShowDialog();
                }

                //Если есть логин или пароль такой в бд
                else if (userObj.Login == txtLogin.Text && txtPassword.Password == null)
                {
                    MessageBox.Show("Пользователь с таким логином уже есть!", "Ошибка при авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //Входит дальше в другое окно и передает пользователя
                else
                {
                    SelectedUser.user = userObj;

                    WindowActivity activity = new WindowActivity();
                    activity.Show();

                    Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Переход в окно регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Regist_Click(object sender, RoutedEventArgs e)
        {
            RegistActivityWindow activityWindow = new RegistActivityWindow();
            activityWindow.Show();

            Close();
        }

        /// <summary>
        /// Вход под пустым пользователем - гостем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser.user = null;

            WindowActivity activity = new WindowActivity();
            activity.Show();

            Close();
        }
    }
}
