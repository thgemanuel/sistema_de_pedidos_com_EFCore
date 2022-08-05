// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.EntityFrameworkCore;
using SistemaPedidoEFCore.Domain;

namespace SistemaDePedidosEFCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //aplica as migracoes na base de dados, nao é indicado para producao
            // using var db = new SistemaPedidoEFCore.Data.ApplicationContext();
            // db.Database.Migrate();

            //verificando migracao pendente
            // var existe = db.Database.GetPendingMigrations().Any();
            // if (existe)
            // {
                //regra de negocio para reverter o problema de migracoes pendentes, como por exemplo fazer uma notificacao ou finalizando a aplicacao 
            // }

            //chama funcao para inserir dados
            InserirDados();
        }

        //inserindo um registro na base de dados
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = SistemaPedidoEFCore.ValueObjects.TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();
            
            //caso todas as opcoes estejam descomentadas, mesmo todas executando somente um registro sera inserido
            // pois o que é monitorado pelo EF COre é a instancia do objeto, assim qualquer metodo chamado nao afertara a instancia do registro
            // o EFCore rastreia a instancia especifica

            //priemira opcao de insercao
            //db.Produtos.Add(produto);

            //segunda opcao de insercao generico
            //db.Set<Produto>().Add(produto);

            //terceira opcao, forcando rastreamento
            // db.Entry(produto).State = EntityState.Added;

            //a quarta instancia é pela propria instacia do contexto, nao indica o tipo do objeto
            db.Add(produto);

            //as alteracao sao persistidas nas bases de dados
            var registros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {registros}");
        }
    }
}