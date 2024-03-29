﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Supports
{
    public interface ISupportsRepository
    {
        Task<List<SupportDto>> GetSupports();

        ValueTask<SupportDto?> GetSupport(int id);

        ValueTask<SupportDto?> GetSupportByName(string name);

        Task InsertSupport(SupportDto support);

        Task UpdateSupport(int id, SupportDto support);

        Task DeleteSupport(int id);
    }
}
