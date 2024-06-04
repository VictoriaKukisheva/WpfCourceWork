using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfAppCourseWork
{
    /// <summary>
    /// Логика взаимодействия для RegistActivityWindow.xaml
    /// </summary>
    public partial class RegistActivityWindow : Window
    {
        //private User _user = new User();

        public RegistActivityWindow()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            //Проверки на пустые поля и почту с номером
            if (string.IsNullOrEmpty(SurnameTxt.Text))
                errors.AppendLine("Не заполнена фамилия");
            if (string.IsNullOrEmpty(NameTxt.Text))
                errors.AppendLine("Не заполнено имя");
            if (string.IsNullOrEmpty(SurnameTxt1.Text))
                errors.AppendLine("Не заполнено отчество");
            if (string.IsNullOrEmpty(PhoneTxt.Text))
                errors.AppendLine("Не заполнен телефон");
            if (string.IsNullOrEmpty(AddressTxt.Text))
                errors.AppendLine("Не заполнен адрес");
            if (string.IsNullOrEmpty(LoginTxt.Text))
                errors.AppendLine("Не заполнен логин");
            if (string.IsNullOrEmpty(PasswordTxt.Password))
                errors.AppendLine("Не заполнен пароль");

            if (!string.IsNullOrEmpty(PasswordTxt.Password) && !Regex.IsMatch(PasswordTxt.Password, @"((?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9!@#$%^&*a-zA-Z]{6,})")
                && !string.IsNullOrEmpty(PasswordTxt.Password))
                errors.AppendLine("Введите корректный пароль");
            if (AppConnect.ModelOdb.User.Count(x => x.Login == LoginTxt.Text) > 0 )
                errors.AppendLine("Пользователь с таким логином уже есть");
            if ((PhoneTxt.Text.Length != 11 || !PhoneTxt.Text.StartsWith("7"))
                && !string.IsNullOrEmpty(PhoneTxt.Text))
                errors.AppendLine("Номер телефона начинается с 7 и не больше 11 символов");

            //Вывод всех ошибок
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Попытка добавления пользователя
            try
            {
                Person personObj = new Person()
                {
                    Surname = SurnameTxt.Text,
                    Name = NameTxt.Text,
                    Secondname = SurnameTxt1.Text,
                    Phone = Convert.ToInt64(PhoneTxt.Text).ToString(),
                    Address = AddressTxt.Text
                };

                AppConnect.ModelOdb.Person.Add(personObj);
                AppConnect.ModelOdb.SaveChanges();

                int ert = (from m in AppConnect.ModelOdb.Person select m.Id).ToList().Last();

                User userObj = new User()
                {
                    Login = LoginTxt.Text,
                    Password = PasswordTxt.Password.ToString(),
                    IdPerson = ert,
                    IdRole = 3
                };


                AppConnect.ModelOdb.User.Add(userObj);
                AppConnect.ModelOdb.SaveChanges();

                //Передает нового пользователя в окно
                SelectedUser.user = userObj;

                WindowActivity activity = new WindowActivity();
                activity.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных! " + ex.Message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                DblPasswordTxt.Clear();
            }
        }

        /// <summary>
        /// Возвращение в авторизацию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        /// <summary>
        /// Ввод только букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Ввод только цифр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Проверка совпадения пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DblPasswordTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordTxt.Password != DblPasswordTxt.Password)
            {
                Registration.IsEnabled = false;
                DblPasswordTxt.Background = Brushes.LightCoral;
                DblPasswordTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                Registration.IsEnabled = true;
                DblPasswordTxt.Background = Brushes.LightGreen;
                DblPasswordTxt.BorderBrush = Brushes.Green;
            }
        }
    }
}
