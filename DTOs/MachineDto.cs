namespace washit.dtos
{
    public class MachineDto
{
    public int Id { get; set; }
    public string MachineName { get; set; }
    public int WashTypeId { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; }
    public int? ReservationId { get; set; } // needed for cancel
}

}