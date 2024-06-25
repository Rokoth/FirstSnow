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

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.Active).HasColumnName("active");
            builder.Property(s => s.Deleted).HasColumnName("deleted");
            builder.Property(s => s.Login).HasColumnName("login");
            builder.Property(s => s.Name).HasColumnName("name");
            builder.Property(s => s.Password).HasColumnName("password");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
        }
    }

    public class MapConfiguration : IEntityTypeConfiguration<Map>
    {
        public void Configure(EntityTypeBuilder<Map> builder)
        {
            builder.ToTable("map").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.Name).HasColumnName("name");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");

            builder.HasOne(s => s.Position).WithOne().HasForeignKey<MapPosition>(s => s.MapId).HasPrincipalKey<Map>(s => s.Id);
            builder.HasMany(s => s.Fields).WithOne().HasForeignKey(s => s.MapId).HasPrincipalKey(s => s.Id);
            builder.HasMany(s => s.Persons).WithOne().HasForeignKey(s => s.MapId).HasPrincipalKey(s => s.Id);
        }
    }

    public class MapPositionConfiguration : IEntityTypeConfiguration<MapPosition>
    {
        public void Configure(EntityTypeBuilder<MapPosition> builder)
        {
            builder.ToTable("map_position").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.X).HasColumnName("x");
            builder.Property(s => s.Y).HasColumnName("y");
            builder.Property(s => s.MapId).HasColumnName("map_id");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
        }
    }

    public class FieldPositionConfiguration : IEntityTypeConfiguration<FieldPosition>
    {
        public void Configure(EntityTypeBuilder<FieldPosition> builder)
        {
            builder.ToTable("field_position").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.X).HasColumnName("x");
            builder.Property(s => s.Y).HasColumnName("y");
            builder.Property(s => s.FieldId).HasColumnName("field_id");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
        }
    }

    public class FieldConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable("field").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.MapId).HasColumnName("map_id");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");

            builder.HasOne(s => s.Position).WithOne().HasForeignKey<FieldPosition>(s => s.FieldId).HasPrincipalKey<Field>(s => s.Id);
            builder.HasMany(s => s.Properties).WithOne().HasForeignKey(s => s.FieldId).HasPrincipalKey(s => s.Id);
        }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("person").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.MapId).HasColumnName("map_id");
            builder.Property(s => s.Name).HasColumnName("name");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
            builder.Property(s => s.UserId).HasColumnName("user_id");

            builder.HasOne(s => s.Position).WithOne().HasForeignKey<FieldPosition>(s => s.FieldId).HasPrincipalKey<Person>(s => s.Id);
            builder.HasMany(s => s.PersonProperties).WithOne().HasForeignKey(s => s.PersonId).HasPrincipalKey(s => s.Id);
        }
    }

    public class PersonPropertyConfiguration : IEntityTypeConfiguration<PersonProperty>
    {
        public void Configure(EntityTypeBuilder<PersonProperty> builder)
        {
            builder.ToTable("person_property").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.DefaultValue).HasColumnName("default_value");
            builder.Property(s => s.MaxValue).HasColumnName("max_value");
            builder.Property(s => s.MinValue).HasColumnName("min_value");
            builder.Property(s => s.PersonId).HasColumnName("person_id");
            builder.Property(s => s.PropertyType).HasColumnName("property_type");
            builder.Property(s => s.Value).HasColumnName("value");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
        }
    }

    public class FieldPropertyConfiguration : IEntityTypeConfiguration<FieldProperty>
    {
        public void Configure(EntityTypeBuilder<FieldProperty> builder)
        {
            builder.ToTable("field_property").HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");
            builder.Property(s => s.DefaultValue).HasColumnName("default_value");
            builder.Property(s => s.MaxValue).HasColumnName("max_value");
            builder.Property(s => s.MinValue).HasColumnName("min_value");
            builder.Property(s => s.FieldId).HasColumnName("field_id");
            builder.Property(s => s.PropertyType).HasColumnName("property_type");
            builder.Property(s => s.Value).HasColumnName("value");
            builder.Property(s => s.VersionDate).HasColumnName("version_date");
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
