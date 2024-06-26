using DataBaseEngine.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Principal;
using System;
using DataBaseEngine.Attributes;

namespace DataBaseEngine.Abstract
{
    public class DbPgContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// settings set
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        public DbPgContext(DbContextOptions<DbPgContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=first_snow;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new EntityConfiguration<Settings>());

            foreach (var type in Assembly.GetAssembly(typeof(Entity)).GetTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var configType = typeof(EntityConfiguration<>).MakeGenericType(type);
                    var config = Activator.CreateInstance(configType);
                    GetType().GetMethod(nameof(ApplyConf), BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(type).Invoke(this, new object[] { modelBuilder, config });

                }
            }
        }

        private static void ApplyConf<T>(ModelBuilder modelBuilder, EntityConfiguration<T> config) where T : class, IEntity
        {
            modelBuilder.ApplyConfiguration(config);
        }
    }

    public class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class
    {
        /// <summary>
        /// Конфигурирование
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<T> builder)
        {
            try
            {
                var type = typeof(T);
                var typeAttribute = type.GetCustomAttribute<TableNameAttribute>();
                if (typeAttribute != null)
                {
                    builder.ToTable(typeAttribute.Name);
                }
                else
                {
                    builder.ToTable(type.Name);
                }

                foreach (var prop in type.GetProperties())
                {
                    var ignore = prop.GetCustomAttribute<IgnoreAttribute>();
                    if (ignore == null)
                    {
                        var pkAttr = prop.GetCustomAttribute<PrimaryKeyAttribute>();
                        if (pkAttr != null)
                        {
                            builder.HasKey(prop.Name);
                        }

                        var propAttribute = prop.GetCustomAttribute<ColumnNameAttribute>();
                        if (propAttribute != null)
                            builder.Property(prop.Name)
                                .HasColumnName(propAttribute.Name);
                        else
                            builder.Property(prop.Name)
                                .HasColumnName(prop.Name);

                        var ctAttr = prop.GetCustomAttribute<ColumnTypeAttribute>();
                        if (ctAttr != null)
                        {
                            builder.Property(prop.Name).HasColumnType(ctAttr.Name);
                        }
                    }
                    else
                    {
                        builder.Ignore(prop.Name);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);//for debug only
                throw;
            }
        }
    }
}
