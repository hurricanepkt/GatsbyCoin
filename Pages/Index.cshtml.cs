using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Chain _chain;

        public IndexModel(ILogger<IndexModel> logger, Chain chain)
        {
            _logger = logger;
            _chain = chain;
        }
        private long _id = 0;
        public void OnGet(long id)
        {
             _id = id;
        }

        [BindProperty]
        public long Id => _id;
        [BindProperty]
        public Chain Chain  => _chain;
    }
}