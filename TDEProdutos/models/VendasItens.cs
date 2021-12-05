using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDEProdutos.models
{
    public class VendasItens
    {
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string CodigoProduto { get; set; }

        public string NomeProduto { get; set; }

        public decimal ValorUnitario { get; set; }

        public int Qtde { get; set; }

        public decimal ValorTotal { get; set; }
    }
}
