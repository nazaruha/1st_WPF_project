using _01_RegLog.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace _01_RegLog
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {        
        public UserWindow()
        {
            InitializeComponent();
        }

        public UserWindow(User user_data)
        {
            InitializeComponent();
            if (Uri.IsWellFormedUriString(user_data.Image, UriKind.Absolute))
            {
                Avatar.Source = new BitmapImage(new Uri(user_data.Image));
            }
            else
            {
                Avatar.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Users Images/" + user_data.Image));
            }
            //if (!user_data.Image.Contains("http"))
            //{
            //    Avatar.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/Users Images/" + user_data.Image));
            //}
            //else
            //{
            //    Avatar.Source = new BitmapImage(new Uri(user_data.Image));
            //}
            txtLabel2.Content += $"{user_data.Name}!";
        }

        private void usersList_btn_Click(object sender, RoutedEventArgs e)
        {
            UsersListWindow usersList = new UsersListWindow();
            usersList.Show();
        }
    }
}
