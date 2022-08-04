using Microsoft.EntityFrameworkCore;
using SistemaPedidoEFCore.Domain;

//mapeando dados atravez do Fluent API
namespace SistemaPedidoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        //é possivel criar o modelo de dados a partir do DbSet<> quanto
        //  sobreescrevendo a funcao OnModelCreating
        public DbSet<Pedido> Pedidos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaDePedidosEFCoreConsole;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //o "p" é uma action, ou seja, o TypeBuilder se tem metodos para fazer o mapeamento do modelo de dados
            modelBuilder.Entity<Cliente>(p =>
            {
                p.ToTable("Clientes"); //nome da tabela na base de dados
                p.HasKey(p => p.Id); //informa a chave primaria
                p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired(); //define a propriedade de um campo/ Configura o tipo do dado/informa que o campo é obrigatorio
                p.Property(p => p.Telefone).HasColumnType("CHAR(11)"); //CHAR possui tamanho fixo e alocacao estatica na memoria
                p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
                p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

                p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone"); //cria um indice para um campo que possui uma grande quantidade de consultas, dando mais perfomance na aplicacao, com o Fluent API
            });

            modelBuilder.Entity<Produto>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.TipoProduto).HasConversion<string>(); //o tipo produto era enum, entao se quer armazenar no banco o dado com um determinado tipo utiliza o HasConversion 
            });

            modelBuilder.Entity<Pedido>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();// quando inserir uma informacao na base de dados o EFCore gera o comando GETDATE()
                p.Property(p => p.Status).HasConversion<string>();
                p.Property(p => p.TipoFrete).HasConversion<string>();
                p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                p.HasMany(p => p.Itens).WithOne(p => p.Pedido).OnDelete(DeleteBehavior.Cascade);//configura o relacionamento muitos para 1//e possibilita a restricao para o relacionamento, ou seja, quando deletar um pedido os itens do pedido serao deletados automaticamente
            });

            modelBuilder.Entity<PedidoItem>(p =>
            {
                p.ToTable("PedidoItens");
                p.HasKey(p => p.Id);
                p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();//quando cria uma instancia do pedido item e nao for informada a quantidade, automaticamente o EFCore ira preencher com o valor 1
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.Desconto).IsRequired();
            });
        }
    }
}