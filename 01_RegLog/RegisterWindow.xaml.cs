using _01_RegLog.Models;
using Microsoft.Win32;
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
using System.Drawing;
using System.Drawing.Imaging;

namespace _01_RegLog
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        bool flagChengeView = false;
        public User registerUser { get; set; } = new User();
        private MyDataContext data { get; set; } = new MyDataContext();
        private List<string> Extensions { get; set; } = new List<string> { ".bmp", ".jpg", ".png" }; 
        private string fileImage { get; set; } = "";
        private string extension { get; set; } = "";
        private string Surname { get; set; } = "";
        private string _Name { get; set; } = "";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void SignUp_btn_Click(object sender, RoutedEventArgs e)
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
                                    if (checkImage())
                                    {
                                        addUser();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (checkPassword(txtPasswordShown.Text))
                            {
                                if (checkConfirmPassword(txtConfirmPasswordShown.Text, txtPasswordShown.Text))
                                {
                                    if (checkImage())
                                    {
                                        addUser();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void addUser()
        {
            initializeUser();
            data.Users.Add(registerUser);
            data.SaveChanges();
            var res = MessageBox.Show("Registration is successfull", "Register success", MessageBoxButton.OK);
            if (res == MessageBoxResult.OK)
                this.Close();
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
                return true;

            MessageBox.Show("Uncorrect phone number.\r\nExample: (096) 812-93-63", "Invalid number", MessageBoxButton.OK);
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
                return IsEmailOccupied(email);
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect email.\r\nExample: johnCina@gmail.com", "Invalid Email", MessageBoxButton.OK);
                return false;
            }
        }

        private bool IsEmailOccupied(string email)
        {
            if (data.Users.Count() != 0)
            {
                foreach (var user in data.Users)
                {
                    if (user.Email == email)
                    {
                        MessageBox.Show("This Email is already exists", "Email is occupied", MessageBoxButton.OK);
                        return false;
                    }
                }
            }
            return true;
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
                return true;
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

        private bool checkImage()
        {
            if (Avatar.Source.ToString() == "pack://application:,,,/Pictures-icon.png")
            {
                MessageBox.Show("Please, choose your Avatar", "Invalid Photo", MessageBoxButton.OK);
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
                fileImage = openFileDialog.FileName;
                extension = System.IO.Path.GetExtension(openFileDialog.FileName);
            }
            changeImageSource(fileImage, extension, Extensions);
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

        private void initializeUser()
        {
            registerUser.SNP = txtSNP.Text;
            registerUser.Surname = Surname;
            registerUser.Name = _Name;
            registerUser.Phone = txtPhone.Text;
            registerUser.Email = txtEmail.Text;
            if (!flagChengeView)
                registerUser.Password = txtPasswordHidden.Password;
            else
                registerUser.Password = txtPasswordShown.Text;
            saveUserAvatar();
        }

        private void saveUserAvatar()
        {

            string dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Users Images");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileRandName = System.IO.Path.GetRandomFileName() + extension;
            Bitmap bmp = new Bitmap(fileImage);
            bmp.Save("Users Images/" + fileRandName);
            registerUser.Image = fileRandName;
            saveImageSmaller(fileRandName, dir);
        }

        private void saveImageSmaller(string fileRandName, string dir)
        {
            string path = "Users Images/" + "small_" + fileRandName;
            Bitmap bmp = new Bitmap(fileImage);
            //Декілька фоток різних розмірів(все менше і менше)
            //for (int i = 1; i < 6; i++)
            //{
            //    var bmpSave1 = ImageWorker.CompressImage(bmp, i * 50, i * 50);
            //    path2 = $"../../../Users Images/{i * 50}_" + $"{new_name}" + $"_small{extension}";
            //    bmpSave1.Save(path2);
            //}

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
    }
}
