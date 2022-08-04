using SistemaPedidoEFCore.ValueObjects;
using System;

namespace SistemaPedidoEFCore.Domain
{
    public class PedidoItem
    {
        public int Id{ get; set; }
        public int PedidoID{ get; set; }
        public Pedido Pedido{ get; set; }
        public int ProdutoID{ get; set; }
        public Produto Produto{ get; set; }
        public int Quantidade{ get; set; }
        public decimal Valor {get; set; }
        public decimal Desconto {get; set; }
    }
}