using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
