using Demo3DAPI.Models;

namespace Demo3DAPI.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<Bill>> GetAllBills();
        Task<Bill?> GetBillById(int id);
        Task<Bill> CreateBill(Bill bill);
        Task<bool> UpdateBill(int id, Bill bill);
        Task<bool> DeleteBill(int id);
    }
}