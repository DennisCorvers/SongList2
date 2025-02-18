# Song List 2
## Overview

This application helps you manage and organize your music collection. It allows you to import music files from a selected folder, view metadata such as title, artist, and album, and manipulate the data in various ways. The application also supports saving, loading, and exporting data while ensuring that duplicate songs (same artist and album) are excluded automatically.

## Features

- **Import Music Files**: Import music from a selected folder, and the app will recursively search through folders and subfolders to find all supported audio file formats.
- **Music Overview**: Display metadata for each imported file, including the title, artist, and album.
- **Save and Load Data**: Save your imported music data and load it at any time, ensuring that your collection is always available.
- **Remove Entries**: Easily remove entries from the data without deleting the actual file.
- **Export Music Data**: Export your music collection to a new folder structure (artist > album > song), where the original files are copied, not moved. The original files remain in their initial location.
- **Duplicate Song Handling**: Automatically detects and excludes duplicate songs with the same artist and album during the import process.

## Use Cases

This application is perfect for users who want to:

- **Create a Custom Music Listing**: Make sure there are no duplicate songs in your music collection, especially for people who have non-streamed songs and want to create a playlist with unique tracks.
- **Search and Organize Music**: Search through a folder structure for any audio files, view them, and copy them to a centralized location with ease.
- **Organize a Collection of Unorganized Music**: Automatically sort a chaotic music library into a well-structured system based on artist and album.

## Installation

To get started with this application, visit the [Releases](https://github.com/DennisCorvers/SongList2/releases) section of the repository and download the latest release for your platform.

Once downloaded, follow the provided instructions to install and run the application.

## Supported Formats

This application supports the following music file formats:

- `.mp3`
- `.ogg`
- `.flac`
- `.mpc`
- `.spx`
- `.wav`
- `.aiff`
- `.mp4`
- `.ape`
- `.dsf`
- `.dff`
- `.aac`

## Disclaimers

- **Metadata Accuracy**: The application relies on the metadata embedded in the audio files (such as title, artist, album). If the metadata appears incorrect, it is because the data is not set properly within the file itself. This issue has no relation to the app, and it can be resolved by updating the metadata within your music files.
  
- **File Location**: When importing songs, the application also stores the file's location. If a file is moved or deleted before exporting, the application will skip the file during the import process. It is essential that the files remain in their original location for the app to function correctly during import and export.

- **Log Files**: Log files are generated during the process and will provide detailed information regarding any issues, such as skipped files due to incorrect metadata or file location changes. You can check these logs to understand what may have gone wrong during an operation.
