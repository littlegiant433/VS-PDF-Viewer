# PDF Viewer for Visual Studio

View PDF files directly in Visual Studio without leaving your IDE.

## Features

- Open PDF files in the Visual Studio editor
- Zoom in/out with mouse wheel
- Navigate through pages
- No external PDF reader required

## Installation

### From Visual Studio Marketplace
1. Open Visual Studio
2. Go to Extensions > Manage Extensions
3. Search for "PDF Viewer for Visual Studio"
4. Click Download and Install

### From VSIX file
1. Download the .vsix file from the [Releases](https://github.com/littlegiant433/VS-PDF-Viewer/releases) page
2. Double-click the .vsix file to install

## Usage

Simply open any PDF file in Visual Studio:
- Right-click on a PDF file in Solution Explorer
- Select "Open With..." > "PDF Viewer"
- Or just double-click the PDF file after setting PDF Viewer as default

## Requirements

- Visual Studio 2022 or later
- .NET Framework 4.7.2 or later

## Building from Source

1. Clone this repository
2. Open `VS_PDF_Viewer.sln` in Visual Studio
3. Restore NuGet packages
4. Build in Release mode
5. The VSIX will be in `bin\Release\`

## License

MIT License - see LICENSE.txt for details