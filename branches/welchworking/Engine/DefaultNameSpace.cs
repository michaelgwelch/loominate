using System;
using System.Collections.Generic;
using System.Text;

namespace Loominate.Engine
{
    internal class DefaultNameSpace : IDisposable
    {
        private static Stack<NameSpace> scopes = new Stack<NameSpace>();

        public static NameSpace Current
        {
            get
            {
                if (scopes.Count == 0) return null;
                return scopes.Peek();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            scopes.Pop();
        }

        #endregion

        public static DefaultNameSpace Set(NameSpace defaultNs)
        {
            scopes.Push(defaultNs);
            return new DefaultNameSpace();
        }

        public static void AssertSet()
        {
            if (scopes.Count == 0) throw new Exception("Default namespace not set.");

        }
    }
}
