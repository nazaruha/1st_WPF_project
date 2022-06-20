using _01_RegLog.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        private MyDataContext data { get; set; } = new MyDataContext();
        private string imagesPath { get; set; }
        public UserVM newUser { get; set; }
        private User _newUser { get; set; }
        private int Id { get; set; }
        private bool flagChengeView { get; set; } = false;
        private List<string> Extensions { get; set; } = new List<string> { ".bmp", ".jpg", ".png" };
        private string extension { get; set; } = "";
        private string newImagePath { get; set; } = "";
        private string newImageName { get; set; } = "";
        private string Surname { get; set; } = "";
        private string _Name { get; set; } = "";


        public ChangeUserWindow(int userId, string imagesPath)
        {
            InitializeComponent();
            _newUser = data.Users.Find(userId);
            this.imagesPath = imagesPath;
            //_newUser = new User()
            //{
            //    Id = user.Id,
            //    SNP = user.Surname + ' ' + user.Name,
            //    Name = user.Name,
            //    Surname = user.Surname,
            //    Phone = user.Phone,
            //    Email = user.Email,
            //    Password = user.Password,
            //    Image = user.ImageUrl
            //};

            Id = _newUser.Id;
            txtSNP.Text = _newUser.Surname + ' ' + _newUser.Name;
            txtPhone.Text = _newUser.Phone;
            txtEmail.Text = _newUser.Email;
            txtPasswordHidden.Password = _newUser.Password;

            if (!Uri.IsWellFormedUriString(_newUser.Image, UriKind.Absolute))
            {
                Avatar.Source = new BitmapImage(new Uri(imagesPath + _newUser.Image));
            }
            else
            {
                Avatar.Source = new BitmapImage(new Uri(_newUser.Image));
            }
            txtConfirmPasswordHidden.Password = _newUser.Password;
            newImageName = _newUser.Image;
        }
        private bool checkSNP(string SNP)
        {
            if (String.IsNullOrWhiteSpace(SNP))
            {
                MessageBox.Show("Please, input SNP", "Invalid SNP", MessageBoxButton.OK);
                return false;
            }

            Regex check_snp_rgx = new Regex(@"^\s*[A-ZА-ЯІ][a-zа-яі]*\s*[A-ZА-ЯІ][a-zа-яі]*\s*$");

            if (check_snp_rgx.IsMatch(SNP))
            {
                Regex split_rgx = new Regex(@"\s*[A-ZА-ЯІ][a-zа-яі]*\s*");
                MatchCollection match = split_rgx.Matches(SNP);

                Surname = correct_PartOfSNP(Surname, match[0]);
                _Name = correct_PartOfSNP(_Name, match[1]);

                _newUser.SNP = Surname + ' ' + _Name;
                _newUser.Surname = Surname;
                _newUser.Name = _Name;

                return true;
            }

            MessageBox.Show("Incorrect SNP.\r\nExample: Fedun Nazar.", "Ivalid SNP", MessageBoxButton.OK);
            return false;
        }

        private string correct_PartOfSNP(string part_of_snp, Match item)
        {
            Regex space_rgx = new Regex(@"\s+");
            part_of_snp = item.ToString();
            part_of_snp = space_rgx.Replace(part_of_snp, "");
            return part_of_snp;
        }

        private bool checkPhone(string phone)
        {
            if (String.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please, input phone", "Invalid Phone", MessageBoxButton.OK);
                return false;
            }

            Regex regex = new Regex(@"^\(0\d{2}\)\s\d{3}\-\d{2}\-\d{2}$");
            if (regex.IsMatch(phone))
            {
                _newUser.Phone = phone;
                return true;
            }

            MessageBox.Show("Uncorrect phone number.\r\nExample: (096)-812-93-63", "Invalid number", MessageBoxButton.OK);
            return false;
        }

        private bool checkEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please, input Email", "Invalid Email", MessageBoxButton.OK);
                return false;
            }

            try
            {
                MailAddress mylo = new MailAddress(email);
                _newUser.Email = email;
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect email.\r\nExample: johnCina@gmail.com", "Invalid Email", MessageBoxButton.OK);
                return false;
            }
        }

        private bool checkPassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please, input Password", "Invalid Password", MessageBoxButton.OK);
                return false;
            }

            Regex regex = new Regex(@"^(?=[A-Za-z]+)(?=.+[0-9])(?=.+[!@#\$%\^&\*\(\)_\+=\-]).{8,}$");
            if (regex.IsMatch(password))
            {
                _newUser.Password = password;
                return true;
            }
            MessageBox.Show("Password must be 8 length and has at least:\r\n\r\t1 upper case;\r\n\r\t1 number;\r\n\r\t1 symbol");
            return false;
        }

        private bool checkConfirmPassword(string confirm_password, string real_password)
        {
            if (String.IsNullOrWhiteSpace(confirm_password))
            {
                MessageBox.Show("Please, input password confirmation", "Invalid Confirm Password", MessageBoxButton.OK);
                return false;
            }
            if (confirm_password != real_password)
            {
                MessageBox.Show("Passwords are not the same", "Invalid confirm password", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void addAvatar_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp, *.jpg, *.png)|*.bmp; *.jpg; *.png|All (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                newImagePath = openFileDialog.FileName;
                newImageName = openFileDialog.SafeFileName;
                extension = System.IO.Path.GetExtension(openFileDialog.FileName);
            }
            changeImageSource(newImagePath, extension, Extensions);
        }

        private void changeImageSource(string source, string extension, List<string> Extensions)
        {
            if (source != "")
            {
                if (Extensions.Contains(extension))
                {
                    Avatar.Source = new BitmapImage(new Uri(source));
                }
                else
                {
                    MessageBox.Show("Uncorrect extensions. Please, choose another photo", "Invalid Photo", MessageBoxButton.OK);
                }
            }
        }

        private void saveUserAvatar()
        {
            if (_newUser.Image != newImageName)
            {
                string dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Users Images");
                if (!Uri.IsWellFormedUriString(_newUser.Image, UriKind.Absolute))
                    deleteOldUserAvatar(dir);
                string fileRandName = System.IO.Path.GetRandomFileName() + extension;
                Bitmap bmp = new Bitmap(newImagePath);

                bmp.Save("Users Images/" + fileRandName);
                saveImageSmaller(fileRandName, dir);
                _newUser.Image = fileRandName;
            }
        }

        private void deleteOldUserAvatar(string dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();
            foreach (var file in files)
            {
                if (_newUser.Image.ToString().Contains(file.Name))
                {
                    if (File.Exists(dir + @"\\small_" + file.Name))
                    {
                        File.Delete(dir + @"\\small_" + file.Name);
                    }
                    if (File.Exists(file.FullName))
                    {
                        File.Delete(file.FullName);
                    }

                    
                    return;
                }
            }
        }

        private void saveImageSmaller(string fileRandName, string dir)
        {
            string path = dir + "/" + "small_" + fileRandName;
            Bitmap bmp = new Bitmap(newImagePath);
            var bmpSave = ImageWorker.CompressImage(bmp, 50, 50);
            bmpSave.Save(path);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!flagChengeView)
            {
                PassworView.Source = getPasswordViewImage("eye-open-icon.png");
                txtPasswordShown.Text = txtPasswordHidden.Password;
                txtConfirmPasswordShown.Text = txtConfirmPasswordHidden.Password;
                txtPasswordHidden.Visibility = Visibility.Hidden;
                txtConfirmPasswordHidden.Visibility = Visibility.Hidden;
                txtPasswordShown.Visibility = Visibility.Visible;
                txtConfirmPasswordShown.Visibility = Visibility.Visible;
                flagChengeView = !flagChengeView;
                return;
            }
            PassworView.Source = getPasswordViewImage("eye-close-icon.png");
            txtPasswordHidden.Password = txtPasswordShown.Text;
            txtConfirmPasswordHidden.Password = txtConfirmPasswordShown.Text;
            txtPasswordHidden.Visibility = Visibility.Visible;
            txtConfirmPasswordHidden.Visibility = Visibility.Visible;
            txtPasswordShown.Visibility = Visibility.Hidden;
            txtConfirmPasswordShown.Visibility = Visibility.Hidden;
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

        private void ChangeUser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (checkSNP(txtSNP.Text))
            {
                if (checkPhone(txtPhone.Text))
                {
                    if (checkEmail(txtEmail.Text))
                    {
                        if (!flagChengeView)
                        {
                            if (checkPassword(txtPasswordHidden.Password))
                            {
                                if (checkConfirmPassword(txtConfirmPasswordHidden.Password, txtPasswordHidden.Password))
                                {
                                    changeUser();
                                }
                            }
                        }
                        else
                        {
                            if (checkPassword(txtPasswordShown.Text))
                            {
                                if (checkConfirmPassword(txtConfirmPasswordShown.Text, txtPasswordShown.Text))
                                {
                                    changeUser();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void changeUser()
        {
            saveUserAvatar();
            //data.Remove(oldUser);
            data.Update(_newUser);
            data.SaveChanges();
            if (!Uri.IsWellFormedUriString(_newUser.Image, UriKind.Absolute))
            {
                newUser = new UserVM() { Id = _newUser.Id, Surname = _newUser.Surname, Name = _newUser.Name, SNP = Surname + ' ' + Name, Email = _newUser.Email, Phone = _newUser.Phone, Password = _newUser.Password, ImageUrl = imagesPath + _newUser.Image };
            }
            else
            {
                newUser = new UserVM() { Id = _newUser.Id, Surname = _newUser.Surname, Name = _newUser.Name, SNP = Surname + ' ' + Name, Email = _newUser.Email, Phone = _newUser.Phone, Password = _newUser.Password, ImageUrl = _newUser.Image };
            }
            MessageBox.Show("Changins are saved", "Data changing", MessageBoxButton.OK);
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
