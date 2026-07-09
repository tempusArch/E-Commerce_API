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
public class ProductController : ControllerBase {
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) { 
        _mediator = mediator;
    }

    [HttpGet("{productId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReadProductDto>> GetSingleProduct(int productId) {
        return Ok(await _mediator.Send(new GetOneProductQuery(productId)));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ProductListResponse>> GetManyProducts(
        [FromQuery] string? categoryName,
        [FromQuery] string? productName,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10
    ) {
        
        return Ok(await _mediator.Send(new GetManyProductsQuery(categoryName, productName, page, limit)));
    }
}