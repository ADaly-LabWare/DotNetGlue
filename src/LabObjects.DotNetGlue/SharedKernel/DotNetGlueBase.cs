using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LabObjects.DotNetGlue.SharedKernel
{
    /// <summary>
    /// <para>Base class for DotNetGlue classes.</para>
    /// </summary>
    /// <remarks>
    /// <para>This class provides support for sub classes error reporting to host applications through the properties: LastError and LastErrorDetail.</para>
    /// </remarks>
    /// <loApi lo_access="public" lo_type="class"></loApi>
    public class DotNetGlueBase
    {

        #region Private fields
        private StringBuilder _lastError = new StringBuilder("N");
        private StringBuilder _lastErrorDetail = new StringBuilder("");
        private bool _isDisposed = false;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>DotNetGlueBase Constructor</para>
        /// </summary>
        /// <loApi lo_access="internal" lo_constructor="true"></loApi>
        internal DotNetGlueBase() { }
        #endregion

        #region Public Properties
        /// <summary>
        /// <para>Read only public property that contains the last error message trapped during library operations.</para>
        /// </summary>
        /// <value>Contains the last error message trapped by the object.</value>
        /// <loApi lo_access="public" lo_datatype="string" lo_readonly="true"></loApi>
        /// <example>
        /// <para>Read the <c>LastError</c> after a .Net Glue object method returns Boolean value of False.</para>
        /// <code>
        /// // read LastError property after a methods returned false
        /// status = dotNetGlueObject.MethodName()
        /// if (Not( status ) ) then
        ///     errMessage = DotNetGlueObject.LastError
        /// endif
        /// </code>
        /// </example>
        public string LastError
        {
            get { return _lastError.ToString(); ; }
        }

        /// <summary>
        /// <para>Read only public property that contains additional details of the last error trapped during library operations (if available).</para>
        /// </summary>
        /// <value>Contains the details associated with the LastError trapped by the object.</value>
        /// <loApi lo_access="public" lo_datatype="string" lo_readonly="true"></loApi>
        /// <example>
        /// <para>Read the <c>LastErrorDetail</c> after a .Net Glue object method returns Boolean value of False.</para>
        /// <code>
        /// // read LastError property after a methods returned false
        /// var status, errMessage, errMessageDetail
        /// status = dotNetGlueObject.MethodName()
        /// if (Not( status ) ) then
        ///     errMessage = DotNetGlueObject.LastError
        ///     errMessageDetail = DotNetGlueObject.LastErrorDetail
        /// endif
        /// </code>
        /// </example>
        public string LastErrorDetail
        {
            get { return _lastErrorDetail.ToString(); }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// <para>Sets the LastError property. Used by classes that inherit from DotNetGlueBase when exceptions are caught or from other trapped error conditions.</para>
        /// </summary>
        /// <param name="errMsg" lo_datatype="string">The message to set as the LastError property.</param>
        /// <returns lo_datatype="void"></returns>
        /// <loApi lo_access="protected"></loApi>
        protected void SetLastError(string errMsg)
        {
            _lastError.Clear();
            _lastError.AppendFormat($"{errMsg}");
        }
        /// <summary>
        /// <para>Protected overload method to set the LastError and LastErrorDetail property.</para>
        /// </summary>
        /// <returns lo_datatype="void"></returns>
        /// <param name="errMsg" lo_datatype="string">The message to set as the LastError property.</param>
        /// <param name="errDetails" lo_datatype="string">The message (string) containing the details to set as the LastErrorDetail property.</param>
        /// <loApi lo_access="protected"></loApi>
        protected void SetLastError(string errMsg, string errDetails)
        {
            _lastError.Clear();
            _lastErrorDetail.Clear();
            _lastError.AppendFormat($"{errMsg}");
            if (errDetails.Length > 0)
                _lastErrorDetail.AppendFormat($"{errDetails}");
        }
        /// <summary>
        /// <para>Protected ovoverloaderride method to set the LastError and LastErrorDetail property.</para>
        /// </summary>
        /// <returns lo_datatype="void"></returns>
        /// <param name="errMsg" lo_datatype="string">The message to set as the LastError property.</param>
        /// <param name="innerException" lo_datatype="Exception">The exception that will be use to set the LastErrorDetail property.</param>
        /// <loApi lo_access="protected"></loApi>
        protected void SetLastError(string errMsg, Exception innerException)
        {

            _lastError.Clear();
            _lastErrorDetail.Clear();

            _lastError.AppendFormat($"{errMsg}");
            if (innerException != null)
                _lastErrorDetail.AppendFormat($"{innerException.Message}{Environment.NewLine}{innerException.StackTrace}");
        }
        /// <summary>
        /// <para>Protected method to reset the last error properties.</para>
        /// </summary>
        /// <returns lo_datatype="void"></returns>
        /// <loApi lo_access="protected"></loApi>
        protected void ResetLastError()
        {
            _lastError.Clear();
            _lastErrorDetail.Clear();
            _lastError.AppendFormat("");

        }
        #endregion


        #region Dispose Methods
        /// <summary>
        /// <para>Protected ready only property.</para>
        /// </summary>
        /// <returns lo_datatype="bool">True is object was disposed.</returns>
        /// <loApi lo_access="protected"></loApi>
        protected bool IsDisposed
        {
            get { return _isDisposed; }
        }
        /// <summary>
        /// <para>Dispose virtual method.</para>
        /// </summary>
        /// <returns lo_datatype="void"></returns>
        /// <loApi lo_access="public" lo_qualifier="virtual"></loApi>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <para>Dispose overload method.</para>
        /// </summary>
        /// <returns>void</returns>
        /// <param name="disposing" lo_datatype="bool">Indicates if the object is currrently disposing.</param>
        /// <loApi lo_access="protected"></loApi>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // free resources if needed
                    _lastError.Clear();
                    _lastErrorDetail.Clear();
                }
            }
            _isDisposed = true;
        }
        #endregion
    }
}
