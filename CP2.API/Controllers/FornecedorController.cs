﻿using CP2.Domain.Entities;
using CP2.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CP2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorService _applicationService;

        public FornecedorController(IFornecedorService applicationService)
        {
            _applicationService = applicationService;
        }

        // GET: api/fornecedor
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var fornecedores = await _applicationService.ObterTodosFornecedores();
            return Ok(fornecedores);
        }

        // GET: api/fornecedor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var fornecedor = await _applicationService.ObterFornecedorPorId(id);
            return fornecedor is not null ? Ok(fornecedor) : NotFound("Fornecedor não encontrado.");
        }

        // POST: api/fornecedor
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FornecedorEntity entity)
        {
            if (entity == null)
            {
                return BadRequest("Fornecedor não pode ser nulo.");
            }

            try
            {
                var novoFornecedor = await _applicationService.AddAsync(entity);
                return CreatedAtAction(nameof(GetPorId), new { id = novoFornecedor.Id }, novoFornecedor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // PUT: api/fornecedor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FornecedorEntity entity)
        {
            if (entity == null)
            {
                return BadRequest("Fornecedor não pode ser nulo.");
            }

            try
            {
                var fornecedorExistente = await _applicationService.ObterFornecedorPorId(id);
                if (fornecedorExistente == null) return NotFound("Fornecedor não encontrado.");

                // Atualize as propriedades conforme necessário
                fornecedorExistente.Nome = entity.Nome;
                // Atualize outras propriedades aqui

                await _applicationService.UpdateAsync(fornecedorExistente);
                return Ok(fornecedorExistente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // DELETE: api/fornecedor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fornecedor = await _applicationService.ObterFornecedorPorId(id);
            if (fornecedor == null) return NotFound("Fornecedor não encontrado.");

            await _applicationService.DeleteAsync(id);
            return Ok("Fornecedor removido com sucesso.");
        }
    }
}
