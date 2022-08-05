// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.EntityFrameworkCore;

namespace SistemaDePedidosEFCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //aplica as migracoes na base de dados, nao é indicado para producao
            using var db = new SistemaPedidoEFCore.Data.ApplicationContext();
            db.Database.Migrate();

            //verificando migracao pendente
            var existe = db.Database.GetPendingMigrations().Any();
            if (existe)
            {
                //regra de negocio para reverter o problema de migracoes pendentes, como por exemplo fazer uma notificacao ou finalizando a aplicacao 
            }
        }
    }
}