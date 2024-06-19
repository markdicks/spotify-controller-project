# Spotify Controller Application

Welcome to the Spotify Controller Application! This project is designed to provide a centralized control panel for managing your Spotify playback, viewing stream information, and interacting with your audience. This readme will guide you through the setup and usage of the application, as well as our contributing guidelines.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Setup](#setup)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction
The Spotify Controller Application is built using WPF in C#. It provides a user-friendly interface to control your Spotify playback, monitor stream health, view the number of viewers, interact with chat, and manage redeems—all in one centralized location.

## Features
- **Spotify Playback Control**: Play, pause, skip tracks, and adjust volume.
- **Stream Information**: View basic info such as the number of viewers and advanced info like stream health.
- **Chat and Redeems**: Integrated tabs for viewing chat and managing redeems.

## Technologies Used
- **WPF (Windows Presentation Foundation)**: For creating the desktop application.
- **C#**: The programming language used for the application logic.
- **Spotify API**: To control Spotify playback.
- **GitHub**: For version control and collaboration.

## Setup
1. **Clone the Repository**:
    ```bash
    git clone https://github.com/your-username/spotify-controller-app.git
    cd spotify-controller-app
    ```

2. **Install Dependencies**:
    - Ensure you have .NET Framework installed.
    - Open the solution file (`spotify-controller-project.sln`) in Visual Studio.

3. **Configure Spotify API**:
    - Register your application on the [Spotify Developer Dashboard](https://developer.spotify.com/dashboard/applications).
    - Add your Client ID and Client Secret to the application settings.

4. **Build and Run**:
    - Build the project in Visual Studio.
    - Run the application.

## Usage
1. **Spotify Playback Control**:
    - Use the playback controls to play, pause, and skip tracks.
    - Adjust the volume using the volume slider.

2. **Stream Information**:
    - Basic info like viewer count is displayed on the main interface.
    - Advanced info like stream health can be accessed through the "Advanced" tab.

3. **Chat and Redeems**:
    - View and interact with chat in the "Chat" tab.
    - Manage redeems in the "Redeems" tab.

## Contributing
We welcome contributions! Please follow these guidelines:

1. **Fork the Repository**: Click the "Fork" button at the top right of the repository page.
2. **Clone Your Fork**:
    ```bash
    git clone https://github.com/markdicks/spotify-controller-project.git
    cd spotify-controller-project
    ```
3. **Create a Branch**:
    ```bash
    git checkout -b feature-branch
    ```
4. **Make Your Changes**: Implement your changes or new features.
5. **Commit Your Changes**:
    ```bash
    git add .
    git commit -m "Description of changes"
    ```
6. **Push to Your Fork**:
    ```bash
    git push origin feature-branch
    ```
7. **Create a Pull Request**: Go to the original repository and click "New Pull Request".

### Committing Rules
- Write clear, concise commit messages.
- Follow the format: `type(scope): subject`, e.g., `feat(player): add shuffle button`.

### Forking Rules
- Always keep your fork up-to-date with the upstream repository.
- Rebase your branch before submitting a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
