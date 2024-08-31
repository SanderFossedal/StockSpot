using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController :ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _stockRepo = stockRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){

            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var stocks = await _stockRepo.GetAllAsync(query); //Gets all stocks from the database
            var StockDto = stocks.Select(s => s.ToStockDto()); //Selects all stocks from the database and maps them to the StockDto
            return Ok(StockDto); //Returns a 200 status code with the stocks
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetAsync(id); //Gets the stock from the database

            if(stock == null){
                return NotFound(); //Returns a 404 status code (IActionResult)
            }

            return Ok(stock.ToStockDto()); //Returns a 200 status code with the stock
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDTO(); //Maps the CreateStockRequestDto to the Stock model
           
            await _stockRepo.CreateAsync(stockModel); //Creates the stock in the database

            //CreatedatAction is a helper method that returns a 201 status code with the location of the created resource
            //It uses the GetById method to get the location of the created resource
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto()); 
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockRequestDto){

            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepo.UpdateAsync(id, stockRequestDto); //Updates the stock in the database

            if(stockModel == null ){
                return NotFound(); //Returns a 404 status code
            }

            return Ok(stockModel.ToStockDto()); //Returns a 200 status code with the updated stock
        }

        [HttpDelete]
        [Route("{id:int}")] //
        public async Task<IActionResult> Delete([FromRoute] int id){

            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var stockModel = await _stockRepo.DeleteAsync(id); //Deletes the stock from the database

            if(stockModel == null){
                return NotFound(); //Returns a 404 status code
            }

            

            return NoContent(); //Returns a 204 status code
        }
    }
}