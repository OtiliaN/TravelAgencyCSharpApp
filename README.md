# 🧳 Travel Agency C# Application

This project was developed as part of a university assignment and implements a multi-stage travel agency system using C#. The application allows travel agency employees to log in, search for flights, book tickets for tourists, and receive real-time updates on flight availability.

## 🧩 Project Overview

Multiple travel agencies use a common airline to book tickets for their customers. Each agency employee interacts with the system through a desktop application with the following core features:

- **Login**: Authenticated access to the system.
- **Flight Search**: Search flights by destination and departure date.
- **Booking**: Purchase tickets for tourists by selecting the number of seats and entering tourist names.
- **Real-Time Updates**: All connected employees see updated flight information instantly when changes occur.
- **Logout**: Exit the application securely.

---

## 🛠️ Technologies Used

- **C# / .NET Framework**
- **Windows Forms (WinForms)**
- **SQLite** – Local database
- **JSON Protocol** – For communication in the Client-Server phase
- **gRPC + Protocol Buffers** – For cross-platform Java client – C# server communication
- **HttpClient** – For testing Java REST API from the C# side

---

## 🌳 Branches

### `networking-part` – Client-Server Architecture (C# only)
Implements the system entirely in C#, with modular separation into:
- `TravelClient`
- `TravelModel`
- `TravelServices`
- `TravelNetworking`
- `TravelPersistence`
- `TravelServer`

The communication is implemented via a custom JSON-based protocol between the client and the server.

  > 🔥 This is the current version on the `main` branch.

### `server-csharp` – Cross-Platform Phase (Java Client + C# Server)
This branch contains the C# implementation of the **server** used during the cross-platform development phase, where the client was implemented in Java.

- Uses **gRPC** and **Protocol Buffers** for communication.
- Defines `.proto` files shared between client and server.
- Implements all flight and booking logic on the C# side.

  [see java project (client-java branch)](https://github.com/OtiliaN/TravelAgencyJavaApp)

### `test-RESTapi-httpClient` – Testing Java REST API from C#
This branch demonstrates how to **consume a REST API** (implemented in Java with Spring Boot) from a C# client using `HttpClient`.

- Sends HTTP requests to test CRUD operations on flights.
- Validates integration between C# and REST-based Java services.

---

## 📁 Project Structure

Each module corresponds to a logical component of the application:

- `TravelClient` – Windows Forms-based desktop client
- `TravelModel` – Domain entities (Flight, Agent, Booking, etc.)
- `TravelNetworking` – Handles client-server communication via JSON
- `TravelPersistence` – Repository layer with SQLite interaction
- `TravelServices` – Business logic / service layer
- `TravelServer` – The main server logic

---

## 🧪 Features Recap

- Search flights by criteria
- Book seats with tourist details
- Automatic UI updates after booking
- Modular and scalable codebase
- Cross-platform compatible backend

---

## 🔗 Related Projects

- [TravelAgencyJavaApp (Java version with REST/WebSockets/React)](https://github.com/OtiliaN/TravelAgencyJavaApp) – Java counterpart of this C# project, including cross-platform integration.

---

## 📌 Notes

This project demonstrates not only a standard client-server application in C#, but also:
- Cross-platform interoperability via gRPC
- REST API integration with an external system
- Good modular separation and scalable design

---


