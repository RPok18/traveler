# Traveler AI - Intelligent Itinerary Planner

Traveler AI is a C# command-line application designed to help users plan optimized travel itineraries using Dijkstra's algorithm. It supports budget constraints, travel preferences, and provides AI-powered recommendations.

## Features

- **Dijkstra-based Route Optimization**: Find the fastest or cheapest route between cities.
- **Budget Tracking**: Ensure your trip stays within monetary constraints.
- **Multi-stop Planning**: Use the `--via` flag to add waypoints to your journey.
- **AI Recommendations**: Get smart travel tips based on your budget, passengers, and duration.
- **Persistence**: Save your itineraries to a local SQLite database and view history.
- **Export**: Export your planned trips to JSON files for external use.

## Quick Start

### Build the project
```bash
dotnet build
```

### Plan a trip
```bash
dotnet run -- plan --from JFK --to LHR --budget 2000 --days 10 --recommend --save
```

### Optimize for budget
```bash
dotnet run -- optimize --from LAX --to SYD --budget 1000 --goal Cheapest
```

### View history
```bash
dotnet run -- history
```

## Commands & Options

Run `dotnet run -- help` to see all available commands and flags.
