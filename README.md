# Job Tracker

**A simple ASP.NET Core MVC app to track your job applications, follow‑ups, and offers.**

## Overview

This ASP.NET Core MVC application lets users:

- Add, edit, delete job applications  
- Tag entries with statuses: Interview Scheduled, Follow‑Up Done, Offer Received  
- Dashboard overview (totals & breakdown by status)  
- AJAX filtering, sorting, and pagination via `fetch()`  
- Secure endpoints & pages with ASP.NET Core Identity  

## Tech Stack

* **ASP.NET Core 8.011 MVC + Razor Pages
* **Entity Framework Core with SQL Server
* **ASP.NET Core Identity for authentication & authorization
* **Bootstrap 5 + custom ETSU color palette for responsive UI
* **Vanilla JS for RESTful table UI 

## Prerequisites
- NET 8.0 SDK
- SQL Server (LocalDB, Express, or full)

## Setup & Running Locally

1. Clone the repo:

   ```bash
   git clone https://github.com/Elias10Afshar/CSCI3110TermProject.git
   cd CSCI3110TermProject

2. Configure connection string in appsettings.Development.json

3. Apply migrations & seed data:
	- dotnet ef database update

4. Run the app
	- dotnet run --project CSCI3110TermProject.Web
	
5. Browse to https://localhost:7029 and register a new user.


## Documentation & AI Disclosure

* **XML Comments**: All controller actions, view models, and public methods include `<summary>` and `<param>` tags.
* **README**: This file describes setup, usage, and design notes.
* **AI Tools Used**: ChatGPT was used for general project advice and fixing potenital bugs or errors. AI was not used for generating any kind of code, only for review.  

## Design Decisions

* **Many‑to‑many** between `JobApplication` and `Tag` for flexible status tracking.
* **AJAX‑first table**: A single server‑endpoint (`JobApplicationsApiController`) supports filtering, sorting, paging in one JSON contract for simplified front‑end logic.
* **Badge styling**: Human-friendly labels and ETSU brand colors applied dynamically in JS.


