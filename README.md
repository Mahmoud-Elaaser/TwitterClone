# Twitter Clone API

Twitter Clone API is a backend application built using **ASP.NET Core** that replicates core features of a social media platform like Twitter. The project is designed with clean architecture principles, ensuring scalability, maintainability, and performance.

---

## Features

### User Management
- **User Registration**: Users can register with their email and username.
- **Authentication**: Secure **JWT**-based authentication.
- **Role-Based Access Control**: Supports **Admin** and **User** roles for managing permissions.
- **Password Management**: Change password, forgot password, and reset password via email services.
- **User Profiles**: Update bio, email, and profile picture.

### Post Management
- Full CRUD operations for tweets and quotes, including timestamps and user associations.

### Social Features
- **Follow/Unfollow Users**: Connect with other users by following or unfollowing them.
- **Mute/Block Users**: Customize your experience by muting or blocking specific users.
- **Create Tweets**: Share your thoughts and updates with others on the platform.
- **Quote Tweets**: Add your commentary while sharing another user's tweet.
- **Retweet**: Share existing tweets with your followers to amplify them.
- **Likes**: Express appreciation or agreement by liking tweets.
- **Comments**: Engage in discussions by adding comments to tweets.

### Notifications
- Notifications for:
  - Follow actions.
  - Likes and comments on posts.

### Feeds
- Personalized feeds showing posts from followed users.


### API Documentation
- Fully documented API using Swagger.

---

## Technologies Used

### Core Technologies
- **.NET 8**: Backend framework.
- **ASP.NET Core Web API**: For RESTful API development.
- **C#**: Core programming language.

### Database and ORM
- **Entity Framework Core**: Code-first database management.
- **SQL Server**: Relational database engine.

### Design Patterns
- **Specification Design Pattern**: Ensures reusable and flexible filtering for queries.
- **Repository Pattern**: Centralizes database interactions.
- **Unit of Work**: Manages transactions and database operations.
- **Dependency Injection**: Decouples dependencies and enhances testability.

### Security
- **JWT Authentication**: Secure token-based authentication.
- **Role-Based Access Control**: Restricts access to resources based on user roles.
- **Data Protection API**: For token and sensitive data security.

### Additional Tools
- **AutoMapper**: Model-to-DTO mapping.
- **FluentValidation**: Input validation.
- **Swagger**: API documentation and testing.

---

## Project Structure
The project follows **Clean Architecture** with the following layers:
- **Data Layer**: Business models.
- **Service Layer**: Business rules, services, mapping, and DTOs.
- **Infrastructure Layer**: Data access and external integrations.
- **Api Layer**: RESTful API endpoints.

---

## Installation

### Prerequisites
Make sure you have the following installed:
- [.NET SDK 8.0](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Mahmoud-Elaaser/TwitterClone.git
2. **Navigate to the project directory:**
   ```bash
   cd TwitterClone
3. **Set Up Configuration**:
   Update your appsettings.json file or configure User Secrets
4. **Restore packages:**
   ```bash
   dotnet restore
5. **Apply migrations**
   ```bash
   dotnet ef database update
6. **Run**
   ```bash
   dotnet run

---

### Screenshot

Here is a full screenshot of the project:

![Screenshot](https://github.com/Mahmoud-Elaaser/TwitterClone/blob/master/TwitterCloneAPI.png)
   
   
 

  


