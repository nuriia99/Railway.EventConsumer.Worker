# Railway.EventConsumer.Worker

A background worker application that consumes and processes messages from RabbitMQ using .NET 10 and asynchronous message handling patterns.

## Overview

This worker application listens to a RabbitMQ queue, consumes incoming messages, and routes them to appropriate handlers based on message type. It implements the **Strategy Pattern** for flexible and extensible message processing.

## Project Structure

- **Railway.EventConsumer.WebApi**: Entry point and host application (Program.cs). Sets up dependency injection and starts background services.
- **Railway.EventConsumer.Application**: Core business logic including message handlers and handler interfaces.
- **Railway.EventConsumer.Infrastructure**: External integrations (RabbitMQ consumer, message dispatcher).
- **Railway.EventConsumer.Domain**: Domain models and events (MessageTest, etc.).
- **Railway.EventConsumer.CrossCutting**: Shared utilities and helpers.
- **Railway.EventConsumer.UnitTests**: Unit tests using xUnit.

## Current Functionality

### Message Consumption
- Connects to RabbitMQ and listens for incoming messages on a configured queue
- Processes messages asynchronously as a background service
- Automatically acknowledges successfully processed messages

### Message Processing
- Parses incoming JSON messages with a `type` and `data` structure
- Routes messages to the appropriate handler based on the message type
- Each message type has its own dedicated handler (Strategy Pattern)

### Error Handling & Retries
- Captures exceptions during message processing
- Implements automatic retry mechanism with configurable retry count (max 3 retries)
- Moves failed messages to a retry queue with retry count tracking
- Discards messages after reaching max retry attempts

### Current Message Types
- **MessageTest**: Handles test messages with `MessageId` and `Amount` properties

### Strategy Pattern

This application uses the **Strategy Pattern** to handle different message types dynamically and independently.

**How it works:**
- Each message type has its own handler implementation
- The `MessageDispatcher` determines which handler to use based on the message type
- New message types can be added without modifying existing code

**Benefits:**
- Easy to add new message types without touching existing code
- Handlers are isolated and testable independently
- Clear separation of concerns

Quick start
1. Restore packages: `dotnet restore`
2. Build: `dotnet build`
3. Run the API: `dotnet run --project Railway.EventConsumer.WebApi`

After the API is running you can open the Scalar UI (OpenAPI viewer) at: `https://localhost:{port}/scalar` (or the path configured by MapScalarApiReference; check console output for the actual port).
