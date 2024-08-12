using System;
using System.Data;
using Dapper;
using EntityFrameworkVsDapper.Data;
using Npgsql;

namespace EntityFrameworkVsDapper.Repositories
{
    public class DapperCustomerRepository
    {
        private readonly string _connectionString;

        public DapperCustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = """
           SELECT CustomerId, Name FROM public.Customers
         """;
                    var hh = await connection.QueryAsync<Customer>(sql);
                    return hh;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomersAsync: {ex.Message}");
                throw;
            }
           
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    var gg = await connection.QueryFirstOrDefaultAsync<Customer>(
                        """ SELECT * FROM Customers WHERE CustomerId = @Id """, new {id=id});
                    return gg;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomerByIdAsync: {ex.Message}");
                throw;
            }
           
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    """ INSERT INTO Customers (Name) VALUES (@Name) """, new {customer});
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "UPDATE Customers SET Name = @Name WHERE CustomerId = @CustomerId", customer);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "DELETE FROM Customers WHERE CustomerId = @Id", new { Id = id });
            }
        }

    }
}


