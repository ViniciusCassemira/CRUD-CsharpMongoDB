using MongoDB.Driver;
using System;

namespace UserControl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userService = new UserService();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("----- User administrator system -----");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Show Users");
                Console.WriteLine("3. Delete User");
                Console.WriteLine("4. Edit User");
                Console.WriteLine("5. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await userService.AddUserAsync();
                    break;
                    case "2":
                        await userService.ShowUsersAsync();
                    break;
                    case "3":
                        await userService.ShowUsersAsync();
                        await userService.DeleteUserAsync();
                    break;
                    case "4":
                        await userService.ShowUsersAsync();
                        await userService.EditUserAsync();
                    break;
                    case "5":
                        exit = true;
                    break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice! Try again.");
                    break;
                }

                if (!exit){
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
