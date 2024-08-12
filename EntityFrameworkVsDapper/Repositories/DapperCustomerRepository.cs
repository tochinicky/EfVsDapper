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
            //_connection = connection;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Customer>("SELECT \"CustomerId\",\"Name\" FROM \"Customers\"");
            }
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(
                    "SELECT * FROM \"Customers\" WHERE \"CustomerId\" = @Id", new { Id = id });
            }
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO \"Customers\" (Name) VALUES (@Name)", customer);
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "UPDATE \"Customers\" SET \"Name\" = @Name WHERE \"CustomerId\" = @CustomerId", customer);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "DELETE FROM \"Customers\" WHERE \"CustomerId\" = @Id", new { Id = id });
            }
        }

    }
}


