using System;
using System.Windows.Controls;
using PdfiumViewer;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace VS_PDF_Viewer.UserControls
{
    /// <summary>
    /// WPF user control that provides PDF viewing functionality within Visual Studio editor panes.
    /// </summary>
    public partial class PDFViewerControl : UserControl
    {
        private PdfViewer pdfViewer;
        private static bool pdfiumInitialized = false;
        private PdfDocument currentDocument;

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

            // Subscribe to page display events
            pdfViewer.Renderer.DisplayRectangleChanged += Renderer_DisplayRectangleChanged;
            pdfViewer.Renderer.ZoomChanged += Renderer_ZoomChanged;

            this.windowsFormsHost.Child = pdfViewer;

            // Initialize navigation controls
            UpdateNavigationControls();
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
                    currentDocument = PdfDocument.Load(filePath);
                    pdfViewer.Document = currentDocument;

                    // Update navigation controls after loading
                    UpdateNavigationControls();
                    UpdatePageDisplay();
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

                    // Disable navigation controls on error
                    DisableNavigationControls();
                }
            }
        }

        /// <summary>
        /// Updates the navigation controls based on the current document state.
        /// </summary>
        private void UpdateNavigationControls()
        {
            if (currentDocument != null && currentDocument.PageCount > 0)
            {
                // Enable navigation controls
                btnFirstPage.IsEnabled = true;
                btnPreviousPage.IsEnabled = true;
                btnNextPage.IsEnabled = true;
                btnLastPage.IsEnabled = true;
                txtCurrentPage.IsEnabled = true;
                btnZoomIn.IsEnabled = true;
                btnZoomOut.IsEnabled = true;
                btnFitToWidth.IsEnabled = true;
                btnFitToPage.IsEnabled = true;

                // Update page count label
                lblPageCount.Content = $"/ {currentDocument.PageCount}";
            }
            else
            {
                DisableNavigationControls();
            }
        }

        /// <summary>
        /// Disables all navigation controls.
        /// </summary>
        private void DisableNavigationControls()
        {
            btnFirstPage.IsEnabled = false;
            btnPreviousPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;
            btnLastPage.IsEnabled = false;
            txtCurrentPage.IsEnabled = false;
            btnZoomIn.IsEnabled = false;
            btnZoomOut.IsEnabled = false;
            btnFitToWidth.IsEnabled = false;
            btnFitToPage.IsEnabled = false;
            txtCurrentPage.Text = "0";
            lblPageCount.Content = "/ 0";
            lblZoomLevel.Content = "100%";
        }

        /// <summary>
        /// Updates the current page display in the navigation bar.
        /// </summary>
        private void UpdatePageDisplay()
        {
            if (pdfViewer != null && currentDocument != null)
            {
                int currentPage = pdfViewer.Renderer.Page + 1; // PdfViewer uses 0-based index
                txtCurrentPage.Text = currentPage.ToString();

                // Update button states
                btnFirstPage.IsEnabled = currentPage > 1;
                btnPreviousPage.IsEnabled = currentPage > 1;
                btnNextPage.IsEnabled = currentPage < currentDocument.PageCount;
                btnLastPage.IsEnabled = currentPage < currentDocument.PageCount;
            }
        }

        /// <summary>
        /// Updates the zoom level display.
        /// </summary>
        private void UpdateZoomDisplay()
        {
            if (pdfViewer != null)
            {
                int zoomPercent = (int)Math.Round(pdfViewer.Renderer.Zoom * 100);
                lblZoomLevel.Content = $"{zoomPercent}%";
            }
        }

        // Event Handlers for Navigation Buttons

        private void BtnFirstPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null && currentDocument != null)
            {
                pdfViewer.Renderer.Page = 0;
                UpdatePageDisplay();
            }
        }

        private void BtnPreviousPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null && currentDocument != null && pdfViewer.Renderer.Page > 0)
            {
                pdfViewer.Renderer.Page--;
                UpdatePageDisplay();
            }
        }

        private void BtnNextPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null && currentDocument != null && pdfViewer.Renderer.Page < currentDocument.PageCount - 1)
            {
                pdfViewer.Renderer.Page++;
                UpdatePageDisplay();
            }
        }

        private void BtnLastPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null && currentDocument != null)
            {
                pdfViewer.Renderer.Page = currentDocument.PageCount - 1;
                UpdatePageDisplay();
            }
        }

        private void BtnZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null)
            {
                pdfViewer.Renderer.Zoom = Math.Min(pdfViewer.Renderer.Zoom * 1.2, 5.0); // Max 500% zoom
                UpdateZoomDisplay();
            }
        }

        private void BtnZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null)
            {
                pdfViewer.Renderer.Zoom = Math.Max(pdfViewer.Renderer.Zoom / 1.2, 0.1); // Min 10% zoom
                UpdateZoomDisplay();
            }
        }

        private void BtnFitToWidth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null)
            {
                pdfViewer.ZoomMode = PdfViewerZoomMode.FitWidth;
                UpdateZoomDisplay();
            }
        }

        private void BtnFitToPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (pdfViewer != null)
            {
                pdfViewer.ZoomMode = PdfViewerZoomMode.FitHeight;
                UpdateZoomDisplay();
            }
        }

        // Text Box Event Handlers

        private void TxtCurrentPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow numeric input
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TxtCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigateToPage();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Only digits allowed
            return !regex.IsMatch(text);
        }

        private void NavigateToPage()
        {
            if (pdfViewer != null && currentDocument != null && int.TryParse(txtCurrentPage.Text, out int pageNumber))
            {
                // Ensure page number is within valid range
                pageNumber = Math.Max(1, Math.Min(pageNumber, currentDocument.PageCount));

                // Navigate to page (convert to 0-based index)
                pdfViewer.Renderer.Page = pageNumber - 1;
                UpdatePageDisplay();
            }
            else
            {
                // Reset to current page if invalid input
                UpdatePageDisplay();
            }
        }

        // PdfViewer Event Handlers

        private void Renderer_DisplayRectangleChanged(object sender, EventArgs e)
        {
            // Update page display when view changes
            UpdatePageDisplay();
        }

        private void Renderer_ZoomChanged(object sender, EventArgs e)
        {
            // Update zoom display when zoom changes
            UpdateZoomDisplay();
        }
    }
}