using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LabObjects.DotNetGlue.Win32
{
    /// <summary>
    ///  MessageBox Class
    /// </summary>
    public class MessageBoxGlue
    {
        /// <summary>
        /// MessageBox Show
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="typeMask"></param>
        /// <returns></returns>
        public static int Show( String text, String caption, int typeMask )
        {

            return Win32MessageBox();
        }


        private static int Win32MessageBox()
        {
            return 0;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr MessageBox(int hWnd, String text,  String caption, uint type);
    }

}

