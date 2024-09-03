using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers
{

    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;

        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepo = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            //User is from ControllerBase
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            if (appUser == null)
            {
                return NotFound();
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string Symbol){

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(Symbol);

            if(stock == null) return BadRequest("Stock not found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if(userPortfolio.Any(s => s.Symbol.ToLower() == Symbol)) return BadRequest("Stock already in portfolio");

            var portfolio = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepo.CreateAsync(portfolio);

            if(portfolio == null){
                return StatusCode(500,"Failed to create portfolio");
            } 
            else{
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        //Delete a stock from the user's portfolio (not the whole portfolio)
        public async Task<IActionResult> DeletePortfolio(string Symbol){

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == Symbol.ToLower()).ToList();

            if(filteredStock.Count == 1){
                await _portfolioRepo.DeletePortfolio(appUser, Symbol);
            }
            else{
                return BadRequest("Stock not found in portfolio");
            }

            return Ok();
        }

    }
}