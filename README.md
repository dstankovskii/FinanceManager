# Finance Manager

Finance Manager is a web-based application for managing personal or corporate finances, built with ASP.NET Core and PostgreSQL.

## Features

- Track income and expenses
- User and role management
- Generate financial reports
- RESTful API for integration
- Role-based authorization

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/dstankovskii/FinanceManager.git
   cd FinanceManager
   ```

2. **Configure the database**:

   Update the connection string in `appsettings.json` with your PostgreSQL credentials.

3. **Build and run the project**:

   ```bash
   dotnet build
   dotnet run
   ```

4. **Apply database migrations**:

   ```bash
   dotnet ef database update
   ```

5. **Test the API**:

   Go to `https://localhost:5001/api/FinanceManager` to access the API.

api/clent/create
```json
"dd6dacd5-cda3-4f6a-b865-d17fdc88fae1"
```

api/transaction/credit
```json
{
   "id": "7bb1fb24-e323-4287-83a4-eef97eae853b",
   "clientId": "dd6dacd5-cda3-4f6a-b865-d17fdc88fae1",
   "dateTime": "2019-04-02T13:10:20.0263632+03:00",
   "amount": 23.05
}
```

api/transaction/debit
```json
{
   "id": "a790eb6b-43fd-465e-b2b8-5bf434180d9f",
   "clientId": "dd6dacd5-cda3-4f6a-b865-d17fdc88fae1",
   "dateTime": "2024-10-27T20:00:52.953Z",
   "amount": 23.05
}
  ```

api/transaction/revert
```json
"a790eb6b-43fd-465e-b2b8-5bf434180d9f"
```

api/clent/balance
```json
"dd6dacd5-cda3-4f6a-b865-d17fdc88fae1"
```

## Contributing

Feel free to submit issues or pull requests if you have ideas or find bugs!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.