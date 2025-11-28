using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabObjects.DotNetGlue.SharedKernel;

namespace LabObjects.DotNetGlue.Diagnostics
{

    /// <summary>
    /// Internal class that provides a first-in, first-out queue and "all data" property to support buffering Process I/O.
    /// </summary>
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process.OutputRead"/>
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process.Output"/>
    /// <loApi lo_access="internal" lo_datatype="class"></loApi>
    internal class ProcessBuffer
    {

        #region private fields
        StringBuilder _buffer = new StringBuilder();
        Queue<string> _dataQueue = new Queue<string>();
        #endregion

        #region public properties
        /// <summary>
        /// Read only Boolean value that indicates whether there is unread data in the buffer.
        /// </summary>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType>System.Boolean</loMemberType>
        /// <loReadOnly>true</loReadOnly>
        public bool HasUnreadData
        {
            get { return (_dataQueue.Count > 0); }
        }

        /// <summary>
        /// Read only string value that contains all of the data in the buffer.
        /// </summary>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType>System.String</loMemberType>
        /// <loReadOnly>true</loReadOnly>
        public string Data
        {
            get { return _buffer.ToString(); }
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Clears the contents of the buffer.
        /// </summary>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType></loMemberType>
        public void Clear()
        {
            _buffer.Clear();
            _dataQueue.Clear();
        }

        /// <summary>
        /// Reads the unread contents of the buffer.
        /// </summary>
        /// <returns>The current contents of the buffer</returns>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType>System.String</loMemberType>
        public string Read()
        {
            if (_dataQueue.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                while (_dataQueue.Count > 0)
                    sb.Append(_dataQueue.Dequeue());
                return sb.ToString();
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Appends a string to the buffer contents.
        /// </summary>
        /// <param name="s">The string data to append to the buffer contents.</param>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType></loMemberType>
        public void Append(string s)
        {
            _dataQueue.Enqueue(s);
            _buffer.Append(s);
        }

        /// <summary>
        /// Appends the string and the current environment new line character conbination to the buffer contents.
        /// </summary>
        /// <param name="s">The string data to append to the buffer contents.</param>
        /// <loAssessor>public</loAssessor>
        /// <loMemberType></loMemberType>
        public void AppendLine(string s)
        {
            _dataQueue.Enqueue($"{s}{Environment.NewLine}");
            _buffer.AppendLine(s);
        }
        #endregion
    }
}
