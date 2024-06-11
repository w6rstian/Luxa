namespace Luxa.ViewModel
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsViewed { get; set; } = false;
    }
}
