# Toronto DineSafe Map

A Blazor WebAssembly application that visualizes food establishment inspection data from the City of Toronto's DineSafe program.

## Overview

This application provides an interactive map showing food establishments in Toronto along with their health inspection results. Users can filter establishments by status (Pass, Conditional Pass, Closed), severity of infractions, and establishment type to explore food safety information across the city.

## Features

- **Interactive Map**: Displays food establishments across Toronto using Leaflet.js
- **Filtering Options**: Filter establishments by:
  - Inspection status (Pass, Conditional Pass, Closed)
  - Infraction severity (Minor, Significant, Crucial)
  - Establishment type
- **Detailed Information**: Click on map markers to view detailed inspection information
- **Responsive Design**: Works on desktop and mobile devices

## Technology Stack

- **Frontend**: Blazor WebAssembly (.NET 8.0)
- **Mapping**: Leaflet.js
- **Data Processing**: CsvHelper
- **Data Source**: City of Toronto Open Data Portal (DineSafe dataset)

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- A modern web browser

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```bash
dotnet run
```

4. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## How It Works

1. The application fetches DineSafe inspection data from the City of Toronto's Open Data Portal
2. Data is processed and filtered based on user selections
3. Establishments are displayed on an interactive map with color-coded markers
4. Users can click on markers to view detailed inspection information

## Project Structure

- **Models/**: Contains the data model for DineSafe records
- **Services/**: Contains services for fetching data and JavaScript interop
  - `DineSafeService.cs`: Handles API requests to the Toronto Open Data Portal
  - `MapJsInterop.cs`: Provides JavaScript interop for map functionality
- **Pages/**: Contains Blazor pages
  - `DineSafeMap.razor`: Main page with map and filtering functionality
- **wwwroot/js/**: Contains JavaScript files
  - `map.js`: Handles map initialization and marker management

## Data Source

#### Data Refresh Rate: Daily

This application uses data from the [City of Toronto's DineSafe program](https://open.toronto.ca/dataset/dinesafe/), which provides information about food safety inspections of restaurants and other food establishments in Toronto.

## Acknowledgements

- City of Toronto Open Data Portal for providing the DineSafe dataset
- OpenStreetMap contributors for map data
- Leaflet.js for mapping functionality

# TODO:

- fix requests to the Toronto Open Data Portal
