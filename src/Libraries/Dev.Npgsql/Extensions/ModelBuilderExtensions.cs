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

            /*
             HasDefaultValue(true);
             modelBuilder.Entity<Company>()
                         .HasMany(c => c.Employees)
                         .WithOne(e => e.Company)
                         .HasConstraintName("MyFKConstraint");
             
             */
            //IsUnique  => Benzersiz değerse indexde bunu ekleyebiliriz.
            //builder.Entity<Table>()
            //   .HasIndex(p => new { p.CreatedDate, p.ModifiedDate }, name: $"IX_{nameof(Table)}_CreatedDate");

            builder.Entity<Table>()
                .HasIndex(p => p.CreatedDate, name: $"IX_{nameof(Table)}_CreatedDate");
            builder.Entity<Table>()
                .HasIndex(p => p.IsDeleted, name: $"IX_{nameof(Table)}_IsDeleted");
            builder.Entity<Table>()
                .HasIndex(p => p.IsPublish, name: $"IX_{nameof(Table)}_IsPublish");


            builder.Entity<Table>().Property(p => p.Id).HasDefaultValueSql("uuid_generate_v1()")
                .ValueGeneratedOnAdd();

            builder.Entity<Table>().Property(p => p.CreatedDate)
                .HasColumnType(ColumnType.Timestamp)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.CreatorIP)
                .HasColumnType(ColumnType.Varchar)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.CreatorUserId)
                .IsRequired();

            builder.Entity<Table>().Property(p => p.ModifiedDate)
                .HasColumnType(ColumnType.Timestamp);

            builder.Entity<Table>().Property(p => p.ModifierIP)
                .HasColumnType(ColumnType.Varchar)
                .HasMaxLength(50);

            builder.Entity<Table>().Property(p => p.ModifierUserId);


            return builder;

        }

        public static async Task EnableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, true);
        public static async Task DisableIdentityInsertAsync<T>(this DbContext context) => await SetIdentityInsertAsync<T>(context, false);

        private static async Task SetIdentityInsertAsync<T>([NotNull] DbContext context, bool enable)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }
    }
}
