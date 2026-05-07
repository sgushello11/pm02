using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class AgroControlDbContext : DbContext
    {
        public AgroControlDbContext(DbContextOptions<AgroControlDbContext> options)
            : base(options)
        { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionBatch> ProductionBatches { get; set; }
        public DbSet<ProductionStep> ProductionSteps { get; set; }
        public DbSet<QualityControl> QualityControls { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<TechMap> TechMaps { get; set; }
        public DbSet<TechMapStep> TechMapSteps { get; set; }
        public DbSet<Deviation> Deviations { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<RecipeComponent> RecipeComponents { get; set; }
        public DbSet<ExtruderProgram> ExtruderPrograms { get; set; }
        public DbSet<ExtruderProgramZone> ExtruderProgramZones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // users
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
            modelBuilder.Entity<Users>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Users>().HasIndex(u => u.Email).IsUnique();

            // products
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasIndex(p => p.Code).IsUnique();

            // recipes
            modelBuilder.Entity<Recipe>().ToTable("recipes");
            modelBuilder.Entity<Recipe>().HasKey(r => r.Id);

            // production_orders
            modelBuilder.Entity<ProductionOrder>().ToTable("production_orders");
            modelBuilder.Entity<ProductionOrder>().HasKey(o => o.Id);
            modelBuilder.Entity<ProductionOrder>().HasIndex(o => o.OrderNumber).IsUnique();

            // production_batches
            modelBuilder.Entity<ProductionBatch>().ToTable("production_batches");
            modelBuilder.Entity<ProductionBatch>().HasKey(b => b.Id);
            modelBuilder.Entity<ProductionBatch>().HasIndex(b => b.BatchNumber).IsUnique();

            // production_steps
            modelBuilder.Entity<ProductionStep>().ToTable("production_steps");
            modelBuilder.Entity<ProductionStep>().HasKey(s => s.Id);

            // quality_control
            modelBuilder.Entity<QualityControl>().ToTable("quality_control");
            modelBuilder.Entity<QualityControl>().HasKey(q => q.Id);

            // equipment
            modelBuilder.Entity<Equipment>().ToTable("equipment");
            modelBuilder.Entity<Equipment>().HasKey(e => e.Id);
            modelBuilder.Entity<Equipment>().HasIndex(e => e.Code).IsUnique();

            // tech_maps
            modelBuilder.Entity<TechMap>().ToTable("tech_maps");
            modelBuilder.Entity<TechMap>().HasKey(t => t.Id);

            // tech_map_steps
            modelBuilder.Entity<TechMapStep>().ToTable("tech_map_steps");
            modelBuilder.Entity<TechMapStep>().HasKey(ts => ts.Id);

            // deviations
            modelBuilder.Entity<Deviation>().ToTable("deviations");
            modelBuilder.Entity<Deviation>().HasKey(d => d.Id);

            // Настройка decimal полей
            modelBuilder.Entity<ProductionBatch>(entity =>
            {
                entity.Property(e => e.ActualQuantityKg).HasPrecision(18, 2);
            });

            modelBuilder.Entity<ProductionOrder>(entity =>
            {
                entity.Property(e => e.PlannedQuantityKg).HasPrecision(18, 2);
            });

            modelBuilder.Entity<ProductionStep>(entity =>
            {
                entity.Property(e => e.PlannedTempC).HasPrecision(8, 2);
                entity.Property(e => e.ActualTempC).HasPrecision(8, 2);
                entity.Property(e => e.PlannedPressureBar).HasPrecision(8, 2);
                entity.Property(e => e.ActualPressureBar).HasPrecision(8, 2);
            });

            modelBuilder.Entity<QualityControl>(entity =>
            {
                entity.Property(e => e.MeasuredValue).HasPrecision(18, 4);
            });

            // Внешние ключи
            modelBuilder.Entity<ProductionOrder>()
                .HasOne<Recipe>()
                .WithMany()
                .HasForeignKey(o => o.RecipeId);

            modelBuilder.Entity<ProductionBatch>()
                .HasOne<ProductionOrder>()
                .WithMany()
                .HasForeignKey(b => b.OrderId);

            modelBuilder.Entity<ProductionStep>()
                .HasOne<ProductionBatch>()
                .WithMany()
                .HasForeignKey(s => s.BatchId);

            modelBuilder.Entity<QualityControl>()
                .HasOne<ProductionBatch>()
                .WithMany()
                .HasForeignKey(q => q.BatchId);

            modelBuilder.Entity<Deviation>()
                .HasOne<ProductionBatch>()
                .WithMany()
                .HasForeignKey(d => d.BatchId);

            modelBuilder.Entity<TechMapStep>()
                .HasOne<Equipment>()
                .WithMany()
                .HasForeignKey(ts => ts.EquipmentId);

            base.OnModelCreating(modelBuilder);

            // materials
            modelBuilder.Entity<Material>().ToTable("materials");
            modelBuilder.Entity<Material>().HasKey(m => m.Id);
            modelBuilder.Entity<Material>().HasIndex(m => m.Code).IsUnique();

            // recipe_components
            modelBuilder.Entity<RecipeComponent>().ToTable("recipe_components");
            modelBuilder.Entity<RecipeComponent>().HasKey(rc => rc.Id);

            // Связь RecipeComponent -> Recipe
            modelBuilder.Entity<RecipeComponent>()
                .HasOne<Recipe>()
                .WithMany()
                .HasForeignKey(rc => rc.RecipeId);

            // Связь RecipeComponent -> Material
            modelBuilder.Entity<RecipeComponent>()
                .HasOne<Material>()
                .WithMany()
                .HasForeignKey(rc => rc.MaterialId);

            // extruder_programs
            modelBuilder.Entity<ExtruderProgram>().ToTable("extruder_programs");
            modelBuilder.Entity<ExtruderProgram>().HasKey(ep => ep.Id);

            // extruder_program_zones
            modelBuilder.Entity<ExtruderProgramZone>().ToTable("extruder_program_zones");
            modelBuilder.Entity<ExtruderProgramZone>().HasKey(epz => epz.Id);
        }
    }
}