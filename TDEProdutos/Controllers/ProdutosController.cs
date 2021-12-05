using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.Context;
using TDEProdutos.models;
using TDEProdutos.Validations;
using Microsoft.AspNetCore.Authorization;

namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProdutosController : Controller
    {
        private readonly ProdutoContext Context;

        public ProdutosController()
        {
            Context = new ProdutoContext();

        }


        [HttpGet]
        public ActionResult ola()
        {
            return Ok("Ola");
        }

        /// <summary>
        /// Consulta dados de uma pessoa a partir do CPF
        /// Requer uso de token.
        /// </summary>
        /// <param name="Id">CPF</param>
        /// <returns>Objeto contendo os dados de uma pessoa.</returns>
        [Authorize]

        [HttpGet("BuscarPorId/{Id}")]

        public ActionResult BuscarPorId(string Id)
        {

            return Ok(Context._produtos.Find<Produto>(p => p.Id == Id).FirstOrDefault());
        }


        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult Adicionar(Produto Produto)
        {
            ProdutoValidation produtoValidation = new ProdutoValidation();

            var validacao = produtoValidation.Validate(Produto);

            if (!validacao.IsValid)
            {
                List<string> erros = new List<string>();
                foreach (var failure in validacao.Errors)
                {
                    erros.Add("Property " + failure.PropertyName +
                        " failed validation. Error Was: "
                        + failure.ErrorMessage);
                }

                return BadRequest(erros);
                
            }
            Context._produtos.InsertOne(Produto);
            return Ok("Produto cadastrado");
            
        }



        [HttpPut("Atualizar/{Id}")]
        public ActionResult Atualizar(string Id, [FromBody] Produto Produto)
        {
            var pResultado = Context._produtos.Find<Produto>(p => p.Id == Id).FirstOrDefault();
            if (pResultado == null) return
            NotFound("Id não encontrado, atualizacao não realizada!");

            Produto.Id = Id;
            Context._produtos.ReplaceOne<Produto>(p => p.Id == Id, Produto);

            return Ok("Produto atualizado com sucesso");


        }

        [HttpDelete("Remover/{Id}")]
        public ActionResult Remover(string Id)
        {
            var pResultado = Context._produtos.Find<Produto>(p => p.Id == Id).FirstOrDefault();
            if (pResultado == null) return
                    NotFound("Id não encontrada, atualizacao não realizada!");

            Context._produtos.DeleteOne<Produto>(filter => filter.Id == Id);
            return Ok("Produto removido com sucesso");
        }




    }


}

