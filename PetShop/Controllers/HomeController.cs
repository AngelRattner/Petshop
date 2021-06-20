using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Controllers
{
    public class HomeController : Controller
    {
        private AnimalContext _AnimalContext;
        private readonly IHostingEnvironment HE;
        public HomeController(AnimalContext AnimalContext, IHostingEnvironment HE)
        {
            _AnimalContext = AnimalContext;
            this.HE = HE;
        }




        public IActionResult Index() //home page work
        {
            return View(_AnimalContext.GetTwoMostCommented());
        }


        #region CATALOG ACTIONS

        public IActionResult Catalog() //work
        {
            ViewBag.GetCategories = _AnimalContext.categories;
            var animals = _AnimalContext.Animals;
            return View(animals);
        }

        public IActionResult CatalogBycategory(int id) //work
        {
            ViewBag.GetCategories = _AnimalContext.categories;
            var animals = _AnimalContext.GetAnimalsByCategory(id);
            return View("Catalog", animals);
        }

        public IActionResult AnimalData(int id) //work
        {
            ViewBag.comments = _AnimalContext.GetCommentsByAnimalId(id);
            var a = _AnimalContext.GetAnimal(id);
            return View(a);
        }
        public IActionResult AddComment(int id, string com)//work
        {
            if (com != null)
            {
                Comments comments = new Comments();
                {
                    comments.AnimalId = id;
                    comments.Comment = com;
                };
                _AnimalContext.Add(comments);
                _AnimalContext.SaveChanges();
                return RedirectToAction("AnimalData", new { id = id });
            }
            else
            {
                TempData["ProcessMessage"] = "Please wirte your comment befoer you try to add it!";
                TempData["displayModal"] = "myModal";
                return RedirectToAction("AnimalData", new { id = id });
            }
        }
        #endregion



        #region ADMIN ACTIONS
        #region administrator page
        public IActionResult administrator() //work
        {
            ViewBag.GetCategories = _AnimalContext.categories;
            return View(_AnimalContext.Animals.ToList());
        }
        public IActionResult CatalogBycategoryAdmin(int id) //work
        {
            ViewBag.GetCategories = _AnimalContext.categories;
            var animals = _AnimalContext.GetAnimalsByCategory(id);
            return View("Catalog", animals);
        }

        #endregion
        public IActionResult Delete(int Id)
        {
            var a = _AnimalContext.Animals.SingleOrDefault(ab => ab.AnimalId == Id);
            _AnimalContext.Animals.Remove(a);
            _AnimalContext.SaveChanges();
            return RedirectToAction("administrator");
        } //work

        #region EDIT
        [HttpGet]
        public IActionResult Edit(int Id)  //work (category/photo) 
        {
            var a = _AnimalContext.Animals.SingleOrDefault(ab => ab.AnimalId == Id);
            CreateAnimal createAnimal = new CreateAnimal();
            {
                createAnimal.Name = a.Name;
                createAnimal.Age = a.Age;
                createAnimal.Description = a.Description;
                createAnimal.CategoryId = a.CategoryId;
            }
            return View(createAnimal);
        }
        [HttpPost]
        public IActionResult Edit(CreateAnimal animal, int id)
        {           
            Animal tmp = _AnimalContext.Animals.First(a => a.AnimalId == id);
            if (animal.Picture !=null && animal.Picture.FileName != tmp.PictureName)
            {
                string unique = null;
                if (animal.Picture != null)
                {
                    string uploads = Path.Combine(HE.WebRootPath, "image");
                    unique = Guid.NewGuid().ToString() + "_" + animal.Picture.FileName;
                    string FilePath = Path.Combine(uploads, unique);
                    animal.Picture.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
                tmp.PictureName = unique;
            }



            if (animal.Name != null) tmp.Name = animal.Name;
            if (animal.Age > 0) tmp.Age = animal.Age;
            if (animal.Description != null) tmp.Description = animal.Description;

            _AnimalContext.Animals.Update(tmp);
            _AnimalContext.SaveChanges();

            return RedirectToAction("administrator");

        }
        #endregion
        #region ADD ANIMAL
        [HttpGet]
        public IActionResult Add()//(category ID)
        {
            ViewBag.GetCategories = _AnimalContext.categories;
            return View();
        }
        [HttpPost]
        public IActionResult Add(CreateAnimal animal)
        {

            if (animal.Name == null ||  animal.Picture == null ||animal.Age<1 ||animal.Description==null)
            {
                TempData["ProcessMessage"] = "Please Fill All The Fields Correctly.";
                TempData["displayModal"] = "myModal";
                return RedirectToAction("Add");
            }


            if (ModelState.IsValid)
            {
                string unique = null;
                if (animal.Picture != null)
                {
                    string uploads = Path.Combine(HE.WebRootPath, "image");
                    unique = Guid.NewGuid().ToString() + "_" + animal.Picture.FileName;
                    string FilePath = Path.Combine(uploads, unique);
                    animal.Picture.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
                Animal newAnimal = new Animal();
                {
                  
                    newAnimal.Name = animal.Name;
                    newAnimal.Age = animal.Age;
                    newAnimal.Description = animal.Description;
                    newAnimal.CategoryId = animal.CategoryId;
                    newAnimal.PictureName = unique;

                };
                _AnimalContext.Add(newAnimal);
                _AnimalContext.SaveChanges();
            }

            return RedirectToAction("administrator");
        }//work (category ID)
        #endregion
        #endregion
    }
}

