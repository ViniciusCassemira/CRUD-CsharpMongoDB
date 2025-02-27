using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace UserControl
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("adminUser");
            _usersCollection = database.GetCollection<User>("Users");
        }

        public async Task AddUserAsync()
        {
            Console.WriteLine("Enter user name: ");
            string userName = Console.ReadLine();

            var newUser = new User { Name = userName };

            await _usersCollection.InsertOneAsync(newUser);
            Console.WriteLine("User added successfully.");
        }

        public async Task ShowUsersAsync()
        {
            Console.Clear();
            var users = await _usersCollection.Find(_ => true).ToListAsync();

            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("User List:");
            foreach (var user in users)
            {
                if (user.Id == ObjectId.Empty)
                {
                    Console.WriteLine("Invalid _id for user.");
                }
                else
                {
                    Console.WriteLine($"ID: {user.Id} | Name: {user.Name}");
                }
            }
        }


        public async Task DeleteUserAsync()
        {
            Console.WriteLine("Enter the ID of the user to delete: ");
            string userId = Console.ReadLine();

            if (!ObjectId.TryParse(userId, out ObjectId objectId))
            {
                Console.Clear();
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var result = await _usersCollection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("User deleted successfully.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("User not found.");
            }
        }

    public async Task EditUserAsync()
        {
            Console.WriteLine("Enter the ID of the user to edit:");
            string userId = Console.ReadLine();

            if (!ObjectId.TryParse(userId, out ObjectId objectId))
            {
                Console.Clear();
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var existingUser = await _usersCollection.Find(u => u.Id == objectId).FirstOrDefaultAsync();
            Console.Clear();

            if (existingUser == null)
            {
                Console.WriteLine("User not found.");
                return;
            }
            Console.WriteLine($"Enter the new name for the user '{existingUser.Name}':");
            string newName = Console.ReadLine();
            
            Console.Clear();

            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Invalid name. Operation cancelled.");
                return;
            }

            var update = Builders<User>.Update.Set(u => u.Name, newName);
            var result = await _usersCollection.UpdateOneAsync(u => u.Id == objectId, update);

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine("User updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update user.");
            }
        }

    }

    public class User
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
