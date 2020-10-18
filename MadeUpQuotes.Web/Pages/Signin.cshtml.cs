using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MadeUpQuotes.Web.Pages
{
    public class SigninModel : PageModel
    {
        public async Task OnGetAsync(string redirectUri = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = redirectUri
            });
        }
    }
}
