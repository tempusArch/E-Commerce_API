using ECommerceAPI.Domain;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerceAPI.Controller;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase {
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator) { 
        _mediator = mediator;
    }

    [HttpGet("{categoryId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReadCategoryDto>> GetOneCategory(int categoryId) {
        return Ok(await _mediator.Send(new GetOneCategory.GetOneCategoryQuery(categoryId)));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<CategoryListResponse>> GetManyCategories(
        [FromQuery] string? categoryName,
        [FromQuery] string? productName,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10
    ) {
        
        return Ok(await _mediator.Send(new GetManyCategories.GetManyCategoriesQuery(categoryName, productName, page, limit)));
    }
}