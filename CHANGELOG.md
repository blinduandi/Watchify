# Changelog

This changelog provides a weekly update of all major changes and feature developments from September 2 to November 4, 2024.

---

## Week of September 2, 2024

### Initial Development Phase
- **Project Setup**:
  - Established foundational codebase for the .NET MAUI frontend and Go backend.
  - Configured PostgreSQL database schema to store movie details, genres, and user interactions.

- **Authentication**:
  - Integrated Firebase for secure user login, registration, and authentication workflows.
  - Configured Firebase in `MauiProgram.cs`.

### Page Development
- **Main Page**:
  - Designed layout for displaying trending movies with a dynamic "See More" option.
  - Implemented lazy loading for seamless movie browsing.

- **Search Page**:
  - Built search functionality for finding movies by title.
  - Added result pagination with real-time querying.

---

## Week of September 9, 2024

### Feature Expansion
- **Recommendation Page**:
  - Developed personalized movie recommendation system based on user preferences.
  - Implemented swipe gestures for movie discovery: **Swipe Right** to like and **Swipe Left** to dismiss.

- **Backend API**:
  - Built initial API endpoints in Go to fetch trending and recommended movies.
  - Optimized PostgreSQL queries for faster data retrieval.

### UI/UX Improvements
- **Swipe Interaction**:
  - Added smooth animations to the swipe functionality for a more engaging experience.

- **AppShell Navigation**:
  - Configured `AppShell` to manage page routes and disable navigation bars, creating a streamlined user interface.

---

## Week of September 16, 2024

### New Pages and Enhancements
- **Movie Page**:
  - Displayed detailed information for each movie, including cast, genre, and synopsis.
  - Added high-quality posters and trailers for visual appeal.

- **Profile Page**:
  - Developed user profile view with options to edit profile picture and display name.

### Backend and API Updates
- **Genre and Cast Filtering**:
  - Enhanced recommendation page with genre and cast filters for custom recommendations.

- **Error Handling**:
  - Developed custom error views for better user experience during connectivity or data retrieval issues.

---

## Week of September 23, 2024

### Feature Finalization
- **Authentication Enhancements**:
  - Completed Firebase authentication workflows, including email verification and "Forgot Password."

- **User Interaction**:
  - Added user rating options on movie pages.
  - Created a swipe history log in profiles for quick access to liked and dismissed movies.

### Performance Optimizations
- **API Response Handling**:
  - Streamlined API responses to reduce payload sizes and improve loading times.

---

## Week of September 30, 2024

### Advanced Features and Bug Fixes
- **Watchlist Functionality**:
  - Added a watchlist feature allowing users to save movies for future viewing.

- **Recommendation Algorithm**:
  - Refined recommendation algorithm with enhanced genre and actor filters.

### Bug Fixes
- **Swipe Interface**:
  - Resolved issues with swipe animations freezing during rapid interactions.

---

## Week of October 7, 2024

### UI/UX Enhancements
- **Search Experience**:
  - Introduced auto-suggestions and recent searches in the search bar.
  - Improved search algorithm for faster results.

- **Profile Customization**:
  - Enabled genre and actor preferences in user profiles for personalized recommendations.

### Accessibility
- **Screen Reader Compatibility**:
  - Added screen reader support and high-contrast themes for improved accessibility.

---

## Week of October 14, 2024

### Database and Performance Enhancements
- **Database Optimization**:
  - Indexed frequently queried fields in PostgreSQL to enhance performance.

- **Community Rating System**:
  - Built a rating system where users can rate and review movies.

---

## Week of October 21, 2024

### New User Engagement Features
- **User Watch History**:
  - Added a watch history log in profiles, allowing users to revisit liked movies.

- **Improved Genre Filtering**:
  - Refined genre filtering on the recommendation page.

### Bug Fixes and Stability Improvements
- **Swipe Interface Stability**:
  - Improved swipe functionality responsiveness.

- **API Stability**:
  - Enhanced error handling for better recovery from API disruptions.

---

## Week of October 28, 2024

### Security Enhancements
- **Two-Factor Authentication (2FA)**:
  - Introduced optional 2FA for added security on user accounts.

- **Session Management**:
  - Improved session persistence for long-lasting logins.

### UI Improvements
- **Tablet Layout**:
  - Optimized UI for tablets with a multi-column view on larger screens.

---

## Week of November 4, 2024

### Final Touches
- **Community Features**:
  - Enabled a comment and review section for each movie.
  - Featured popular reviews on movie detail pages.

- **Bug Fixes**:
  - Fixed UI inconsistencies and improved error messages for clarity.

---

With these weekly updates, Watchify has evolved into a feature-rich, user-friendly movie recommendation platform that is optimized for performance, security, and accessibility.
