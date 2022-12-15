namespace SystemMonitor.Core.Domain
{
    public class DiskSpecs
    {
        public int Id { get; set; }
        public string DiskName { get; set; }
        public double DiskSize { get; set; }

        public int SystemSpecsId { get; set; }
        public SystemSpecs SystemSpecs { get; set; }
    }
}
