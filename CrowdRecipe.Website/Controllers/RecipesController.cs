using CrowdRecipe.Website.Models;
using CrowdRecipe.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CrowdRecipe.Website.Controllers
{

    public class RecipesController : Controller
    {

        private readonly RecipesService recipesService;

        public RecipesController(RecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        [HttpGet]
        public async Task<List<Recipe>> Get() => await recipesService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Recipe>> Get(ObjectId id)
        {
            var recipe = await recipesService.GetAsync(id);
            return recipe is null ? NotFound() : recipe;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Recipe recipe)
        {
            await recipesService.CreateAsync(recipe);
            return CreatedAtAction(nameof(Get), new { id = recipe.Id }, recipe);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(ObjectId id, Recipe updateRecipe)
        {
            var recipe = await recipesService.GetAsync(id);
            if (recipe is null)
            {
                return NotFound();
            }
            updateRecipe.Id = recipe.Id;
            await recipesService.UpdateAsync(id, updateRecipe);
            return NoContent();
        }

        public async Task<IActionResult> Index()
        {
            return View(await Get());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            var result = recipesService.GetAsync(recipe.Id).Result;
            if (result == null)
            {
                recipe.Id = new ObjectId();
                recipe.AuthorName = User.Identity.Name != null ? User.Identity.Name : "";
                if(recipe.AuthorName.Equals("") || !User.Identity.IsAuthenticated)
                {
                    TempData["Message"] = "You are not signed in!";
                    return View("Create", recipe);
                }
                await recipesService.CreateAsync(recipe);
            } else
            {
                TempData["Message"] = "Recipe already exists";
                return View("Create", recipe);
            }
            return RedirectToAction("Index");
        }

        /*     public async Task<IActionResult> Delete(ObjectId recipeId)
             {
                 return View(await Delete(recipeId));
             }*/

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return View(await recipesService.GetAsync(new ObjectId(id)));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            return View(await recipesService.GetAsync(new ObjectId(id)));
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string id, Recipe recipe)
        {
            if(ModelState.IsValid)
            {
                recipe.Id = new ObjectId(id);
                // could also set author id here
                await recipesService.UpdateAsync(recipe.Id, recipe);
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {

            var recipe = await recipesService.GetAsync(new ObjectId(id));

            return View(recipe);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id, Recipe recipe)
        {
            
            await recipesService.RemoveAsync(new ObjectId(id));

            return RedirectToAction("Index");

        }

        [Authorize]
        public async Task<IActionResult> MyRecipes(string username)
        {

            var recipes = await recipesService.GetByAuthorName(username);
            if(recipes.Count() > 0)
            {
                return View(recipes);
            }
            return RedirectToAction("Index");

        }

    }
}
