# Overview
Electronics_Store is a web application built using ASP.NET Core MVC and Entity Framework. The application allows users to browse through a variety of Electronics, add them to their cart, place orders, and manage their account. It also includes administrative features for managing products, categories, and orders. The project incorporates user authentication and authorization, enabling different roles for customers and administrators. For payment, the app integrates with Stripe to handle secure transactions. The project explain the use of various modern technologies and best practices such as the Repository Pattern and dependency injection.


# Features

<h3> 1-User Authentication & Authorization:</h3>
-Register and login functionality for users.<br>
-Role-based authorization (Admin & Customer roles).<br>
-Identity framework integration to manage users.

<h3>2-CRUD Operations:</h3>
-Manage Electronics (create, read, update, delete).<br>
-Manage Electronics categories and Products<br>
-Manage customer orders and view order history.<br>
-Manage shopping cart (add, remove items).

<h3>3-Payment Integration:</h3>
-Stripe payment gateway integration for secure payments.

<h3>4-Admin Panel:</h3>
-Admin users can add, edit, and delete Electronics and categories<br>
-Admin users can view and manage customer orders.

# Technology Stack
ASP.NET Core MVC: For building the web application using the Model-View-Controller pattern.<br>
Entity Framework Core: For database management and interaction using code-first migrations.<br> 
SQL Server: As the database for storing Electronics, users, orders, etc.<br>
ASP.NET Identity: For handling user registration, login, and role management.<br>
Stripe Payment Gateway: For secure payment processing.<br>
Razor Pages & Views: For dynamic content rendering.<br>
Dependency Injection: For managing services in a loosely coupled manner.<br>
Repository Pattern: For interacting with the database through a clean abstraction.<br>
