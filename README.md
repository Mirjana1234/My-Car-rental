# My Car rental

My Car rental/
  └── My Car rental/
      ├── Areas/
      │   └── Identity/           # Everything related to users and authentication
      ├── Controllers/            # Controllers (logic for routes, e.g., Home, Cars, Bookings)
      ├── Migrations/             # Database migrations (schema versions)
      ├── Models/                 # Data models (Car, Booking, etc.)
      ├── Services/               # Helper services (e.g., NoOpEmailSender)
      ├── Views/
      │   ├── Shared/             # Shared layout (_Layout.cshtml), header, footer, etc.
      │   ├── Home/               # Views for homepage and details
      │   ├── Cars/               # Views for cars
      │   └── Bookings/           # Views for bookings
      ├── wwwroot/                # Static files: images, CSS, JS
      ├── appsettings.json        # Application settings
      ├── Program.cs              # Application entry point
      └── MyCarRental.db          # SQLite database
