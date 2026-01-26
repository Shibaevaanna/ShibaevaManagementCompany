using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Contexts;

namespace ShibaevaManagementCompany
{
    public class ShibaevaManagementCompanyEntities : DbContext
    {
        public ShibaevaManagementCompanyEntities()
            : base("name=ShibaevaManagementCompanyEntities")
        {
        }

        private static ShibaevaManagementCompanyEntities _context;

        public static ShibaevaManagementCompanyEntities GetContext()
        {
            if (_context == null)
                _context = new ShibaevaManagementCompanyEntities();
            return _context;
        }

        public DbSet<Apartments> Apartments { get; set; }
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<Debts> Debts { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<ServiceRequests> ServiceRequests { get; set; }
    }
}