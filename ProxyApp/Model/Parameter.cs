namespace ProxyApp.Model
{
    public class Parameter
    {
        public string @in { get; set; }
        public string name { get; set; }
        public Schema schema { get; set; }
        public string @default { get; set; }

    }

    public class Schema
    {
        public string type { get; set; }
        public string format { get; set; }
        public string required { get; set; }

    }
}
