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
            //InserirDados();

            //chama funcao para inserir dados em massa
            //InserirDadosEmMassa();

            //consultar dados
            // ConsultarDados();

            //cadastrando pedido na base de dados
            // CadastrarPedido();

            //metodo consulta carregamento adiantado
            // ConsultarPedidoCarregamentoAdiantado();

            //metodo para atualizar dados
            AtualizarDados();
        }

        //inserindo dados em massa
        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste Massa",
                CodigoBarras = "1234567456234",
                Valor = 20m,
                TipoProduto = SistemaPedidoEFCore.ValueObjects.TipoProduto.Embalagem,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Cliente Teste",
                CEP = "99999000",
                Cidade = "Almenara",
                Estado = "MG",
                Telefone = "99988877745",
            };

            var listaCliente = new[]{
                new Cliente
                {
                    Nome = "Cliente Teste Lista 1",
                    CEP = "99999000",
                    Cidade = "Almenara",
                    Estado = "MG",
                    Telefone = "99988877745",
                },
                new Cliente
                {
                    Nome = "Cliente Teste Lista 2",
                    CEP = "99999000",
                    Cidade = "Almenara",
                    Estado = "MG",
                    Telefone = "99988877746",
                },
            };

            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();
            //adicionando instancias de diferentes tipos
            //db.AddRange(produto, cliente);

            //adicionando uma lista de instancias
            // db.AddRange(listaCliente);

            //tambem se pode adicionar de outra forma
            db.Set<Cliente>().AddRange(listaCliente);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {registros}");
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

        //consulta no BD
        private static void ConsultarDados()
        {
            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();
            //consulta por sintaxe
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();

            //consulta por metodo em memoria a partir do rastreamento
            var consultaPorMetodo = db.Clientes
            .Where(p => p.Id > 0)
            .OrderBy(p => p.Id)  //ordenando a consulta
            .ToList();

            //consulta por metodo forcando acesso ao BD com AsNoTracking(), nao criando copia em memoria e nao fazendo rastreamento
            //var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList();
            foreach (var cliente in consultaPorMetodo)
            {
                //escrevendo o cliente que esta sendo consultado
                Console.Write($"Consultando Cliente: {cliente.Id}");


                //executando consulta primeiro os objetos em memoria e se n encontrar, executa a consulta na BD
                //essa consulta é feita na chave primaria da entidade
                //apenas o metodo Find faz a consulta primeiramente na memoria para depois no BD, os demais somente consultam BD
                //db.Clientes.Find(cliente.Id);

                //retorna a primeira instancia encontrada no BD que satisfaz a condicao 
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }
        //carregamento adiantado
        private static void CadastrarPedido()
        {
            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Clientes.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteID = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = SistemaPedidoEFCore.ValueObjects.StatusPedido.Analise,
                TipoFrete = SistemaPedidoEFCore.ValueObjects.TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoID = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();

            //o metodo Include faz o carregamento adiantado, nele é informado a propriedade de navegacao que o EFCore ira carregar automaticamente
            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto) //include do que estado no metodo Include, ou seja é o segundo nivel do Include
                .ToList();

            Console.WriteLine(pedidos.Count);
        }


        //atualizar registros
        private static void AtualizarDados()
        {
            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();

            //consultando o cliente com id == 1
            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Cliente alterado passo 1";

            //atualizando a entidade, porem essa abordagem nao é muito boa pois atualiza todos os campus da entidade
            // db.Clientes.Update(cliente);

            //para resolver isso, apenas chamando o SaveChanges sem o update, somente o campo nome sera atualizado

            //outra forma para atualizar dados é a partir do rastreamento de forma explicita
            // db.Entry(cliente).State = EntityState.Modified;


            //---------- CENARIO DESCONECTADO -------------
            //um cenario desconectado acontece quando os dados ainda não estao instanciados
            // como por exemplo numa aplicacao FrontEnd que envia dados de cadastro de clientes para uma API
            // e a API que tem acesso ao banco de dados, e ela trata os dados para serem atualizados
            
            //é possivel ao invez de consultar a base de dados para obter o ID do cliente, ter uma instancia de um cliente e para ele ja ser informado o ID dele
            var clienteJaInstanciado = new Cliente{
                Id = 1,
            };
            var clienteDesconectadoRastreado = new
            {
                Nome = "Cliente Desconectado Rastreado",
                Telefone = "4567891234"
            };

            //fazer com que o objeto comece a ser rastreado, pois ela n é provinda de uma consulta no BD
            db.Attach(clienteJaInstanciado);
            db.Entry(clienteJaInstanciado).CurrentValues.SetValues(clienteDesconectadoRastreado);


            //---------- FIM CENARIO DESCONECTADO -------------

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado",
                Telefone = "4567891234"
            };

            //atualizando o cliente com os valores
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();

        }
    }
}