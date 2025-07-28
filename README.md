Meta-Exchange API
A .NET Core Web API that finds the best execution price for buying or selling Bitcoin across multiple cryptocurrency exchanges, taking into account balance constraints and market liquidity.
🚀 Features

Smart Order Routing: Automatically finds the optimal combination of orders across multiple exchanges
Balance Constraints: Respects EUR and BTC balance limitations for each exchange
Best Price Execution: Minimizes cost for buy orders and maximizes revenue for sell orders
RESTful API: Clean, well-documented endpoints with comprehensive error handling
Swagger Integration: Interactive API documentation and testing interface
Docker Support: Easy containerized deployment
Unit Testing: Comprehensive test coverage with mocking

<img width="1378" height="863" alt="image" src="https://github.com/user-attachments/assets/0a617b31-f293-4838-9290-22a429e2ba1e" />

Key Components

Exchange: Represents a cryptocurrency exchange with balance constraints and order book data
OrderBook: Contains market data (bids and asks) from an exchange
BestExecutionService: Core algorithm for finding optimal execution across exchanges
OrderBookService: Handles order book data processing

📦 Installation & Setup
Prerequisites

.NET 8.0 SDK
Docker Desktop (for containerized deployment)
Visual Studio 2022


🐳 Docker Deployment
Build and Run with Docker

Build the Docker image
bashdocker build -t meta-exchange-api .

Run the container
bashdocker run -d -p 8080:8080 --name meta-exchange-container meta-exchange-api

Access the API
http://localhost:8080/swagger


🧮 Algorithm Explanation
For Buy Orders (User wants to purchase BTC):

Collect all asks (sell orders) from all exchanges
Apply balance constraints - filter by available BTC on each exchange
Sort by price (lowest first) to minimize total cost
Aggregate orders until the desired amount is filled
Return execution plan with minimum total cost

For Sell Orders (User wants to sell BTC):

Collect all bids (buy orders) from all exchanges
Apply balance constraints - filter by available EUR on each exchange
Sort by price (highest first) to maximize total revenue
Aggregate orders until the desired amount is filled
Return execution plan with maximum total revenue

Example Scenario
Goal: Buy 2.0 BTC at the best possible price
Available Asks:

Exchange A: 0.5 BTC @ €2,964.29
Exchange B: 0.8 BTC @ €2,965.00
Exchange A: 1.2 BTC @ €2,966.40

Algorithm Result:

Take 0.5 BTC from Exchange A @ €2,964.29 = €1,482.15
Take 0.8 BTC from Exchange B @ €2,965.00 = €2,372.00
Take 0.7 BTC from Exchange A @ €2,966.40 = €2,076.48
Total: 2.0 BTC for €5,930.63 (average: €2,965.32/BTC)

Test Input JSON (Buy Operation):

{
  "orderType": "Buy",
  "amount": 0.5,
  "exchanges": [
    {
      "exchangeId": "Binance",
      "balance": {
        "eur": 10000.0,
        "btc": 5.0
      },
      "orderBook": {
        "acqTime": "2019-01-29T11:00:00.2518854Z",
        "bids": [],
        "asks": [
          {
            "order": {
              "id": "ask_1",
              "time": "2019-01-29T10:55:00Z",
              "type": "Sell",
              "kind": "Limit",
              "amount": 0.3,
              "price": 2960.00
            }
          },
          {
            "order": {
              "id": "ask_2", 
              "time": "2019-01-29T10:56:00Z",
              "type": "Sell",
              "kind": "Limit",
              "amount": 0.4,
              "price": 2965.00
            }
          }
        ]
      }
    },
    {
      "exchangeId": "Coinbase",
      "balance": {
        "eur": 8000.0,
        "btc": 3.0
      },
      "orderBook": {
        "acqTime": "2019-01-29T11:00:00.1234567Z",
        "bids": [],
        "asks": [
          {
            "order": {
              "id": "ask_3",
              "time": "2019-01-29T10:54:00Z", 
              "type": "Sell",
              "kind": "Limit",
              "amount": 0.2,
              "price": 2958.00
            }
          },
          {
            "order": {
              "id": "ask_4",
              "time": "2019-01-29T10:57:00Z",
              "type": "Sell", 
              "kind": "Limit",
              "amount": 0.5,
              "price": 2962.00
            }
          }
        ]
      }
    }
  ]
}

Expected Output JSON:

{
  "success": true,
  "data": {
    "orders": [
      {
        "exchangeId": "Coinbase",
        "type": "Buy",
        "amount": 0.2,
        "price": 2958.0,
        "cost": 591.6
      },
      {
        "exchangeId": "Binance", 
        "type": "Buy",
        "amount": 0.3,
        "price": 2960.0,
        "cost": 888.0
      }
    ],
    "totalAmount": 0.5,
    "totalCost": 1479.6,
    "averagePrice": 2959.2,
    "isFullyExecuted": true,
    "message": "Fully executed."
  },
  "message": "Best execution plan calculated successfully"
}

Test Input JSON (Sell Operation):

{
  "orderType": "Sell",
  "amount": 1.0,
  "exchanges": [
    {
      "exchangeId": "Exchange-A",
      "balance": {
        "eur": 8000.0,
        "btc": 5.0
      },
      "orderBook": {
        "acqTime": "2019-01-29T11:00:00.2518854Z",
        "bids": [
          {
            "order": {
              "id": "bid_1",
              "time": "2019-01-29T10:55:00Z",
              "type": "Buy",
              "kind": "Limit",
              "amount": 0.4,
              "price": 2950.00
            }
          },
          {
            "order": {
              "id": "bid_2",
              "time": "2019-01-29T10:56:00Z",
              "type": "Buy",
              "kind": "Limit",
              "amount": 0.3,
              "price": 2945.00
            }
          }
        ],
        "asks": []
      }
    },
    {
      "exchangeId": "Exchange-B",
      "balance": {
        "eur": 12000.0,
        "btc": 3.0
      },
      "orderBook": {
        "acqTime": "2019-01-29T11:00:00.1234567Z",
        "bids": [
          {
            "order": {
              "id": "bid_3",
              "time": "2019-01-29T10:54:00Z",
              "type": "Buy",
              "kind": "Limit",
              "amount": 0.6,
              "price": 2955.00
            }
          },
          {
            "order": {
              "id": "bid_4",
              "time": "2019-01-29T10:57:00Z",
              "type": "Buy",
              "kind": "Limit",
              "amount": 0.2,
              "price": 2952.00
            }
          }
        ],
        "asks": []
      }
    }
  ]
}

Expected Output JSON (Sell Operation):

{
  "success": true,
  "data": {
    "orders": [
      {
        "exchangeId": "Exchange-B",
        "type": "Sell",
        "amount": 0.6,
        "price": 2955.00,
        "cost": 1773.00
      },
      {
        "exchangeId": "Exchange-B", 
        "type": "Sell",
        "amount": 0.2,
        "price": 2952.00,
        "cost": 590.40
      },
      {
        "exchangeId": "Exchange-A",
        "type": "Sell",
        "amount": 0.2,
        "price": 2950.00,
        "cost": 590.00
      }
    ],
    "totalAmount": 1.0,
    "totalCost": 2953.40,
    "averagePrice": 2953.40,
    "isFullyExecuted": true,
    "message": "Fully executed."
  },
  "message": "Best execution plan calculated successfully"
}


