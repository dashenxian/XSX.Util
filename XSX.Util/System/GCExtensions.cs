using System.Runtime;

namespace System
{
    public static class GCExtensions
    {
        public static void CollectLarge()
        {
            GCSettings.LargeObjectHeapCompactionMode =
                GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
