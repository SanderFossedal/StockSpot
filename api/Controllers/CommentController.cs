using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        private readonly IStockRepository _stockRepo;

        private readonly UserManager<AppUser> _userManager;

        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepository;
            _userManager = userManager;
            _fmpService = fmpService;
        }
        
        
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {

            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepo.GetAllAsync(queryObject);
            
            var commentDto = comments.Select(c => c.ToCommentDto());

            return Ok(commentDto); 
        }

        [HttpGet("{id:int}")] //api/Comment/1 :int for validation
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto commentDto)
        {
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var stock = await _stockRepo.GetBySymbolAsync(symbol);

           if(stock == null)
           {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);

                if (stock == null)
                {
                    return BadRequest("Stock not found");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
           }

           var username = User.GetUsername();
           var appUser = await _userManager.FindByNameAsync(username);

           var commentModel = commentDto.ToCommentFromCreate(stock.Id);
           commentModel.AppUserId = appUser.Id;
           await _commentRepo.CreateAsync(commentModel);

           return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());

            if (comment == null)
            {
                return NotFound("Comment not found");
            }


            return Ok(comment.ToCommentDto());
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //Perform validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentModel = await _commentRepo.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(commentModel);
        }
        
    }
}