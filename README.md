## Angular 21 + ASP.NET Web API – Student CRUD Example

Simple end‑to‑end **Student CRUD (Create, Read, Update, Delete)** application built with **Angular 21 standalone components** on the frontend and **ASP.NET Web API (.NET 10 + Entity Framework Core + SQL Server)** on the backend.

This project is aimed at **beginners** who want to see how a modern Angular frontend talks to a RESTful ASP.NET Web API backend using JSON over HTTP.

---

## Tech Stack

- **Frontend**: Angular 21, Standalone Components, `HttpClient`, Forms
- **Backend**: ASP.NET Core Web API (.NET 10), Entity Framework Core, Swashbuckle (Swagger)
- **Database**: SQL Server (via `Microsoft.EntityFrameworkCore.SqlServer`)
- **Package managers / tooling**:
  - .NET SDK
  - Node.js + npm
  - Angular CLI (local via `@angular/cli` in `devDependencies`)
  - Optional: `dotnet-ef` CLI for running EF Core migrations

---

## Project Structure

At the root of the repository:

```text
angular-aspnet-web-api-crud/
├── SimpleCrudApi/           # Backend – ASP.NET Web API + EF Core
│   └── SimpleCrudApi/
│       ├── Controllers/
│       │   └── StudentsController.cs
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Migrations/      # EF Core migrations (database schema)
│       ├── Models/
│       │   └── Student.cs
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── Program.cs       # App startup, DI, CORS, Swagger, DbContext
│       ├── appsettings*.json
│       └── SimpleCrudApi.csproj
│
├── SimpleCrudUi/            # Frontend – Angular 21 app
│   ├── angular.json         # Angular CLI configuration
│   ├── package.json         # npm scripts and dependencies
│   └── src/
│       ├── app/
│       │   ├── app.ts       # Root standalone component (CRUD logic)
│       │   ├── app.html     # Template (form + list UI)
│       │   └── app.css      # Styling
│       ├── main.ts          # Angular bootstrap entry point
│       └── index.html
│
└── README.md                # This file
```

---

## Features

- **Full CRUD for students**
  - Create new students
  - List all students
  - Edit an existing student’s name
  - Delete students
- **RESTful backend API**
  - `GET /api/students`
  - `POST /api/students`
  - `PUT /api/students/{id}`
  - `DELETE /api/students/{id}`
- **Entity Framework Core integration**
  - `Student` entity with `Id` and `Name`
  - `AppDbContext` with `DbSet<Student>`
  - Migrations folder for database schema
- **Swagger UI**
  - Auto‑generated interactive API docs in Development
- **CORS enabled**
  - Policy `AllowAngular` allows calls from the Angular dev server
- **Beginner‑friendly Angular UI**
  - Simple form + list layout
  - Clear **Add / Update / Delete** actions

---

## Backend – ASP.NET Web API

### 1. Requirements

- **.NET SDK 10** (or compatible preview that supports `net10.0`)
- **SQL Server** (LocalDB, Developer, or Express edition)

### 2. Configure the database connection

The backend connection string is defined in `SimpleCrudApi/SimpleCrudApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StudentDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

- **Change `YOUR_SERVER_NAME`** to your own SQL Server instance name.
- If you are not using Windows authentication, you can extend the connection string locally with `User Id=...;Password=...;` but **do not commit real usernames/passwords to GitHub**.
- The database name is `StudentDb` (you can change this if you like).

> **Tip for beginners**: If you are unsure what to use for `Server`, try:
> - `localhost`
> - `localhost\\SQLEXPRESS`
> - `(localdb)\\MSSQLLocalDB`

### 3. Apply EF Core migrations (create the database)

Migrations already exist in `SimpleCrudApi/SimpleCrudApi/Migrations`. To create/update the database you can use the EF Core CLI:

```bash
cd SimpleCrudApi/SimpleCrudApi

# (One-time) install the EF Core tools if you don't have them
dotnet tool install -g dotnet-ef

# Apply migrations and create the StudentDb database
dotnet ef database update
```

If you prefer, you can also create the database manually and let EF create tables on first run, but using `dotnet ef database update` is the recommended approach here.

### 4. Run the Web API

From the backend project folder:

```bash
cd SimpleCrudApi/SimpleCrudApi
dotnet restore
dotnet run
```

By default (based on `launchSettings.json`), the API will listen on:

- **HTTPS**: `https://localhost:7237`
- **HTTP**: `http://localhost:5229`

The main Student API endpoint used by the Angular app is:

- **`https://localhost:7237/api/students`**

### 5. Explore the API with Swagger

When the backend is running in `Development` environment, Swagger is enabled in `Program.cs`:

- Open a browser and go to: `https://localhost:7237/swagger`
- You will see all endpoints under `Students`:
  - `GET /api/students`
  - `POST /api/students`
  - `PUT /api/students/{id}`
  - `DELETE /api/students/{id}`

You can test the API directly from this page before wiring it up (or in addition to) the Angular UI.

---

## Frontend – Angular 21 Standalone App

