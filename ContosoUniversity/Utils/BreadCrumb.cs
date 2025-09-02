namespace ContosoUniversity.Utils
{
    public class BreadCrumb
    {
        public string Name { get; set; }
        public string LinkTo { get; set; }
        public bool IsCurrent { get; set; } = false;
    }
}
