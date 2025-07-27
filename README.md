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
