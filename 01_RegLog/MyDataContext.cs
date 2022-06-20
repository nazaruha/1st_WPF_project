using _01_RegLog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_RegLog
{
    //клас для підключення до БД
    public class MyDataContext : DbContext
    {
        public DbSet<User> Users { get; set; } // буде створюватись список Юзерів
        public MyDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) // визивається тоді, коли ми звертаємось до даних (створення, видалення, перегляд)
        {
            options.UseSqlite($"Data Source=myData.sqlite;"); // Назва База даних це файл myData.sqlite
        }
    }
}
