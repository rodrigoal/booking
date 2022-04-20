# Booking API

This API was created using ASP .NET Core 6 with Entity Framework and SQL Azure.

It is running at https://bookingtestapi.azurewebsites.net. You can try for a month.

# Some considerations
1. We have just one room. The room ID is 1;
2. To identify the user, you can input a passport (string) and a country id (int);
3. You can see a list of countries by the API;
4. We can just update or delete my own bookings;

# Requirements
1. You can not booking for no longer than 3 days;
2. You can book starting next day;
3. The Day of booking is starting at 00:00 until 23:59;
4. You can not book after 30 days in advance;

# Methods
1. Create a reservation;
2. Update a reservation;
3. Delete a reservation;
4. List my reservations;
5. List free dates to booking;
6. List the countries;

# Technical considerations
1. We are using a mix of Entity Framework and SQL raw; It can improve operations using procedures and functions;
2. We are using one project, but it was organized using Repositories, Business classes and Controllers;
3. We are using Models and Entities to separate the concepts that user's can work;
4. We enable the swagger for just test purposes;
5. The business classes are validating some rules, but it could be at the sql procedures too;
6. Starting a new api project is a good opportunity to create a layout using base concepts and services that will be very useful to the following projects;
