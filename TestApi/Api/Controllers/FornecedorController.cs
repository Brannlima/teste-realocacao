using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Data;
using System.Text.Json;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly ApiContext _context;

        public FornecedoresController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFornecedores()
        {
            var pessoasFisicas = await _context.Fornecedores.OfType<PessoaFisica>().ToListAsync();
            var empresariais = await _context.Fornecedores.OfType<Empresarial>().ToListAsync();

            var fornecedores = pessoasFisicas
                .Union<Fornecedor>(empresariais)
                .Select(
                    fornecedor =>
                        new
                        {
                            FornecedorId = fornecedor.FornecedorId,
                            Nome = fornecedor.Nome,
                            Email = fornecedor.Email,
                            CEP = fornecedor.CEP,
                            Tipo = fornecedor.GetType().Name,
                            CPF = fornecedor is PessoaFisica pfCpf ? pfCpf.CPF : null,
                            RG = fornecedor is PessoaFisica pfRg ? pfRg.RG : null,
                            CNPJ = fornecedor is Empresarial emp ? emp.CNPJ : null,
                            DataNascimento = fornecedor is PessoaFisica pfNasc
                                ? pfNasc.DataNascimento
                                : DateTime.MinValue
                        }
                );

            return Ok(fornecedores);
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            var tipo = fornecedor.GetType();

            object resultado = null;

            if (tipo == typeof(PessoaFisica))
            {
                var pf = await _context.PessoasFisicas.FindAsync(id);
                resultado = new
                {
                    FornecedorId = fornecedor.FornecedorId,
                    Nome = fornecedor.Nome,
                    Email = fornecedor.Email,
                    Tipo = "PessoaFisica",
                    CPF = pf.CPF,
                    RG = pf.RG,
                    DataNascimento = pf.DataNascimento
                };
            }
            else if (tipo == typeof(Empresarial))
            {
                var emp = await _context.Empresariais.FindAsync(id);
                resultado = new
                {
                    FornecedorId = fornecedor.FornecedorId,
                    Nome = fornecedor.Nome,
                    Email = fornecedor.Email,
                    Tipo = "Empresarial",
                    CNPJ = emp.CNPJ
                };
            }

            return Ok(resultado);
        }

        // PUT: api/Fornecedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, [FromBody] object fornecedorObject)
        {
            var fornecedorJson = JsonSerializer.Serialize(fornecedorObject);

            if (fornecedorJson.Contains("CPF"))
            {
                var pessoaFisicaAtualizada = JsonSerializer.Deserialize<PessoaFisica>(
                    fornecedorJson
                );

                var pf = await _context.PessoasFisicas.FindAsync(id);

                if (pf == null)
                {
                    throw new Exception($"Fornecedor com o ID {id} não encontrado");
                }

                pf.Nome = pessoaFisicaAtualizada.Nome;
                pf.Email = pessoaFisicaAtualizada.Email;
                pf.CPF = pessoaFisicaAtualizada.CPF;
                pf.RG = pessoaFisicaAtualizada.RG;
                pf.DataNascimento = pessoaFisicaAtualizada.DataNascimento;

                _context.PessoasFisicas.Update(pf);
            }
            else if (fornecedorJson.Contains("CNPJ"))
            {
                var empresarialAtualizado = JsonSerializer.Deserialize<Empresarial>(fornecedorJson);

                var emp = await _context.Empresariais.FindAsync(id);

                if (emp == null)
                {
                    throw new Exception($"Fornecedor com o ID {id} não encontrado");
                }

                emp.Nome = empresarialAtualizado.Nome;
                emp.Email = empresarialAtualizado.Email;
                emp.CNPJ = empresarialAtualizado.CNPJ;

                _context.Empresariais.Update(emp);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id))
                {
                    return NotFound($"Empresa com o ID {id} não encontrado");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fornecedores
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedor(
            [FromBody] object fornecedorObject
        )
        {
            var fornecedorJson = JsonSerializer.Serialize(fornecedorObject);

            if (fornecedorJson.Contains("CNPJ"))
            {
                Empresarial empresarial = JsonSerializer.Deserialize<Empresarial>(fornecedorJson);
                if (empresarial is null)
                    return BadRequest();
                _context.Empresariais.Add(empresarial);
                await _context.SaveChangesAsync();
                return CreatedAtAction(
                    nameof(GetFornecedor),
                    new { id = empresarial.FornecedorId },
                    empresarial
                );
            }
            else if (fornecedorJson.Contains("CPF"))
            {
                PessoaFisica pessoaFisica = JsonSerializer.Deserialize<PessoaFisica>(
                    fornecedorJson
                );
                if (pessoaFisica is null)
                    return BadRequest();
                _context.PessoasFisicas.Add(pessoaFisica);
                await _context.SaveChangesAsync();
                return CreatedAtAction(
                    nameof(GetFornecedor),
                    new { id = pessoaFisica.FornecedorId },
                    pessoaFisica
                );
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.FornecedorId == id);
        }
    }
}
