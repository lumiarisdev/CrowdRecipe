using CrowdRecipe.Website.Models;
using CrowdRecipe.Website.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CrowdRecipe.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RecipesService recipesService;

        public HomeController(ILogger<HomeController> logger, RecipesService recipesService)
        {
            _logger = logger;
            this.recipesService = recipesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search(Recipe recipe)
        {

            var result = await recipesService.GetByTitle(recipe.Title);
            if(result.Count() > 0)
            {
                return RedirectToRoute("Recipes");
            }
            return RedirectToAction("Index");

        }

    }
}