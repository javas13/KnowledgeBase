namespace StudyTodocups.Models
{
    public class ModuleForList
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? VideosCount { get; set; }
        public int? ArticlesCount { get; set; }
    }
}
