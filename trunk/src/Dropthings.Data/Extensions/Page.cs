using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.Data
{
    public partial class Tab
    {
        #region Properties

        public string UserTabName
        {
            get
            {
                return this.Title.Replace(' ', '_');
            }
        }

        public string LockedTabName
        {
            get
            {
                return this.Title.Replace(' ', '_') + "_Locked";
            }
        }

        #endregion Properties

        #region Methods

        public static int[] GetColumnWidths(int layoutType)
        {
            int[] columnWidths;
            if (layoutType == 2)
                columnWidths = new int[] { 25, 75 };
            else if (layoutType == 3)
                columnWidths = new int[] { 75, 25 };
            else if (layoutType == 4)
                columnWidths = new int[] { 100 };
            else
                columnWidths = new int[] { 33, 33, 33 };

            return columnWidths;
        }


        #endregion Methods
    }
}
