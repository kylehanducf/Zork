using System;
using System.Diagnostics;

namespace Zork.Common
{
    public static class Assert
    {
        [Conditional("DEBUG")]
        public static void IsTrue(bool expression, string message = null)
        {
            if (expression == false)
            {
                throw new Exception(message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsFalse(bool expression, string message = null)
        {
            if (expression)
            {
                throw new Exception(message);
            }
        }
    }
}
