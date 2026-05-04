using GestionProducto.Domain;
using GestionProducto.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionProducto.Infra.Persistence;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {

    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Activo)
                .HasDefaultValue(true);

            entity.Property(u => u.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            entity.Property<DateTime>("CreateAt")
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");
            entity.Property<DateTime>("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        });

        // Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(r => r.Nombre)
                .IsUnique();

            entity.Property<DateTime>("CreateAt")
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");
            entity.Property<DateTime>("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        });

        // UsuarioRol (muchos a muchos manual)
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(ur => new { ur.UsuarioId, ur.RolId });

            entity.HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            entity.HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.RolId);

            entity.Property<DateTime>("CreateAt")
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");
            entity.Property<DateTime>("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        });

        // Tabla Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Codigo)
            .IsRequired()
            .HasMaxLength(30);

            entity.HasIndex(e => e.Codigo)
            .IsUnique();

            entity.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(50);

            entity.Property(e => e.StockActual)
            .IsRequired()
            .HasDefaultValue(0);

            entity.Property(e => e.StockMinimo)
            .IsRequired()
            .HasDefaultValue(5);

            entity.Property(e => e.PrecioCompra)
            .IsRequired()
            .HasPrecision(10, 2);

            entity.Property(e => e.PrecioVenta)
            .IsRequired()
            .HasPrecision(10, 2);

            entity.Property(e => e.Activo)
            .HasDefaultValue(true);


            entity.Property<DateTime>("CreateAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
            entity.Property<DateTime>("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        });

        // Tabla Movimiento
        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ProductoId)
           .IsRequired();

            entity.Property(e => e.Tipo)
            .IsRequired()
            .HasConversion<string>();

            entity.Property(e => e.Motivo)
            .IsRequired();

            entity.Property(e => e.Cantidad)
            .IsRequired()
            .HasDefaultValue(1);

            entity.Property(e => e.Fecha)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

            // Llave foranea
            entity.HasOne(e => e.Producto)
            .WithMany()
            .HasForeignKey(e => e.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

            // Busqueda rapida
            entity.HasIndex(e => e.ProductoId);
            entity.HasIndex(e => e.Fecha);


            entity.Property<DateTime>("CreateAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
            entity.Property<DateTime>("UpdatedAt")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        // detectar que elementos se modifican
        var entries = ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Modified);


        foreach (var entry in entries)
        {
            // buscar un campo en la BD
            if (entry.Metadata.FindProperty("UpdatedAt") != null)
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                if (entry.Metadata.FindProperty("CreateAt") != null)
                    entry.Property("CreateAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
