using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

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
