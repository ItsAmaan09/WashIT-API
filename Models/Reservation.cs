namespace washit.models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int? UserId { get; set; }
        public int WashTypeId { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public bool CheckedIn { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

    }
}