using System;
using System.Windows.Controls;
using PdfiumViewer;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace VS_PDF_Viewer.UserControls
{
    /// <summary>
    /// WPF user control that provides PDF viewing functionality within Visual Studio editor panes.
    /// </summary>
    public partial class PDFViewerControl : UserControl
    {
        private PdfViewer pdfViewer;
        private static bool pdfiumInitialized = false;

        /// <summary>
        /// Initializes a new instance of the PDF viewer control.
        /// </summary>
        public PDFViewerControl()
        {
            InitializeComponent();

            // Initialize PDFium if not already done
            if (!pdfiumInitialized)
            {
                InitializePdfium();
                pdfiumInitialized = true;
            }

            // Create the PdfViewer control
            pdfViewer = new PdfViewer();
            this.windowsFormsHost.Child = pdfViewer;
        }

        private void InitializePdfium()
        {
            try
            {
                // Try to find pdfium.dll in the extension directory
                string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string pdfiumPath = Path.Combine(assemblyLocation, "pdfium.dll");

                if (!File.Exists(pdfiumPath))
                {
                    // Try x64 subdirectory
                    pdfiumPath = Path.Combine(assemblyLocation, "x64", "pdfium.dll");
                }

                if (!File.Exists(pdfiumPath))
                {
                    // Try x86 subdirectory
                    pdfiumPath = Path.Combine(assemblyLocation, "x86", "pdfium.dll");
                }

                if (File.Exists(pdfiumPath))
                {
                    // Load the library
                    LoadLibrary(pdfiumPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing PDFium: {ex.Message}");
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Loads a PDF file into the viewer.
        /// </summary>
        /// <param name="filePath">The path to the PDF file to load.</param>
        public void LoadPdf(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var document = PdfDocument.Load(filePath);
                    pdfViewer.Document = document;
                }
                catch (Exception ex)
                {
                    // Handle error - maybe show a message
                    System.Diagnostics.Debug.WriteLine($"Error loading PDF: {ex.Message}");

                    // Show error in the viewer
                    var errorLabel = new System.Windows.Forms.Label
                    {
                        Text = $"Error loading PDF: {ex.Message}",
                        Dock = System.Windows.Forms.DockStyle.Fill,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                    };
                    this.windowsFormsHost.Child = errorLabel;
                }
            }
        }
    }
}