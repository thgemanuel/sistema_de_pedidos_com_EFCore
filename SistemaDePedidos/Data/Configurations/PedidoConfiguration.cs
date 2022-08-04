//mapeando modelo de dados em arquivos separados evitando auto acoplamento de codigo
// fazendo com que a manutencao se torne mais facil
// cria arquivos com suas respectivas responsabilidades

//esse arquivo mapeia o modelo de dados para pedido
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidoEFCore.Domain;

namespace SistemaPedidoEFCore.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();// quando inserir uma informacao na base de dados o EFCore gera o comando GETDATE()
            builder.Property(p => p.Status).HasConversion<string>();
            builder.Property(p => p.TipoFrete).HasConversion<string>();
            builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

            builder.HasMany(p => p.Itens).WithOne(p => p.Pedido).OnDelete(DeleteBehavior.Cascade);//configura o relacionamento muitos para 1//e possibilita a restricao para o relacionamento, ou seja, quando deletar um pedido os itens do pedido serao deletados automaticamente
        }
    }
}