using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

/// <summary>
/// Visual Studio package that provides PDF viewing capabilities by registering the PDF editor factory.
/// </summary>
[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[ProvideEditorFactory(typeof(PDFEditorFactory), 110)]
[ProvideEditorExtension(typeof(PDFEditorFactory), ".pdf", 50)]
[ProvideEditorLogicalView(typeof(PDFEditorFactory), "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}")]
[Guid("D9B436BE-2898-433E-B7B4-C10CFAE9F191")]
public sealed class VS_PDF_ViewerPackage : AsyncPackage
{
    /// <summary>
    /// Initializes the package asynchronously and registers the PDF editor factory with Visual Studio.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <param name="progress">The progress reporter for initialization status.</param>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        RegisterEditorFactory(new PDFEditorFactory());
    }
}
