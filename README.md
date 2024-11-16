# Tokero Crypto Calculator

Tokero Crypto Calculator is a web application that helps users simulate a portfolio resulting from monthly cryptocurrency investments using the Dollar Cost Averaging (DCA) strategy. The application allows users to track their investments, calculate returns, and visualize simulated portfolios.

## Features
- ✅ Monthly cryptocurrency investments simulation  
- ✅ Tracks portfolio value over time  
- ✅ Supports multiple cryptocurrencies (e.g., Bitcoin, Ether, etc.)  
- ✅ Coin Market Cap API integration for retrieving ALL available currencies (+5000) -> that are stored in the SQL db and cached for a week
- ❌ Interactive charts and projections for portfolio growth  

## Prerequisites
- .NET 8 or higher
- SQL Server (for storing investment data)

## Setup Instructions
1. Clone this repository: git clone https://github.com/vlad-zinca/Tokero.CryptoCalculator.git
2. Open the solution file in Visual Studio.
3. Restore NuGet packages.
4. Set up the database using the provided SQL script (`TokeroDbScript.sql`). -- I added the database backup to this just in case is needed.
5. Change `DefaultConnection` in `appsettings.json` to appropriate value.
6. Make sure that the `Tokero.CryptoCalculator.Web` project is set up as the startup project.
7. **IMPORTANT:** In the `CoinMarketCapService.cs` file, there is an `API_KEY` hardcoded. This key belongs to a demo account created for calling the CoinMarketCap API. If you encounter any issues with the key (e.g., permissions or credit expiration), **please use your own key**!
8. Build and run the application locally.

## Usage
Once the application is running, you can:
- Input your investment amount.
- Choose cryptocurrencies to invest in.
- View portfolio projections over time.

## Contributing
Feel free to fork this repository and submit pull requests. Contributions are welcome!

## License
This project is licensed under the MIT License.
