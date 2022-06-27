using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _01_RegLog.Models
{
    public class User
    {
        //Ідентифікатор
        public int Id { get; set; }
        // ПІБ
        public string SNP { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //Ім'я файлу, В налаштуваннях шлях до файлу
        public string Image { get; set; }

        public ImageSource Image_View
        {
            get
            {
                string fileName = Directory.GetCurrentDirectory() + "/Users Images/" + Image;
                using (FileStream file = new FileStream(Image, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    var imageSource = new BitmapImage();
                    using (var bmpStream = new MemoryStream(bytes, 0, (int)file.Length))
                    {
                        imageSource.BeginInit();
                        imageSource.StreamSource = bmpStream;
                        imageSource.CacheOption = BitmapCacheOption.OnLoad;
                        imageSource.EndInit();
                    }

                    imageSource.Freeze(); // here

                    return imageSource;
                }
            }
        }
        //Зберігаємо у шифрованому виді
        public string Password { get; set; }

        public User()
        {

        }

        public User(User other)
        {
            initializeUser(other);
        }

        private void initializeUser(User other)
        {
            Id = other.Id;
            SNP = other.SNP;
            Surname = other.Surname;
            Name = other.Name;
            Email = other.Email;
            Phone = other.Phone;
            Password = other.Password;
            Image = other.Image;
        }
    }
}
