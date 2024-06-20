# Carrier Shipping Rates Web API

This project is a Web API developed using C# that aggregates shipping rates from different carriers. The system allows users to query shipping rates based on package details and destination, fetching rates from multiple carrier APIs and returning them in a standardized format.

## Table of Contents
1. [Features](#features)
2. [Getting Started](#getting-started)
    1. [Prerequisites](#prerequisites)
    2. [Installation](#installation)
3. [Usage](#usage)
    1. [Carrier Management Endpoints](#carrier-management-endpoints)
    2. [Rate Query Endpoint](#rate-query-endpoint)
5. [Author](#author)

## Features

- **Rate Query Endpoint:**
  - Accepts package details including dimensions, weight, origin, and destination.
  - Fetches shipping rates from at least three different carrier APIs (e.g., FedEx, UPS, and DHL).
  - Returns the rates in a unified response format.
  - Implement in-memory database for simplicity to eliminate the need for database restoration.

- **Carrier Management:**
  - Endpoints to add, update, and remove carrier configurations (API endpoints, credentials, etc.).

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or any C# compatible IDE
- Access to carrier APIs (e.g., FedEx, UPS, DHL) with necessary credentials
- **Note:** For this task, I'll be using simulated API response data.

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/reekzap/CAPLINQ-Backend-Developer-Functional-Task.git
   cd CAPLINQ-Backend-Developer-Functional-Task

### Usage Notes

- **Simulated API Response Data:** The application will use hardcoded or mocked data to simulate responses from carrier APIs for demonstration purposes. This allows testing and development without actual integration with live carrier APIs.
  
- **Configuration:** Before running the application, ensure that you have configured the necessary API endpoints and credentials for the carriers in the application settings or through the provided management endpoints.

If you wish to integrate with real carrier APIs, replace the simulated data and update the respective configurations accordingly.


### Carrier Management Endpoints

POST api/Carrier/{id}/disable-carrier
To disable carrier
```string
    "Reason to Disable"
```
Possible Responses:

(200) OK
```json
    // if authorize as ADMIN
    {
      "message": "Carrier disabled successfully."
    }
    // if authorize as USER
    {
      "message": "Request to disable carrier sent."
    }
```
(400) ERROR

```json
    // Carrier ID not found
    {
      "message": "Carrier not found."
    }
    // Request without any reason
    {
      "message": "Cannot disable a carrier without a reason."
    }
    // Only active carrier
    {
      "message": "Cannot disable the only active carrier."
    }
    // Carrier has on-going shipments
    {
      "message": "Cannot disable a carrier with ongoing shipments."
    }
    // Carrier has pending invoices
    {
      "message": "Cannot disable a carrier with pending invoices."
    }
```

### Rate Query Endpoint

POST api/ShippingRates/rates
To query shipping rates using the API, use the following sample request payload:
```json
{
  "origin": {
    "postalCode": "12345",
    "countryCode": "US"
  },
  "destination": {
    "postalCode": "67890",
    "countryCode": "US"
  },
  "package": {
    "weightKg": 5,
    "dimensionsCm": {
      "length": 10,
      "width": 5,
      "height": 5
    }
  }
}
```
API calls sample response payload (standardized):
```json
[
  {
    "carrier": "FedEx",
    "rateOptions": [
      {
        "serviceName": "FedEx Ground",
        "estimatedDelivery": "2024-06-15T00:00:00",
        "price": {
          "amount": 12.34,
          "currency": "USD"
        }
      },
    ]
  },
  {
    "carrier": "DHL",
    "rateOptions": [
      {
        "serviceName": "DHL Economy Select",
        "estimatedDelivery": "2024-06-16T00:00:00",
        "price": {
          "amount": 11,
          "currency": "USD"
        }
      },
    ]
  },
  {
    "carrier": "UPS",
    "rateOptions": [
      {
        "serviceName": "UPS Ground",
        "estimatedDelivery": "2024-06-15T00:00:00",
        "price": {
          "amount": 15.2,
          "currency": "USD"
        }
      },
    ]
  }
]
```

## Author

- **Name:** Ricardo Zapanta Jr.

## Date

- June 18, 2024
