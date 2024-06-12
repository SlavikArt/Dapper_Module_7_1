using MailingList.Entities;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MailingList
{
    public class Program
    {
        private static string connectionString = @"Server=LAPTOP-MCLUN3BN\SQLSERVER;Database=MailingListDb;Trusted_Connection=True;Encrypt=False;";

        static async Task Main(string[] args)
        {
            await InsertDataAsync();
            await DisplayDataAsync();

            await UpdateDataAsync();
            await DisplayDataAsync();

            await DeleteDataAsync();
        }

        private static async Task InsertDataAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Insert Country
                    var country = new Country { Name = "Italy" };
                    await connection.ExecuteAsync(
                        "INSERT INTO Countries (Name) VALUES (@Name)", country);

                    // Insert City
                    var city = new City { Name = "Rome", CountryId = 1 };
                    await connection.ExecuteAsync(
                        "INSERT INTO Cities (Name, CountryId) VALUES (@Name, @CountryId)", city);

                    // Insert Section
                    var section = new Section { Name = "Electronics" };
                    await connection.ExecuteAsync(
                        "INSERT INTO Sections (Name) VALUES (@Name)", section);

                    // Insert Promo Product
                    var product = new Product { Name = "Laptop", IsPromotional = true, SectionId = 1 };
                    await connection.ExecuteAsync(
                        "INSERT INTO Products (Name, IsPromotional, SectionId) VALUES (@Name, @IsPromotional, @SectionId)", product);

                    // Insert Customer
                    var customer = new Customer { Name = "Emily", Surname = "Clark", Email = "emilyclark@gmail.com", SectionId = 1 };
                    await connection.ExecuteAsync(
                        "INSERT INTO Customers (Name, Email, SectionId) VALUES (@Name, @Email, @SectionId)", customer);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Помилка при вставці даних: {ex.Message}");
                }
            }
        }

        private static async Task UpdateDataAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Update Country
                    var country = new Country { Id = 1, Name = "France" };
                    await connection.ExecuteAsync(
                        "UPDATE Countries SET Name = @Name WHERE Id = @Id", country);

                    // Update City
                    var city = new City { Id = 1, Name = "Paris", CountryId = 1 };
                    await connection.ExecuteAsync(
                        "UPDATE Cities SET Name = @Name, CountryId = @CountryId WHERE Id = @Id", city);

                    // Update Section
                    var section = new Section { Id = 1, Name = "Home Appliances" };
                    await connection.ExecuteAsync(
                        "UPDATE Sections SET Name = @Name WHERE Id = @Id", section);

                    // Update Promo Product
                    var product = new Product { Id = 1, Name = "Refrigerator", IsPromotional = true, SectionId = 1 };
                    await connection.ExecuteAsync(
                        "UPDATE Products SET Name = @Name, IsPromotional = @IsPromotional, SectionId = @SectionId WHERE Id = @Id", product);

                    // Update Customer
                    var customer = new Customer { Id = 1, Name = "Jake", Surname = "Will", Email = "jakewill@gmail.com", SectionId = 1 };
                    await connection.ExecuteAsync(
                        "UPDATE Customers SET Name = @Name, Email = @Email, SectionId = @SectionId WHERE Id = @Id", customer);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Помилка при оновленні даних: {ex.Message}");
                }
            }
        }

        private static async Task DeleteDataAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Delete City
                    var countryId = 1;
                    await connection.ExecuteAsync(
                        "DELETE FROM Cities WHERE CountryId = @Id", new { Id = countryId });

                    // Delete Country
                    await connection.ExecuteAsync(
                        "DELETE FROM Countries WHERE Id = @Id", new { Id = countryId });


                    // Delete Promo Product
                    var productId = 1;
                    await connection.ExecuteAsync(
                        "DELETE FROM Products WHERE Id = @Id", new { Id = productId });

                    // Delete Customer
                    var customerId = 1;
                    await connection.ExecuteAsync(
                        "DELETE FROM Customers WHERE Id = @Id", new { Id = customerId });

                    // Delete Section
                    var sectionId = 1;
                    await connection.ExecuteAsync(
                        "DELETE FROM Sections WHERE Id = @Id", new { Id = sectionId });
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Помилка при видаленні даних: {ex.Message}");
                }
            }
        }

        private static async Task DisplayDataAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Display Cities
                    var countryId = 1;
                    var cities = await connection.QueryAsync<City>(
                        "SELECT * FROM Cities WHERE CountryId = @CountryId",
                        new { CountryId = countryId });

                    Console.WriteLine("Cities:");
                    foreach (var city in cities)
                        Console.WriteLine($"Id: {city.Id}, " +
                            $"Name: {city.Name}, CountryId: {city.CountryId}");

                    // Display Sections
                    var customerId = 1;
                    var sections = await connection.QueryAsync<Section>(
                        "SELECT s.* FROM Sections s JOIN Customers c ON s.Id = c.SectionId WHERE c.Id = @CustomerId",
                        new { CustomerId = customerId });

                    Console.WriteLine("\nSections:");
                    foreach (var section in sections)
                        Console.WriteLine($"Id: {section.Id}, Name: {section.Name}");

                    // Display Promo Products of a Section
                    var sectionId = 1;
                    var products = await connection.QueryAsync<Product>(
                        "SELECT * FROM Products WHERE SectionId = @SectionId AND IsPromotional = 1",
                        new { SectionId = sectionId });

                    Console.WriteLine("\nPromotional Products:");
                    foreach (var product in products)
                        Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, " +
                            $"IsPromotional: {product.IsPromotional}, SectionId: {product.SectionId}\n");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Помилка при відображенні даних: {ex.Message}");
                }
            }
        }
    }
}
