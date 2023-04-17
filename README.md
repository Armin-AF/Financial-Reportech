# Financial Reportech

Financial Reportech is a finance website that fetches financial data of Swedish tech companies and provides a daily analysis using Yahoo Finance API and OpenAI's ChatGPT API. The backend is built using ASP.NET Core, and the frontend is developed using Next.js.

## Prerequisites

- Node.js (>= 12.0.0)
- .NET SDK (>= 5.0)

##Getting Started

To set up the project, follow these steps:

- Clone the repository:
```bash
git clone https://github.com/yourusername/financial-reportech.git
```

- Navigate to the backend directory and restore packages:
```bash
cd financial-reportech/FinanceBackend
dotnet restore
```

- Navigate to the frontend directory and install dependencies:

```bash
cd ../FinanceFrontend
npm install
```

- Add your API keys for Yahoo Finance API and ChatGPT API in the appropriate configuration files.

## Running the Project

- Start the backend:
```arduino
cd financial-reportech/FinanceBackend
dotnet run
```

- Start the frontend (in a separate terminal):
```arduino
cd financial-reportech/FinanceFrontend
npm run dev
```

- Open your browser and navigate to http://localhost:3000 to view the application.

## Contributing

If you'd like to contribute to the project, please submit a pull request with your proposed changes or open an issue to discuss your ideas.

## License

This project is licensed under the MIT License.
