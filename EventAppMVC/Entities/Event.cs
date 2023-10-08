namespace EventAppMVC.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public bool IsFree { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
