public static class ConsoleUtil
{
    public static void PrintDots(int threadSleep, int nDots)
    {
        for (int i = 0; i < nDots; i++)
        {
            Console.Write(".");
            Thread.Sleep(threadSleep);
        }
    }
}