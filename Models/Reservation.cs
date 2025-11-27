namespace washit.models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string UserName { get; set; }
        public int WashTypeId { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public bool CheckedIn { get; set; }

    }
}