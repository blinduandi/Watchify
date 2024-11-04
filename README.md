# Watchify

Watchify is a cross-platform movie recommendation application that delivers personalized movie suggestions. It leverages .NET MAUI for the mobile app development and Go for the backend API. Users can explore trending movies, find recommendations based on their viewing habits, and enjoy an intuitive, swiping interaction for easy movie discovery.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Setup](#setup)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Movie Recommendations**: Receive tailored movie recommendations based on user preferences.
- **Trending Movies**: Discover trending movies updated regularly.
- **Swipe Interaction**: Swipe right to like or left to dismiss movies.
- **Search Functionality**: Look up movies directly by title.
- **Genre and Cast Filtering**: Filter recommendations by genre or cast for a customized experience.
- **Firebase Authentication**: Secure login and registration.

## Tech Stack

- **Frontend**: .NET MAUI
  - Cross-platform support for iOS and Android.
- **Backend**: Go
  - Provides API for movies, genres, recommendations, and more.
- **Database**: PostgreSQL
  - Stores movie details, genres, and user interaction data.
- **Authentication**: Firebase
  - Manages secure user login and registration.

## Setup

### Prerequisites

1. **.NET MAUI**: Follow the [Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/maui/) to install .NET MAUI on your system.
2. **Go**: Install Go from the official website [here](https://golang.org/dl/).
3. **PostgreSQL**: Install PostgreSQL for database management.
4. **Firebase**: Set up Firebase for user authentication.

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/YourUsername/Watchify.git
   cd Watchify
   ```

2. **Backend Setup**:
   - Navigate to the API directory and set up the Go API:
     ```bash
     cd api
     go mod tidy
     go run main.go
     ```
   - Configure the database connection and other environment variables in the `.env` file.

3. **Frontend Setup**:
   - Open the project in Visual Studio or Visual Studio Code.
   - Build and run the app:
     ```bash
     dotnet build
     dotnet run
     ```

4. **Database Setup**:
   - Set up PostgreSQL and create necessary tables for movies, genres, and user interactions. 

### Running the Project

- Run the Go API server from the `api` directory.
- Start the Watchify .NET MAUI app on your preferred device or emulator.

## API Documentation

The backend API provides several endpoints to support the app’s functionality.

### Endpoints

- **Get Trending Movies**: `GET /movies/trending`
- **Get Recommended Movies**: `GET /movies/recommendations`
- **Get Movie Details**: `GET /movies/{id}`
- **Search Movies**: `GET /movies/search?q={query}`

For detailed documentation, refer to the [API documentation]([api/README.md](https://github.com/Darzu-Catalin/watchify-api)).

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the project.
2. Create a new branch for your feature (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m "Add a new feature"`).
4. Push to the branch (`git push origin feature-name`).
5. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
