using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using VS_PDF_Viewer.EditorFactories;

/// <summary>
/// The pdf editor factory class that implements the IVsEditorFactory interface.
/// </summary>
[Guid("255CDD48-13FC-45F1-A005-8F1B691889A6")]
[ComVisible(true)]
public sealed class PDFEditorFactory : IVsEditorFactory
{
    /// <summary>
    /// Creates an instance of the editor pane for PDF files.
    /// </summary>
    /// <param name="grfCreateDoc">The creation flags from __VSCREATEEDITORFLAGS enumeration.</param>
    /// <param name="pszMkDocument">The document path or moniker to open.</param>
    /// <param name="pszPhysicalView">The physical view name.</param>
    /// <param name="pvHier">The project hierarchy interface containing the document.</param>
    /// <param name="itemid">The item identifier in the project hierarchy.</param>
    /// <param name="punkDocDataExisting">The existing document data object (if any).</param>
    /// <param name="ppunkDocView">The returned document view interface.</param>
    /// <param name="ppunkDocData">The returned document data interface.</param>
    /// <param name="pbstrEditorCaption">The returned editor window caption.</param>
    /// <param name="pguidCmdUI">The returned command UI context GUID.</param>
    /// <param name="pgrfCDW">The returned document window flags.</param>
    /// <returns>An HRESULT for generic success</returns>
    public int CreateEditorInstance(uint grfCreateDoc, string pszMkDocument, string pszPhysicalView,
        IVsHierarchy pvHier, uint itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView,
        out IntPtr ppunkDocData, out string pbstrEditorCaption, out Guid pguidCmdUI, out int pgrfCDW)
    {
        // Initialize output parameters
        ppunkDocView = IntPtr.Zero;
        ppunkDocData = IntPtr.Zero;
        pbstrEditorCaption = "";
        pguidCmdUI = Guid.Empty;
        pgrfCDW = 0;

        var editorPane = new PDFEditorPane(pszMkDocument);

        // Prep COM objects for VS
        ppunkDocView = Marshal.GetIUnknownForObject(editorPane);
        ppunkDocData = Marshal.GetIUnknownForObject(editorPane);

        return VSConstants.S_OK;
    }

    /// <summary>
    /// Sets the service provider for accessing Visual Studio services.
    /// </summary>
    /// <param name="psp">The service provider interface for accessing VS services.</param>
    /// <returns>S_OK indicating successful initialization.</returns>
    public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
    {
        return VSConstants.S_OK;
    }

    /// <summary>
    /// Closes the editor factory and performs cleanup operations.
    /// </summary>
    /// <returns>S_OK indicating successful closure.</returns>
    public int Close()
    {
        return VSConstants.S_OK;
    }

    /// <summary>
    /// Maps a logical view to a physical view for the PDF editor.
    /// </summary>
    /// <param name="rguidLogicalView">The logical view GUID to map.</param>
    /// <param name="pbstrPhysicalView">The corresponding physical view name (set to null for default view).</param>
    /// <returns>S_OK if the logical view is supported, E_NOTIMPL if not supported.</returns>
    public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
    {
        pbstrPhysicalView = null;

        if (rguidLogicalView == VSConstants.LOGVIEWID_Primary || rguidLogicalView == new Guid("{7651A703-06E5-11D1-8EBD-00A0C90F26EA}"))
        {
            return VSConstants.S_OK;
        }

        return VSConstants.E_NOTIMPL;
    }
}