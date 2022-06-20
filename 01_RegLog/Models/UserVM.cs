using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _01_RegLog.Models
{
    public class UserVM : INotifyPropertyChanged // будуть біндитись данні
    {
        public int Id { get; set; }


        private string surname;
        public string Surname
        {
            get { return surname; }
            set
            {
                if (this.surname != value)
                {
                    surname = value;
                    NotifyPropertyChanged("Surname");
                }
            }
        }

        //propfull
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (this.name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name"); // та назва яка буде біндитись
                }
            }
        }

        private string snp;

        public string SNP
        {
            get { return snp; }
            set { snp = Surname + ' ' + Name; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                if (this.email != value)
                {
                    email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }


        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                if (this.phone != value)
                {
                    phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }

        private string image_url;
        public string ImageUrl
        {
            get { return image_url; }
            set 
            { 
                if (this.image_url != value)
                {
                    image_url = value;
                    NotifyPropertyChanged("ImageUrl"); // та назва яка буде біндитись
                }
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        


    }
}
