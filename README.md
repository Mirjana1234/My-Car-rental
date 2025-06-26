 My Car Rental

A modern web application for car rental, built with **ASP.NET Core MVC** and **Entity Framework**.

Features

- **Homepage** with hero section and requirements for picking up a car
- **Car catalog** with card-based design
- **User booking**: select dates, see price, and book a car
- **Admin area**: manage cars and view all bookings
- **User authentication** (register/login)
- **Demo data seeding** for easy testing
- **Responsive design** for desktop and mobile

Technologies

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (with SQLite)
- Bootstrap 5 (UI)
- Identity for authentication

How to Run

1. **Clone the repository**
2. Open in Visual Studio
3. Run the project (`dotnet run` or F5)
4. The app will auto-create and seed the database if needed

Development Notes

- No real emails are sent (dummy email sender for development)
- All main features are accessible from the navigation bar
