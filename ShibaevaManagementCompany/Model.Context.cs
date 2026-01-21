namespace ShibaevaManagementCompany
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ShibaevaManagementCompanyEntities : DbContext
    {
        public ShibaevaManagementCompanyEntities()
            : base("name=ShibaevaManagementCompanyEntities")
        {
            // Инициализация конфигурации
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        private static ShibaevaManagementCompanyEntities _context;

        public static ShibaevaManagementCompanyEntities GetContext()
        {
            if (_context == null)
                _context = new ShibaevaManagementCompanyEntities();
            return _context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Для Database First оставляем исключение
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Apartments> Apartments { get; set; }
        public virtual DbSet<Buildings> Buildings { get; set; }
        public virtual DbSet<Debts> Debts { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<ServiceRequests> ServiceRequests { get; set; }

        // Удалите sysdiagrams если у вас его нет в базе
        // public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}