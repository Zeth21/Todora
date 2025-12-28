namespace API.RequestDTO_s.RepositoryController
{
    public class CreateRepositoryControllerDTO
    {
        public IFormFile? RepositoryPhoto { get; set; }
        public string RepositoryTitle { get; set; } = null!;
        public string RepositoryDescription { get; set; } = null!;
    }
}
