using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using LabObjects.DotNetGlue.SharedKernel;

namespace LabObjects.DotNetGlue.Xml
{
    /// <summary>
    /// XmlDoc Class - Wrapper class for XmlDocument. Provides support for XML Schema Validation from 
    /// scripting languages that interface with the .Net Framework but don't support events.
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/aa468565.aspx</remarks>
    public class XmlDoc : DotNetGlueBase, IDisposable
    {
        #region private fields
        //private bool _isDisposed = false;
        private System.Xml.XmlDocument _xml = new XmlDocument();
        private XmlReaderSettings _readerSettings = new XmlReaderSettings();
        //private System.Xml.Schema.XmlSchemaSet _schemas;        
        private StringBuilder _validationMsgs = new StringBuilder();
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public XmlDoc()
        {

        }

        //public XmlDoc(string xmlFragmentOrFileName)
        //{

        //}
        //public XmlDoc( string xmlFragmentOrFileName,  string [] schemaRefs )
        //{

        //}
        #endregion

        #region Public Properties
        /// <summary>
        /// The xml document as a string
        /// </summary>
        public string XmlString
        {
            get { return _xml.DocumentElement != null ? _xml.DocumentElement.InnerXml : ""; }
        }



        #endregion

        #region Public Methods
        /// <summary>
        /// Load XML from file name method
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            return LoadXmlFile(fileName);
        }
        /// <summary>
        /// Save XML File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Save(string fileName)
        {
            return SaveToFile(fileName);
        }
        /// <summary>
        /// overload method for Save method
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="preserveWhitespace"></param>
        /// <returns></returns>
        public bool Save(string fileName, bool preserveWhitespace)
        {
            _xml.PreserveWhitespace = true;
            return SaveToFile(fileName);
        }
        /// <summary>
        /// The Xml.XmlDocument object
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDocument()
        {
            return _xml;
        }

#if DEBUG
        /// <summary>
        /// Method to validate the current xnl document against one or more schemas
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool status = false;
            try
            {
                // TODO replace with dotnetglue exception
                if (_xml.Schemas.Count == 0)
                    throw new Exception("No Schemas Defined");

                if (_validationMsgs.Length > 0)
                    _validationMsgs.Clear();
                ValidationEventHandler eventHandler = new ValidationEventHandler(DocValidationHandler);
                _xml.Validate(eventHandler);
                if (_validationMsgs.Length == 0)
                    status = true;
                else
                    SetLastError(_validationMsgs.ToString());
            }
            catch (XmlSchemaValidationException ex)
            {
                SetLastError(string.Format("Error Validating XML: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                SetLastError(string.Format("Error Validating XML: {0}", ex.Message));
            }
            return status;
        }
        /// <summary>
        /// Method to add a Schema (XSD) document to the schema collection for the designated target namespace
        /// </summary>
        /// <param name="targetNamespace">Schema target namespace</param>
        /// <param name="schemaUri">URI to the schema file (e.g., http://, c:\FilePath\schemafile.xsd)</param>
        /// <returns></returns>
        public bool AddSchema(string targetNamespace, string schemaUri)
        {
            bool status = false;
            try
            {
                _readerSettings.Schemas.Add(targetNamespace, schemaUri);
                //_xml.Schemas.Add(targetNamespace, schemaUri);
                status = true;
            }
            catch (Exception ex)
            {
                SetLastError(ex.Message);
            }
            return status;
        }
        /// <summary>
        /// Method to remove a schema specified by a target namespace from the schemas collection
        /// </summary>
        /// <param name="targetNamespace"></param>
        /// <returns></returns>
        public bool RemoveSchema(string targetNamespace)
        {
            bool status = false;
            try
            {
                XmlSchemaSet schemas = _readerSettings.Schemas;
                if (_xml.Schemas.Contains(targetNamespace))
                {
                    foreach (XmlSchema s in schemas.Schemas(targetNamespace))
                        _readerSettings.Schemas.Remove(s);
                }
                else
                {
                    throw new Exception(string.Format("No Schemas found for Target Namespace: {0}", targetNamespace));
                }

                status = true;
            }
            catch (Exception ex)
            {
                SetLastError(ex.Message);
            }
            return status;
        }

        /// <summary>
        /// XPathToArray
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        /*
       public string[,] XPathToArray(string xpath, string[] nodeNames)
       {
           string[,] nodeArray = null;
           string[] namesArray;
           int numCols;
           try
           {
               numCols = nodeNames.Length;
               XmlNodeList nodeList = _xml.SelectNodes(xpath);
               if (numCols == 0)
                   throw new Exception("Node Names not defined");
               else if (numCols == 1 && nodeNames[0] == "*")
               {
                   // get #nodes
                   numCols = nodeList[0].ChildNodes.Count;
                   namesArray = new string[numCols];
               }
               else
               {
                   namesArray = new string[numCols];
                   nodeNames.CopyTo(namesArray, 0);
               }
               nodeArray = new string[nodeList.Count,nodeNames.Length];
               for (int row=0; row < nodeList.Count; row++)
               {
                   XmlNode node = nodeList[row];
                   for (int col=0; col < numCols; col++)
                   {
                       string nodeName = namesArray[col];
                       nodeArray[row, col] = node.SelectSingleNode(nodeName).Value.ToString();
                   }
               }
           }
           catch (Exception ex)
           {
               nodeArray = new string[1, 1];
               nodeArray[1, 1] = "ERROR";
               SetLastError(ex.Message, ex.InnerException);
           }
           finally
           {

           }
           return nodeArray;
       }
       */
#endif

        #endregion

        #region Private Methods
        /// <summary>
        /// Method to load a XML file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool LoadXmlFile(string fileName)
        {
            bool status = false;
            try
            {
                if (_readerSettings.Schemas.Count > 0)
                {
                    _readerSettings.ValidationType = ValidationType.Schema;

                    _readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                    _readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    _readerSettings.ValidationEventHandler += new ValidationEventHandler(DocValidationHandler);
                }

                XmlReader reader = XmlReader.Create(fileName, _readerSettings);
                _xml.Load(reader);
                status = true;
            }
            catch (XmlException ex)
            {
                SetLastError(string.Format("Unable to Load XML Document: {0} - {1}", fileName, ex.Message));
            }
            catch (System.IO.FileNotFoundException ex)
            {
                SetLastError(string.Format("Unable to Load XML Document: {0} - {1}", fileName, ex.Message));
            }
            catch (Exception ex)
            {
                SetLastError(string.Format("Unable to Load XML Document: {0} - {1}", fileName, ex.Message), ex.InnerException);
            }
            return status;
        }
        /// <summary>
        /// Method to load a Xml Document from a String
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private bool LoadFromString(string xml)
        {

            bool status = false;
            try
            {

                _xml.LoadXml(xml);
                status = true;
            }
            catch (Exception ex)
            {
                SetLastError(string.Format("Unable to Load XML Document: {0}", ex.Message), ex.InnerException);
            }
            return status;
        }
        /// <summary>
        /// Helper method to save a xml file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool SaveToFile(string fileName)
        {
            bool status = false;
            try
            {
                _xml.Save(fileName);
                status = true;
            }
            catch (XmlException ex)
            {
                SetLastError(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                SetLastError(ex.Message, ex.InnerException);
            }
            return status;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Schema validation event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DocValidationHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    _validationMsgs.AppendFormat("Error: {0}\r\n", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    _validationMsgs.AppendFormat("Warning: {0}\r\n", e.Message);
                    break;
                default:
                    _validationMsgs.AppendFormat("Information: {0}\r\n", e.Message);
                    break;
            }
        }
        #endregion

        #region Disposal
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        /// <summary>
        /// Dispose Method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual new void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    if (_xml != null)
                    {
                        _xml = null;

                    }
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// XmlDoc Finalization Method
        /// </summary>
        ~XmlDoc()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
