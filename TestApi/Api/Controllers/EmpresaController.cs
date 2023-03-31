using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Console;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly ApiContext _context;

        public EmpresasController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Empresa
        [HttpGet]
        public async Task<ActionResult> GetEmpresas()
        {
            var empresas = await _context.Empresas.Include(e => e.Fornecedores).ToListAsync();
            var response = empresas.Select(
                empresa =>
                    new
                    {
                        empresa.EmpresaId,
                        empresa.CNPJ,
                        empresa.NomeFantasia,
                        empresa.CEP,
                        Fornecedores = empresa.Fornecedores
                            .Select(
                                fornecedor =>
                                    new
                                    {
                                        fornecedor.FornecedorId,
                                        fornecedor.Nome,
                                        fornecedor.Email,
                                        fornecedor.CEP,
                                        fornecedor.TipoFornecedor,
                                        CPF = fornecedor is PessoaFisica pfCpf ? pfCpf.CPF : null,
                                        RG = fornecedor is PessoaFisica pfRg ? pfRg.RG : null,
                                        CNPJ = fornecedor is Empresarial emp ? emp.CNPJ : null,
                                        DataNascimento = fornecedor is PessoaFisica pfNasc
                                            ? pfNasc.DataNascimento
                                            : DateTime.MinValue
                                    }
                            )
                            .ToList()
                    }
            );
            return Ok(response);
        }

        // GET: api/Empresa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.Fornecedores)
                .ThenInclude(f => f.Empresas)
                .FirstOrDefaultAsync(e => e.EmpresaId == id);

            if (empresa == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(empresa, options));
        }

        // POST: api/Empresa
        [HttpPost]
        public async Task<IActionResult> PostEmpresa([FromBody] Empresa empresa)
        {
            var fornecedores = new List<Fornecedor>();
            foreach (var fornecedor in empresa.Fornecedores)
            {
                var fornecedorId = fornecedor.FornecedorId;
                var currentFornecedor = await _context.Fornecedores.FindAsync(fornecedorId);
                if (currentFornecedor == null)
                {
                    return NotFound();
                }
                fornecedores.Add(currentFornecedor);
            }
            empresa.Fornecedores = fornecedores;
            _context.Empresas.Add(empresa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.EmpresaId }, empresa);
        }

        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresaUpdate)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            var fornecedores = new List<Fornecedor>();

            if (empresa == null)
            {
                throw new Exception($"Empresa com o ID {id} não encontrado");
            }

            foreach (var fornecedor in empresaUpdate.Fornecedores)
            {
                var fornecedorId = fornecedor.FornecedorId;
                var currentFornecedor = await _context.Fornecedores.FindAsync(fornecedorId);
                if (currentFornecedor == null)
                {
                    return NotFound();
                }
                fornecedores.Add(currentFornecedor);
            }

            empresa.NomeFantasia = empresaUpdate.NomeFantasia;
            empresa.CNPJ = empresaUpdate.CNPJ;
            empresa.NomeFantasia = empresaUpdate.NomeFantasia;
            empresa.CEP = empresaUpdate.CEP;
            empresa.Fornecedores = fornecedores;

            _context.Empresas.Update(empresa);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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

        // DELETE: api/Empresa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.EmpresaId == id);
        }
    }
}
