namespace Sample.Service
{
    public interface ISingleService
    {
        int CreateCounter { get; }
    }
    public class SingleService: ISingleService
    {
        private static int counter = 0;
        public SingleService()
        {
            counter++;
        }
        public int CreateCounter => counter;
    }
}
