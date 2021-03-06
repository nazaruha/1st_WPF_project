using _01_RegLog.Models;
using Bogus;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UsersListWindow.xaml
    /// </summary>
    public partial class UsersListWindow : Window
    {
        public string imagesPath { get; set; } = System.IO.Directory.GetCurrentDirectory() + "/Users Images/";
        private MyDataContext data = new MyDataContext();
        private ObservableCollection<UserVM> users = new ObservableCollection<UserVM>(); // пов'язане з базою нашою. Змінює її
        int page;
        int pages;
        int pageSize = 10;
        int usersCount;

        public UsersListWindow()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        private void InitializeDataGrid(int currentPage = 1)
        {
            var query = data.Users.AsQueryable();
            page = currentPage;
            int skip = (page - 1) * pageSize;
            usersCount = query.Count();
            pages = (int)Math.Ceiling((double)usersCount / (double)pageSize);
            query = query.Skip(skip).Take(10);

            var con = query.Select(x => new UserVM
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Password = x.Password,
                ImageUrl = x.Image
            }).ToList();
            users = new ObservableCollection<UserVM>(con);
            for (int i = 0; i < users.Count(); i++)
            {
                if (!Uri.IsWellFormedUriString(users[i].ImageUrl, UriKind.Absolute))
                {
                    users[i].ImageUrl = imagesPath + users[i].ImageUrl;

                }
            }
            dgUsers.ItemsSource = users;
            pageNumber_label.Content = $"{page} of {pages}";
            

        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void addUser_btn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registration = new RegisterWindow();
            registration.Title = "Registration new user";
            registration.ShowDialog();
            InitializeDataGrid(page);
        }

        private void deleteUser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem == null) return;
            Models.User userToRemove = data.Users.Find((dgUsers.SelectedItem as UserVM).Id);
            data.Users.Remove(userToRemove);
            data.SaveChanges();
            users.Remove(dgUsers.SelectedItem as UserVM);
            dgUsers.ItemsSource = users;
        }

        private void allUsers_btn_Click(object sender, RoutedEventArgs e)
        {
            if (users.Count() < data.Users.Count())
            {
                InitializeDataGrid();
            }
        }

        private void changeUser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                if (dgUsers.SelectedItem is UserVM)
                {
                    var userView = (dgUsers.SelectedItem as UserVM);
                    ChangeUserWindow changeUserWindow = new ChangeUserWindow(userView.Id, imagesPath);
                    changeUserWindow.Show(); 
                    if (changeUserWindow.newUser != null)
                    {
                        int index = users.IndexOf((dgUsers.SelectedItem as UserVM));
                        users[index] = changeUserWindow.newUser;
                        
                    }
                }
            }
        }

        [Obsolete]
        private void randomUser_btn_Click(object sender, RoutedEventArgs e)
        {
            //uk - ua
            var faker = new Faker<Models.User>("uk")
                .RuleFor(u => u.Surname, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Name, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Image, f => f.Image.Cats());


            //var user = faker.Generate(); // Generates random user
            
            for (int i = 0; i < 10; i++)
            {
                var user = faker.Generate();
                user.SNP = user.Surname + ' ' + user.Name;
                UserVM _user = new UserVM()
                {
                    Id = user.Id,
                    Surname = user.Surname,
                    Name = user.Name,
                    SNP = user.SNP,
                    Phone = user.Phone,
                    Email = user.Email,
                    Password = user.Password,
                    ImageUrl = user.Image
                };
                users.Add(_user);
                data.Users.Add(user);
                data.SaveChanges();
            }
        }

        private void SearchUser_btn_Click(object sender, RoutedEventArgs e)
        {
            SearchUsersWindow searchUserWindow = new SearchUsersWindow();
            searchUserWindow.ShowDialog();
            if (searchUserWindow.isTxtEmpty == true)
            {
                if (users.Count() < data.Users.Count())
                {
                    InitializeDataGrid();
                }
                return;
            }
            users = new ObservableCollection<UserVM>(searchUserWindow.foundUsersVM);
            dgUsers.ItemsSource = users;
        }

        private void refreshDG_btn_Click(object sender, RoutedEventArgs e)
        {
            InitializeDataGrid(page);
        }

        private void prevPage_btn_Click(object sender, RoutedEventArgs e)
        {
            if (page > 1)
            {
                page--;
                InitializeDataGrid(page);
            }
        }

        private void nextPage_btn_Click(object sender, RoutedEventArgs e)
        {
            if (page < pages)
            {
                page++;
                InitializeDataGrid(page);
            }
        }
    }
}