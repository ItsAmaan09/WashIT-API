namespace washit.models
{
    public class Machine
    {
        public int Id {get; set;}
        public string MachineName { get; set; }
        public int WashTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}