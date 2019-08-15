using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UserContext db = new UserContext())
            {
                // создаем два объекта User
                User user1 = new User { userName = "Fobbi", passwordUser ="123test",
                firstName = "Fobbi", phoneNum = "+380635621456", groupName="RPZSTO-162"};

                // добавляем их в бд
                db.Users.Add(user1);
                db.SaveChanges();
                Console.WriteLine("Объекты успешно сохранены");

                // получаем объекты из бд и выводим на консоль
                var users = db.Users;
                Console.WriteLine("Список объектов:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.userId}\n{u.userName}\n{u.passwordUser}\n{u.firstName}\n{u.phoneNum}\n{u.groupName}");
                }
            }

            using (UserContext db = new UserContext())
            {
                var phones = db.Users.Where(p => p.userName == "Fobbi");
                foreach (User p in phones)
                    Console.WriteLine("{0}.{1} - {2}", p.userId, p.userName, p.passwordUser);
            }
            Console.Read();
        }
    }
}
