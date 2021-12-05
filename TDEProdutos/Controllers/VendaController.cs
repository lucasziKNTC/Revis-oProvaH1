using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEProdutos.Context;
using TDEProdutos.models;

namespace TDEProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly ProdutoContext context;
        public VendaController()
        {
            context = new ProdutoContext();

        }


        //  [Authorize]
        [HttpPost("RegistrarVenda")]
        public ActionResult RegistrarVenda(Venda venda)
        {

            foreach (var item in venda.Itens)
            {
                var resultado = context._produtos.Find<Produto>(p => p.Codigo == item.CodigoProduto).FirstOrDefault();
                if (resultado == null)
                {
                    return NotFound($"O produto {item.CodigoProduto} não existe na base de dados, venda nao pde ser feita");
                }

                if (resultado.EstoqueAtual < item.Qtde)
                {
                    return BadRequest($"O produto {item.Qtde} nao pode ter venda relizada, Venda maior que o estoque atual!");
                }

                resultado.EstoqueAtual = resultado.EstoqueAtual - item.Qtde;

                context._produtos.ReplaceOne<Produto>(p => p.Id == resultado.Id, resultado);

            }
            context._VendaProduto.InsertOne(venda);
            return Ok(venda);

        }

    }
}

