using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace living_room_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
		public DbSet<Person> People { get; set; }
        public DbSet<Television> Televisions { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<HomeTheater> HomeTheaters { get; set; }
		public DbSet<PersonHomeTheater> PeopleHomeTheaters { get; set; }
        public DbSet<PersonComputer> PeopleComputers { get; set; }
        public DbSet<PersonTelevision> PeopleTelevisions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User { ID = "admin-1234", Name = "admin", Email = "admin@example.com", Password = "V1ZkU2RHRlhOSGhOYWsw" }); //Senha descriptografada = "admin123"

			modelBuilder.Entity<Person>()
				.HasData(
					new Person { ID = "1234567890", LastName = "blabla", FirstName = "blublu", CountryBirthLocation = "Brazil", Email = "email@example.com" });


    		// modelBuilder.Entity<Person>()
        	// 	.HasKey(b => b.ID);

    		// modelBuilder.Entity<Television>()
        	// 	.HasKey(b => b.ID);

			// modelBuilder.Entity<Computer>()
        	// 	.HasKey(b => b.ID);

    		// modelBuilder.Entity<HomeTheater>()
        	// 	.HasKey(b => b.ID);


			// modelBuilder.Entity<Person>()
			// 	.HasMany(b => b.Televisions)
			// 	.WithMany(c => c.People);

			// modelBuilder.Entity<Television>()
			// 	.HasMany(b => b.People)
			// 	.WithMany(c => c.Televisions);

			// modelBuilder.Entity<Person>()
			// 	.HasMany(b => b.Computers)
			// 	.WithMany(c => c.People);

			// modelBuilder.Entity<Computer>()
			// 	.HasMany(b => b.People)
			// 	.WithMany(c => c.Computers);

			// modelBuilder.Entity<Person>()
			// 	.HasMany(b => b.HomeTheaters)
			// 	.WithMany(c => c.People);

			// modelBuilder.Entity<HomeTheater>()
			// 	.HasMany(b => b.People)
			// 	.WithMany(c => c.HomeTheaters);


			modelBuilder.Entity<Television>()
				.HasData(
					new Television { ID = "1111111111", Brand = "Vony", Model = "bleble", isBeingSold = false });
        }
    }
}
