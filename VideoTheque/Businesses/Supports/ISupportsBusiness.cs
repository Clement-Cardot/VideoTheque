using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Supports
{
    public interface ISupportsBusiness
    {
        Task<List<SupportDto>> GetSupports();

        SupportDto GetSupport(int id);

        SupportDto InsertSupport(SupportDto Support);

        void UpdateSupport(int id, SupportDto Support);

        void DeleteSupport(int id);
    }
}
