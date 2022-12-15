namespace SystemMonitor.Core.Domain
{
    public class NetworkSpecs
    {
        public int Id { get; set; }
        public string AdapterName { get; set; }
        public double Bandwidth { get; set; }

        public int SystemSpecsId { get; set; }
        public SystemSpecs SystemSpecs { get; set; }
    }
}
