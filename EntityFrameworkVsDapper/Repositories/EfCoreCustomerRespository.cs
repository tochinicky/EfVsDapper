﻿using System;
using EntityFrameworkVsDapper.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkVsDapper.Repositories
{
    public class EfCoreCustomerRepository
    {
        private readonly AppDbContext _context;

        public EfCoreCustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            // return await _context.Customers.FromSql($"SELECT * FROM \"Customers\" WHERE \"CustomerId\" = {id}").FirstOrDefaultAsync()
            return await _context.Customers.FindAsync(id);
        }
        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}

