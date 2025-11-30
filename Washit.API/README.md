Part 2 – Design/Description & Extension Plan
=================================

The following section describes how the Washit application would be extended to support:

===============================================
🔐 1. User Identification (Only reserver can use the machine)
Identification of the end user so only the person who reserved can use 
the machine.
The following enhancements would be added:
================================================

# 1.1 Add User Authentication

Use JWT-based Authentication

Add a Users table in SQL Server

Users(
    Id INT PRIMARY KEY IDENTITY,
    UserName NVARCHAR(50),
    PasswordHash NVARCHAR(200)
)


Implement login endpoint:
POST /api/auth/login → returns JWT token

Register endpoint:
POST /api/auth/register

# 1.2 Include UserId in Reservations

Add UserId foreign key inside Reservations:

# 1.3 Controller Authorization

Apply JWT Authorization:

[Authorize]
public class ReservationController : ControllerBase
{
}

# 1.4 Ensure only the owner can access the reservation

When canceling or checking in, verify:

if(reservation.UserId != currentUser.Id) return Unauthorized("This reservation does not belong to you.");


This ensures:

- Only reserver can cancel
- Only reserver can check-in
- Only reserver can use the machine

=============================================
2. Automatic Cancellation After 15 Minutes
=============================================

The requirement:

Automatic cancellation if the user does not check in within 15 minutes.

To support this:

# 2.1 Reservation Should Store Timing Information

The table already contains:

StartsAt

ExpiresAt = StartsAt + 15 minutes

CheckedIn = 0 or 1

This allows the system to decide whether to auto-cancel.

# 2.2 Add Background Worker (Hosted Service)

Use .NET BackgroundService:
public class ReservationMonitorService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            await _reservationService.AutoCancelExpiredReservations();
            await Task.Delay(TimeSpan.FromMinutes(1), token);
        }
    }
}
This job runs every minute.


# 2.3 Auto-Cancel Logic

AutoCancelExpiredReservations() would:

Query expired reservations:

WHERE CheckedIn = 0 
AND ExpiresAt < GETDATE()

- Cancel those reservations

- Mark machines as available

- Notify next user on waiting list (IsNotified = 1)

# 2.4 Notification Behavior

Notification remains simple per project requirement:

✔ Update a flag (IsNotified = 1)
✔ OR add a log entry
✔ OR show notification in Angular UI

# 2.5 Angular Handling

Angular polls every 30–60 seconds:

If user has a reservation → show “Check In” button

If auto-cancel happened → show “Reservation expired”

If user is notified from waiting list → show “Machine is available for you”

(Polling can later be replaced with SignalR.)