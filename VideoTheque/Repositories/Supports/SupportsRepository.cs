using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data.Common;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Supports
{
    public class SupportsRepository : ISupportsRepository
    {
        // private readonly VideothequeDb _db;
       
        private enum _db
        {
            STREAMING = 0b_0000_0000,  // 0
            BLURAY = 0b_0000_0001,  // 1
            DVD = 0b_0000_0010,  // 2
            // SLOT_4 = 0b_0000_0100,  // 4
            // SLOT_5 = 0b_0000_1000,  // 8
            // SLOT_6 = 0b_0001_0000,  // 16
            // SLOT_7 = 0b_0010_0000,  // 32
            // SLOT_8 = 0b_0100_0000,  // 64
        }

        public SupportsRepository(VideothequeDb db)
        {
            // Database Not Ready => Mocked
            // _db = db;
        }

        public Task<List<SupportDto>> GetSupports()
        {
            return Task.Run(() =>
            {
                List<SupportDto> supportDtos = new List<SupportDto>();
                foreach (int id in Enum.GetValues(typeof(_db)))
                {
                    supportDtos.Add
                    (
                        new SupportDto
                        {
                            Id = id,
                            Name = Enum.GetName(typeof(_db), id)
                        }
                    );
                }
                return supportDtos;
            }
            );
        }

        public ValueTask<SupportDto?> GetSupport(int id)
        {
            return new ValueTask<SupportDto?>(
                new SupportDto {
                    Id = id,
                    Name = Enum.GetName(typeof(_db), id)
                }
            );
        }

        public ValueTask<SupportDto?> GetSupportByName(string name)
        {
            string[] supports = Enum.GetNames(typeof(_db));
            int supportsIndex = -1;
            for (int i = 0; i < supports.Length; i++)
            {
                if (name == supports[i])
                {
                    supportsIndex = i;
                    break;
                }
            }
            if (supportsIndex == -1)
            {
                return new ValueTask<SupportDto?>();
            }
            return new ValueTask<SupportDto?>(
                new SupportDto
                {
                    Id = supportsIndex,
                    Name = name
                }
            );
        }

        public Task DeleteSupport(int id)
        {
            throw new NotImplementedException();
        }        

        public Task InsertSupport(SupportDto support)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSupport(int id, SupportDto support)
        {
            throw new NotImplementedException();
        }
    }
}
