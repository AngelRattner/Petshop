using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetShop.Data
{
    public class AnimalContext : DbContext
    {

        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
        {
        }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Comments> commentes { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>().HasData(
                new { AnimalId = 1, Name = "dog", Age = 9, PictureName = "dog.jpg", Description = "the best animal ever", CategoryId = 1 },
                new { AnimalId = 2, Name = "cat", Age = 5, PictureName = "cat.jpg", Description = "a cat..", CategoryId = 1 },
                new { AnimalId = 3, Name = "spider", Age = 200, PictureName = "Spider.jpg", Description = "nop", CategoryId = 3 },
                new { AnimalId = 4, Name = "bird", Age = 2, PictureName = "bird.jpg", Description = "it can fly!", CategoryId = 4 },
                new { AnimalId = 5, Name = "Desert rain frog", Age = 5, PictureName = "frog.jpg", Description = "small frog with high voice", CategoryId = 2 },
                new { AnimalId = 6, Name = " Anura", Age = 4, PictureName = "Anura.jpg", Description = "the cutest Amphibians you can find ", CategoryId = 2 },
                new { AnimalId = 7, Name = "indian frogmouth", Age = 20, PictureName = "frogmouth.jpg", Description = "that may sound like a frog but it will surprise you", CategoryId = 4 },
                new { AnimalId = 8, Name = "scorpion", Age = 8, PictureName = "scorpion.jpg", Description = "just run away!!", CategoryId = 3 }
            );
            modelBuilder.Entity<Comments>().HasData(
                new { AnimalId = 1, CommentId = 1, Comment = "so cute!" },
                new { AnimalId = 2, CommentId = 2, Comment = "much wow!!" },
                new { AnimalId = 3, CommentId = 3, Comment = "wow" },
                new { AnimalId = 3, CommentId = 4, Comment = "he is so small!" },
                new { AnimalId = 3, CommentId = 5, Comment = "ohh no" },
                new { AnimalId = 1, CommentId = 6, Comment = "flaf" },
                new { AnimalId = 4, CommentId = 7, Comment = "wow it is so fast" },
                new { AnimalId = 5, CommentId = 8, Comment = "it can reee hahaha" },
                new { AnimalId = 6, CommentId = 9, Comment = "cute" },
                new { AnimalId = 8, CommentId = 10, Comment = "scary" }
            );
            modelBuilder.Entity<Category>().HasData(
                new { Name = "mammal", CategoryId = 1 },
                new { Name = "Amphibians", CategoryId = 2 },
                new { Name = "Arthropoda", CategoryId = 3 },
                new { Name = "Aves", CategoryId = 4 }
            );
        }

        public Animal GetAnimal(int id)
        {
            var animal = Animals.SingleOrDefault(animal => animal.AnimalId == id);
            return animal;
        }
        public Category GetAnimalCategory(int id)
        {
            var Category = categories.SingleOrDefault(Category => Category.CategoryId == id);
            return Category;
        }

        public IEnumerable<Animal> GetAnimalsByCategory(int id)
        {
            var animalofcategory = categories.Where(a => a.CategoryId.Equals(id)).SelectMany(b => b.Animals).ToList().AsEnumerable();
            return animalofcategory;
        }
        public IEnumerable<Comments> GetCommentsByAnimalId(int id)
        {
            var listOfComments = Animals.Where(a => a.AnimalId.Equals(id)).SelectMany(b => b.Comments).ToList().AsEnumerable();
            return listOfComments;
        }

        public IEnumerable<Animal> GetTwoMostCommented()
        {
            var besttwo = Animals.OrderByDescending(a => a.Comments.Count()).Take(2).ToList().AsEnumerable();

            return besttwo;
        }
    }
}


//List<Animal> a = new List<Animal>();
//foreach (var animal in _context.Animals)
//{
//    foreach (var item in tmp)
//    {
//        if (animal.AnimalId == item.Name)
//        {
//            a.Add(new Animal() { Name = animal.Name, CommentCounter = item.Count, Description = animal.Description, ImageSource = animal.ImageSource });
//        }
//    }
//}
