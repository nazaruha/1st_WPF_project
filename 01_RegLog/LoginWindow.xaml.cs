using _01_RegLog.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace _01_RegLog
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        bool flagChengeView = false;
        MyDataContext data = new MyDataContext();


        public LoginWindow()
        {
            InitializeComponent();
        }

        private void SignIn_btn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtEmail.Text) && String.IsNullOrWhiteSpace(txtPasswordHidden.Password)) return;
            bool checkError = false;
            if (data.Users.Count() != 0)
            {
                foreach (var user in data.Users)
                {
                    if (txtEmail.Text == user.Email)
                    {
                        if (checkPassword("Uncorrect Password", user))
                        {
                            clearWindow();
                            UserWindow userWindow = new UserWindow(user);
                            userWindow.ShowDialog();
                        }
                        checkError = true;
                        return;
                    }
                    else if (txtEmail.Text != user.Email)
                    {
                        checkPassword("Uncorrect Email", user);
                    }
                }
                if (!checkError)
                {
                    MessageBox.Show("Uncorrect Email or Password", "Invalid User's data", MessageBoxButton.OK);
                    return;
                }
                
            }
        }

        private bool checkPassword(string error_text, User user)
        {
            if (error_text == "Uncorrect Password")
            {
                if (!flagChengeView)
                {
                    if (txtPasswordHidden.Password != user.Password)
                    {
                        MessageBox.Show(error_text, "Invalid Password", MessageBoxButton.OK);
                        return false;
                    }
                }
                else
                {
                    if (txtPasswordShown.Text != user.Password)
                    {
                        MessageBox.Show(error_text, "Invalid Password", MessageBoxButton.OK);
                        return false;
                    }
                }
            }
            else if (error_text == "Uncorrect Email")
            {
                if (!flagChengeView)
                {
                    if (txtPasswordHidden.Password == user.Password)
                    {
                        MessageBox.Show(error_text, "Invalid Email", MessageBoxButton.OK);
                        return false;
                    }
                }
                else
                {
                    if (txtPasswordShown.Text == user.Password)
                    {
                        MessageBox.Show(error_text, "Invalid Email", MessageBoxButton.OK);
                        return false;
                    }
                }
            }
            return true;
        }

        private void SignUp_btn_Click(object sender, RoutedEventArgs e)
        {
            clearWindow();
            RegisterWindow register = new RegisterWindow();
            register.Title = "Registration";
            register.ShowDialog();
        }

        private void clearWindow()
        {
            txtEmail.Clear();
            txtPasswordHidden.Clear();
            txtPasswordShown.Clear();
            PasswordView.Source = getPasswordViewImage("eye-close-icon.png");
            txtPasswordHidden.Visibility = Visibility.Visible;
            txtPasswordShown.Visibility = Visibility.Hidden;
            flagChengeView = false;
        }

        private void PasswordView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!flagChengeView)
            {
                PasswordView.Source = getPasswordViewImage("eye-open-icon.png");
                txtPasswordHidden.Visibility = Visibility.Hidden;
                txtPasswordShown.Visibility = Visibility.Visible;
                txtPasswordShown.Text = txtPasswordHidden.Password;
                txtPasswordShown.Focus();
                txtPasswordShown.SelectionStart = txtPasswordShown.Text.Length;
                flagChengeView = !flagChengeView;
                return;
            }
            PasswordView.Source = getPasswordViewImage("eye-close-icon.png");
            txtPasswordHidden.Password = txtPasswordShown.Text;
            txtPasswordHidden.Visibility = Visibility.Visible;
            txtPasswordShown.Visibility = Visibility.Hidden;
            txtPasswordHidden.Focus();
            flagChengeView = !flagChengeView;

        }

        private BitmapSource getPasswordViewImage(string imageName)
        {
            string path = "../../../";
            Stream imageStreamSource = new FileStream(path + imageName, FileMode.Open, FileAccess.Read, FileShare.Read);
            PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapsource = decoder.Frames[0];
            return bitmapsource;
        }

    }
}
