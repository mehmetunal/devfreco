using Dev.Data.Npgsql;
using Dev.Core.DbType.Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Dev.Npgsql.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder BaseModelBuilder<Table>(this ModelBuilder builder) where Table : BaseEntity
        {
            builder.Entity<Table>().HasKey(p => p.Id);

            builder.Entity<Table>().Property(p => p.Id)
                .HasColumnType(ColumnType.Serial)
                .ValueGeneratedOnAdd();

            builder.Entity<Table>().Property(p => p.CreatedDate)
                .HasColumnType(ColumnType.Timestamp)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.CreatorIP)
                .HasColumnType(ColumnType.Varchar)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.CreatorUserId)
                .HasColumnType(ColumnType.Numeric)
                .HasPrecision(18, 0)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.ModifiedDate)
                .HasColumnType(ColumnType.Timestamp);

            builder.Entity<Table>().Property(p => p.ModifierIP)
                .HasColumnType(ColumnType.Varchar)
                .HasMaxLength(50);

            builder.Entity<Table>().Property(p => p.ModifierUserId)
                .HasColumnType(ColumnType.Numeric)
                .HasPrecision(18, 0);


            return builder;

        }
    }
}
