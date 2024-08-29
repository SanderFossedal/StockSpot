using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock?> GetAsync(int id); //? means that the return type can be null

        Task<Stock> CreateAsync(Stock stock);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);

        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExists(int id);
    }
}