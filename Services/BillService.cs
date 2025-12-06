using Demo3DAPI.Data;
using Demo3DAPI.Interfaces;
using Demo3DAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo3DAPI.Services
{
    public class BillService : IBillService
    {
        private readonly ApplicationDbContext _context;

        public BillService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bill>> GetAllBills()
        {
            return await _context.Bills
                .Include(b => b.PlayerAccount)
                .ToListAsync();
        }

        public async Task<Bill?> GetBillById(int id)
        {
            return await _context.Bills
                .Include(b => b.PlayerAccount)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Bill> CreateBill(Bill bill)
        {
            bill.CreateDate = DateTime.Now; 
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
            return bill;
        }

        public async Task<bool> UpdateBill(int id, Bill bill)
        {
            var existingBill = await _context.Bills.FindAsync(id);
            if (existingBill == null) return false;

            existingBill.PaymentDate = bill.PaymentDate;
            existingBill.Status = bill.Status;
           

            _context.Entry(existingBill).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null) return false;

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}