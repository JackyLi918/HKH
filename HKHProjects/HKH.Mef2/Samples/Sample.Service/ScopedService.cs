namespace Sample.Service
{
    public interface IScopedService
    {
        int CreateCounter { get; }
    }
    public class ScopedService : IScopedService
    {
        private static int counter = 0;
        public ScopedService()
        {
            counter++;
            myCounter = counter;
        }
        private int myCounter = 0;
        public int CreateCounter => myCounter;
    }
}
