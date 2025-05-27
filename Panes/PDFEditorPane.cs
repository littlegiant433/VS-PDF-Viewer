using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;

namespace VS_PDF_Viewer.EditorFactories
{
    /// <summary>
    /// PDF editor pane that displays PDF documents within Visual Studio using a WPF control.
    /// </summary>
    [Guid("76F4CBE3-0BA9-4E39-AAC6-553C1EC21873")]
    public sealed class PDFEditorPane : WindowPane, IVsPersistDocData
    {
        /// <summary>
        /// The file name or path of the currently loaded PDF document.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Indicates whether the document has unsaved changes.
        /// </summary>
        private bool isDirty = false;

        /// <summary>
        /// Initializes a new PDF editor pane with the specified file name.
        /// </summary>
        /// <param name="fileName">The path to the PDF file to display.</param>
        public PDFEditorPane(string fileName) : base(null)
        {
            this.fileName = fileName;
            // Hier wird Ihr WPF Control geladen
            this.Content = new VS_PDF_Viewer.UserControls.PDFViewerControl();
        }

        /// <summary>
        /// Gets the GUID that identifies this editor type.
        /// </summary>
        /// <param name="pClassID">The returned editor type GUID.</param>
        /// <returns>S_OK indicating successful retrieval.</returns>
        public int GetGuidEditorType(out Guid pClassID)
        {
            pClassID = new Guid("255CDD48-13FC-45F1-A005-8F1B691889A6");
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines whether the document data has unsaved changes.
        /// </summary>
        /// <param name="pfDirty">1 if document is dirty, 0 if clean.</param>
        /// <returns>S_OK indicating successful check.</returns>
        public int IsDocDataDirty(out int pfDirty)
        {
            pfDirty = isDirty ? 1 : 0;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Sets the path for an untitled document.
        /// </summary>
        /// <param name="pszDocDataPath">The path to set for the untitled document.</param>
        /// <returns>S_OK indicating successful operation.</returns>
        public int SetUntitledDocPath(string pszDocDataPath)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Loads document data from the specified file path.
        /// </summary>
        /// <param name="pszMkDocument">The moniker or path of the document to load.</param>
        /// <returns>S_OK indicating successful loading.</returns>
        public int LoadDocData(string pszMkDocument)
        {
            fileName = pszMkDocument;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Saves the document data with the specified save flags.
        /// </summary>
        /// <param name="dwSave">The save flags from VSSAVEFLAGS enumeration.</param>
        /// <param name="pbstrMkDocumentNew">The new document moniker if renamed during save.</param>
        /// <param name="pfSaveCanceled">1 if save was canceled, 0 if completed.</param>
        /// <returns>S_OK indicating successful save operation.</returns>
        public int SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
        {
            pbstrMkDocumentNew = null;
            pfSaveCanceled = 0;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Closes the PDF editor pane and performs cleanup.
        /// </summary>
        /// <returns>S_OK indicating successful closure.</returns>
        public int Close()
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when the document data is registered with the running document table.
        /// </summary>
        /// <param name="docCookie">The document cookie assigned by the running document table.</param>
        /// <param name="pHierNew">The hierarchy that owns the document.</param>
        /// <param name="itemidNew">The item ID of the document in the hierarchy.</param>
        /// <returns>S_OK indicating successful registration.</returns>
        public int OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Handles renaming of the document data.
        /// </summary>
        /// <param name="grfAttribs">The rename attributes.</param>
        /// <param name="pHierNew">The new hierarchy that will own the document.</param>
        /// <param name="itemidNew">The new item ID in the hierarchy.</param>
        /// <param name="pszMkDocumentNew">The new document moniker.</param>
        /// <returns>S_OK indicating successful rename operation.</returns>
        public int RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines whether the document data can be reloaded.
        /// </summary>
        /// <param name="pfReloadable">1 if document is reloadable, 0 if not.</param>
        /// <returns>S_OK indicating successful check.</returns>
        public int IsDocDataReloadable(out int pfReloadable)
        {
            pfReloadable = 1;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Reloads the document data with the specified flags.
        /// </summary>
        /// <param name="grfFlags">The reload flags.</param>
        /// <returns>S_OK indicating successful reload.</returns>
        public int ReloadDocData(uint grfFlags)
        {
            return VSConstants.S_OK;
        }
    }
}
