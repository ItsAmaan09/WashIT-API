namespace washit.models
{
    public class WaitingListEntry
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int WashTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Notified { get; set; }
    }

}