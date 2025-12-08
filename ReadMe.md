# 🚀 Todora - Collaborative Project Management API

**Todora** is a robust, scalable, and multi-user **.NET 9 Backend** project designed for teams to organize projects (repositories), manage tasks, and streamline workflows.

Built on **Onion Architecture** principles, this project adheres to modern software development practices such as **CQRS** and **Clean Code**, prioritizing high security standards with **Resource-Based Authorization**.

![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)
![Platform](https://img.shields.io/badge/Platform-.NET%209.0-blueviolet)
![License](https://img.shields.io/badge/License-MIT-blue)

## 🏗 Architecture & Design

The project implements **Onion Architecture** to ensure separation of concerns and maintainability, with dependencies flowing inward.

### 🧩 Layers
1.  **Domain (Core):** The heart of the project containing only Entities, Enums, and Value Objects. It has no external dependencies.
2.  **Application:** Contains the Business Logic, including CQRS (Commands/Queries), DTOs, Interfaces, Validators, and Mapping Profiles.
3.  **Persistence (Infrastructure):** Handles Database access, Migrations, Seeding, and Repository implementations.
4.  **API (Presentation):** The entry point comprising Controllers and Middleware configurations.

### 🛠 Technologies & Patterns

* **Framework:** .NET 9 & ASP.NET Core Web API.
* **Database & ORM:** SQL Server (MSSQL), Entity Framework Core (Code-First).
* **Architectural Patterns:** Onion Architecture, Repository & Unit of Work Pattern.
* **Workflow:** CQRS (Command Query Responsibility Segregation) + MediatR.
* **Validation:** FluentValidation + MediatR Pipeline Behaviors (Automated Request Validation).
* **Object Mapping:** AutoMapper.
* **Security:**
    * ASP.NET Core Identity.
    * JWT (JSON Web Token) Authentication.
    * **Resource-Based Authorization:** Custom authorization logic that validates user permissions dynamically based on their specific role within a Repository (e.g., Owner, Manager, Guest) rather than just a global role.
* **Logging:** Serilog (MSSQL Sink).
* **Testing:** xUnit, Moq, FluentAssertions.
* **Documentation:** Swagger / OpenAPI.

## 🔥 Key Features

* **Advanced Repository Management:** Users can create isolated workspaces (Repositories) for their projects.
* **Dynamic Role Management:** Granular role assignment within repositories (Owner, Manager, Senior Member, Junior Member, Guest, etc.).
* **Task Lifecycle:** Complete flow for creating, assigning, staging, and completing tasks.
* **Robust Middleware Pipeline:**
    * Global Exception Handling.
    * Result Wrapper (Standardized API Responses).
    * Correlation ID (Request Tracing).
    * Logging Middleware.
* **Notification System:** Built-in domain support for user notifications.

## 📂 Project Structure

```bash
Todora
├── 📁 API                 # Entry point, Controllers, Middlewares
├── 📁 Application         # Business Layer (CQRS, Interfaces, Validators)
├── 📁 Domain              # Core Layer (Entities, Enums)
├── 📁 Persistence         # Data Access (DbContext, Migrations, Repositories)
└── 📁 Tests               # Unit Tests (xUnit)
```

## ⚙️ Setup & Run

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/Zeth21/Todora.git](https://github.com/Zeth21/Todora.git)
    cd Todora
    ```

2.  **Configure Database:**
    Update the `DefaultConnection` string in `API/appsettings.json` with your SQL Server credentials.

3.  **Apply Migrations:**
    Open your terminal in the `API` directory and run:
    ```bash
    dotnet ef database update --project ../Persistence --startup-project .
    ```
    *Note: The `DataSeeder` will automatically populate the database with default Roles and an Admin user upon startup.*

4.  **Run the Project:**
    ```bash
    dotnet run
    ```
    Access the Swagger UI at `https://localhost:7253/swagger`.

## 🧪 Testing

The project utilizes **xUnit** for unit testing. Handlers, services, and business logic are isolated and tested using **Moq** to ensure reliability.

To run tests:
```bash
dotnet test
```
## 👨‍💻 Author

**Zeyitcan DASDEMIR** - *Backend Developer*

* **Contact:** zytcn01@outlook.com

---

### 💡 Technical Highlights

* **Pipeline Validation:** Validation rules are decoupled from Controllers and handled automatically within the MediatR Pipeline using `ValidationBehavior`.
* **Resource Authorization:** Implements `ResourceAuthorization<T>` handlers to strictly control access to specific resources (Tasks, Repositories) based on context-aware permissions.
* **Consistent Responses:** All API responses are wrapped in a generic `Result<T>` structure via middleware, ensuring a predictable contract for clients.