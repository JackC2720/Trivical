using Microsoft.AspNetCore.Mvc;

namespace Trivical.Core.Components
{
	public class QuestionsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<string> products = new List<string>() {
                "Product 1", "Product 2", "Product 3", "Product 4", "Product 5"
            };

            return View(products);
        }
    }
}
