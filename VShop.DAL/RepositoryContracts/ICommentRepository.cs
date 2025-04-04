﻿using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Task<List<Comment>> GetCommentsByProductId(int id);
 
    }
}
