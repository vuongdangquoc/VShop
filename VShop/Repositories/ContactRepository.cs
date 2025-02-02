﻿using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private readonly VShopContext _context;

        public ContactRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
