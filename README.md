# CacheService

A .NET application demonstrating caching implementations with both in-memory and Redis caching services.

## Project Structure

```
CacheService/
├── Controllers/
│   └── CacheController.cs
├── Interfaces/
│   ├── IMemoryCacheService.cs
│   └── IRedisCacheService.cs
├── Models/
│   └── CacheRequest.cs
├── Services/
│   ├── MemoryCacheService.cs
│   └── RedisCacheService.cs
├── Program.cs
└── CacheService.csproj
```

## Features

- **In-Memory Caching**: Uses .NET's built-in IMemoryCache for simple caching scenarios
- **Redis Caching**: Provides distributed caching capabilities with StackExchange.Redis
- **API Endpoints**: RESTful endpoints for cache operations
- **Configuration**: Easy configuration through appsettings.json

## Endpoints

- `POST /cache/memory` - Store data in memory cache
- `GET /cache/memory/{key}` - Retrieve data from memory cache
- `DELETE /cache/memory/{key}` - Remove data from memory cache
- `POST /cache/redis` - Store data in Redis cache
- `GET /cache/redis/{key}` - Retrieve data from Redis cache
- `DELETE /cache/redis/{key}` - Remove data from Redis cache

## Setup

1. Install dependencies:
```bash
dotnet restore
```

2. For Redis functionality, ensure Redis server is running

3. Run the application:
```bash
dotnet run
```

## Architecture

The application follows a clean architecture pattern:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Implement caching logic and business rules
- **Interfaces**: Define contracts for caching services
- **Models**: Represent data structures used in the application