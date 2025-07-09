using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class BlogWithAuthorViewModel
    {
        public BlogPostDTO Blog { get; set; }
        public UserDTO? Author { get; set; }
        public List<BlogReportDTO> Reports { get; set; } = new();
    }
}
