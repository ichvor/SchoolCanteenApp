using System.Data.Entity;
using SchoolCanteenApp.Models;

namespace SchoolCanteenApp.Data
{
    public class SchoolCanteenContext : DbContext
    {
        public SchoolCanteenContext() : base("SchoolCanteenContext")
        {
            // Отключаем ленивую загрузку для избежания циклических ссылок
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<Paid> Paids { get; set; }
        public DbSet<Day> Days { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка связей
            modelBuilder.Entity<Class>()
                .HasOptional(c => c.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.IdTeacher);

            modelBuilder.Entity<Student>()
                .HasOptional(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.IdClass);

            // Настройка связей для MealPlan
            modelBuilder.Entity<MealPlan>()
                .HasRequired(mp => mp.Student)
                .WithMany(s => s.MealPlans)
                .HasForeignKey(mp => mp.IdStudent);

            modelBuilder.Entity<MealPlan>()
                .HasRequired(mp => mp.Meal)
                .WithMany(m => m.MealPlans)
                .HasForeignKey(mp => mp.IdMeal);

            modelBuilder.Entity<MealPlan>()
                .HasRequired(mp => mp.Paid)
                .WithMany(p => p.MealPlans)
                .HasForeignKey(mp => mp.IdPaid);

            modelBuilder.Entity<MealPlan>()
                .HasRequired(mp => mp.Day)
                .WithMany(d => d.MealPlans)
                .HasForeignKey(mp => mp.IdDay);

            // Многие-ко-многим: Dish-Ingredient
            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Ingredients)
                .WithMany(i => i.Dishes)
                .Map(m =>
                {
                    m.ToTable("DishIngredient");
                    m.MapLeftKey("IdDish");
                    m.MapRightKey("IdIngredient");
                });

            // Многие-ко-многим: Meal-Dish
            modelBuilder.Entity<Meal>()
                .HasMany(m => m.Dishes)
                .WithMany(d => d.Meals)
                .Map(m =>
                {
                    m.ToTable("MealDish");
                    m.MapLeftKey("IdMeal");
                    m.MapRightKey("IdDish");
                });
        }
    }
}