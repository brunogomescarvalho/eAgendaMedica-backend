using EAgendaMedica.Dominio.ModuloMedico;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAgendaMedica.Infra.ModuloMedico
{
    public class MapeadorMedico : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("TB_Medico");

            builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();
            builder.Property(x => x.CRM).HasColumnType("varchar(30)").IsRequired();
            builder.Property(e => e.Nome).HasColumnType("varchar(100)").IsRequired();
            builder.Property(e => e.Ativo).IsRequired();
            builder.Property(e=>e.Prefixo).IsRequired();
            builder.Ignore(x => x.HorasTrabalhadasNoPeriodo);
        }
    }
}



