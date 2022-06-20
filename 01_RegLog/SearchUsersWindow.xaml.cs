using _01_RegLog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for SearchUsersWindow.xaml
    /// </summary>
    public partial class SearchUsersWindow : Window
    {
        public string imagesPath { get; set; } = Directory.GetCurrentDirectory() + "/Users Images/";
        public bool isTxtEmpty = true;
        public List<UserVM> foundUsersVM { get; set; }

        public SearchUsersWindow()
        {
            InitializeComponent();
        }

        private bool checkSNP()
        {
            Regex check_snp_rgx = new Regex(@"^\s*[A-ZА-ЯІ][a-zа-яі]*\s*[A-ZА-ЯІ][a-zа-яі]*\s*$");

            if (check_snp_rgx.IsMatch(txtSNP.Text))
            {
                txtSNP.BorderBrush = Brushes.Green;
                return true;
            }

            txtSNP.BorderBrush = Brushes.Red;
            return false;
        }

        private bool checkEmail()
        {
            try
            {
                MailAddress mylo = new MailAddress(txtEmail.Text);
                txtEmail.BorderBrush = Brushes.Green;
                return true;
            }
            catch (FormatException)
            {
                txtEmail.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool checkPhone()
        {
            Regex regex = new Regex(@"^\(0\d{2}\)\s\d{3}\-\d{2}\-\d{2}$");
            if (regex.IsMatch(txtPhone.Text))
            {
                txtPhone.BorderBrush = Brushes.Green;
                return true;
            }

            txtPhone.BorderBrush = Brushes.Red;
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSNP.Text) || String.IsNullOrWhiteSpace(txtEmail.Text) || String.IsNullOrWhiteSpace(txtPhone.Text))
            {
                isTxtEmpty = true;
                this.Close();
                return;
            }

            if (txtSNP.BorderBrush == Brushes.Green && txtEmail.BorderBrush == Brushes.Green && txtPhone.BorderBrush == Brushes.Green)
            {
                MyDataContext data = new MyDataContext();
                findUsers(data);
            }
            
        }

        private void findUsers(MyDataContext data)
        {
            isTxtEmpty = false;
            var foundUsers = data.Users.Where(x => x.SNP == txtSNP.Text && x.Email == txtEmail.Text && x.Phone == txtPhone.Text);
            if (foundUsers.Count() == 0)
            {
                isTxtEmpty = true;
                this.Close();
                return;
            }
            foundUsersVM = foundUsers.Select(x => new UserVM
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Password = x.Password,
                ImageUrl = imagesPath + x.Image
            }).ToList();
            this.Close();
        }

        private void txtSNP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSNP.Text))
            {
                txtSNP.BorderBrush = Brushes.Gray;
                return;
            }
            checkSNP();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.BorderBrush = Brushes.Gray;
                return;
            }
            checkEmail();
        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPhone.Text))
            {
                txtPhone.BorderBrush = Brushes.Gray;
                return;
            }
            checkPhone();
        }
    }
}
