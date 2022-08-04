//mapeando modelo de dados em arquivos separados evitando auto acoplamento de codigo
// fazendo com que a manutencao se torne mais facil
// cria arquivos com suas respectivas responsabilidades

//esse arquivo mapeia o modelo de dados para pedidoItem
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidoEFCore.Domain;

namespace SistemaPedidoEFCore.Data.Configurations
{
    public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();//quando cria uma instancia do pedido item e nao for informada a quantidade, automaticamente o EFCore ira preencher com o valor 1
            builder.Property(p => p.Valor).IsRequired();
            builder.Property(p => p.Desconto).IsRequired();
        }
    }
}