### 1. Requirements

- **Node.js** (LTS recommended, e.g. 20.x)
- **npm**
- (Optional) **Angular CLI** globally: `npm install -g @angular/cli`

### 2. Install dependencies

From the frontend folder:

```bash
cd SimpleCrudUi
npm install
```

### 3. Start the Angular dev server

```bash
cd SimpleCrudUi
npm run start      # Equivalent to: ng serve
```

By default, the Angular dev server runs at:

- **`http://localhost:4200/`**

### 4. API base URL used by the Angular app

In `SimpleCrudUi/src/app/app.ts`, the Angular app calls the backend at:

```ts
private apiUrl = 'https://localhost:7237/api/students';
```

Make sure this matches how your backend is running:

- If your API is on **another port or host**, update `apiUrl` accordingly.
- If you only use **HTTP** (not HTTPS), you can change it to e.g. `http://localhost:5229/api/students`.
- Ensure the ASP.NET dev certificate is trusted if you keep using HTTPS.

---

## How to Run the Full Stack

1. **Start the backend (Web API)**  
   - Open a terminal:
     ```bash
     cd SimpleCrudApi/SimpleCrudApi
     dotnet run
     ```
   - Confirm it is running at `https://localhost:7237` (or your configured URL).

2. **Start the frontend (Angular)**  
   - Open a second terminal:
     ```bash
     cd SimpleCrudUi
     npm install      # first time only
     npm run start
     ```
   - Open your browser at `http://localhost:4200/`.

3. **Verify that the UI loads and calls the API**  
   - Open the browser dev tools **Network** tab.
   - Perform an action (e.g. Add a student) and you should see requests going to `/api/students` on your backend URL.

---

## Testing CRUD Operations

You can test CRUD in two ways: **via the Angular UI** or **directly via the API (Swagger / HTTP client)**.

### 1. Testing via Angular UI

1. Open `http://localhost:4200/` in your browser.
2. **Create (POST)**  
   - Type a student name in the input box (e.g. `Alice`).
   - Click **Add**.  
   - You should see `Alice` appear in the list.
3. **Read (GET)**  
   - On page load, the app automatically calls `GET /api/students` to populate the list.
   - After adding or editing, the list is reloaded.
4. **Update (PUT)**  
   - Click **Edit** next to a student.
   - The name appears in the input field and the button text changes to **Update**.
   - Change the name (e.g. to `Alice Smith`) and click **Update**.
   - The list should show the updated name.
5. **Delete (DELETE)**  
   - Click **Delete** next to a student.
   - The student should disappear from the list.

If any of these do not work, check:

- That the **backend is running** and reachable.
- That the **`apiUrl`** in `app.ts` is correct.
- Browser dev tools console for errors (CORS, SSL, etc.).

### 2. Testing via Swagger or HTTP client

- **Using Swagger UI**  
  - Navigate to `https://localhost:7237/swagger`.
  - Try each endpoint with sample data:
    - `POST /api/students` with body `{"name": "Bob"}`.
    - `GET /api/students` to see all students.
    - `PUT /api/students/{id}` to update a specific student.
    - `DELETE /api/students/{id}` to remove a student.

- **Using `curl` (example)**  
  Adjust the base URL if needed.

  ```bash
  # Create
  curl -k -X POST "https://localhost:7237/api/students" \
    -H "Content-Type: application/json" \
    -d "{\"name\":\"Alice\"}"

  # Read
  curl -k "https://localhost:7237/api/students"

  # Update (replace 1 with an actual ID)
  curl -k -X PUT "https://localhost:7237/api/students/1" \
    -H "Content-Type: application/json" \
    -d "{\"name\":\"Alice Updated\"}"

  # Delete (replace 1 with an actual ID)
  curl -k -X DELETE "https://localhost:7237/api/students/1"
  ```

> Note: The `-k` flag tells `curl` to ignore SSL certificate validation errors. In a real production environment, you should use a proper trusted certificate instead.

---

## Notes & Tips for Beginners

- **Backend vs Frontend separation**
  - The **backend** (`SimpleCrudApi`) is responsible for **data, business logic, and persistence**.
  - The **frontend** (`SimpleCrudUi`) is responsible for **user interface and calling the API**.
- **CORS**  
  - The backend enables CORS with a policy named `AllowAngular` and `AllowAnyOrigin/AllowAnyHeader/AllowAnyMethod`, so the Angular dev server can talk to it without issues during development.
- **Don’t commit secrets**  
  - In real projects, you should not commit real usernames/passwords in `appsettings.json`. Use **user secrets** or environment variables instead.
- **Where to change the API URL**
  - Update the URL in `SimpleCrudUi/src/app/app.ts` (`apiUrl`) if your backend runs on a different host/port.
- **Next steps / practice ideas**
  - Add more fields to the `Student` model (e.g. `Email`, `Age`).
  - Add validation on both backend and frontend.
  - Introduce routing in Angular and split the app into multiple components.

---

## License

You can use this project freely for learning, demos, or as a starting point for your own CRUD projects.
