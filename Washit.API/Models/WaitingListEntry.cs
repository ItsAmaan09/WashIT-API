namespace washit.models
{
    public class WaitingListEntry
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int WashTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Notified { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

}