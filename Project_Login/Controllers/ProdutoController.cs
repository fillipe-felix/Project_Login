using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Project_Login.Extensions;
using Project_Login.Models;
using Project_Login.Persistence;

namespace Project_Login.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProdutoController : MainController
{
    private readonly MeuDbContext _context;

    public ProdutoController(MeuDbContext context)
    {
        _context = context;
    }

    [ClaimsAuthorize("Produto", "Cadastrar")]
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    
    [ClaimsAuthorize("Produto", "Listar")]
    [HttpGet()]
    public async Task<IActionResult> Listar()
    {
        var listProduto = await _context.Produtos.ToListAsync();
        
        return Ok(listProduto);
    }
}
