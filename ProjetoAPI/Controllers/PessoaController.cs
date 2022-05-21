using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoAPI.Data;
using ProjetoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoAPI.Controllers
{
    [ApiController]
    [Route("pessoa")]
    public class PessoaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Pessoa>>> Get([FromServices] DataContext context)
        {
            var pessoas = await context.Pessoas
                .AsNoTracking()
                .ToListAsync();
            return pessoas;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Pessoa>> Post([FromServices] DataContext context, [FromBody] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                context.Pessoas.Add(pessoa);
                await context.SaveChangesAsync();
                return pessoa;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Pessoa>> GetById([FromServices] DataContext context, int id)
        {
            var pessoa = await context.Pessoas
                .AsNoTracking() //Não rastrear versões do objeto na busca (melhor performance)
                .FirstOrDefaultAsync(x => x.Id == id);
            return pessoa;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Pessoa>> Put([FromServices] DataContext context, [FromBody] Pessoa pessoa, int id)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                context.Entry(pessoa).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return pessoa;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Pessoa>> Delete([FromServices] DataContext context, int id)
        {
            var pessoa = await context.Pessoas
            .FirstOrDefaultAsync(e => e.Id == id);
            if (pessoa != null)
            {
                context.Pessoas.Remove(pessoa);
                await context.SaveChangesAsync();
                return pessoa;
            }
            return null;
        }
    }
}
