namespace Dropthings.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class DebugStreamWriter : StreamWriter
    {
        #region Constructors

        public DebugStreamWriter()
            : base(new MemoryStream())
        {
        }

        #endregion Constructors

        #region Methods

        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        #endregion Methods
    }
}