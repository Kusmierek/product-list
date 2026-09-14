# Product Catalog

Full-stack product catalog app — .NET 9 Minimal API + Angular.

## Wymagania

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)

## Backend

```bash
cd ProductCatalog.API
dotnet run
```

API działa na `http://localhost:5000`.

## Frontend

```bash
cd product-catalog-app
npm install
npm start
```

Aplikacja działa na `http://localhost:4200`.

## Docker

```bash
docker-compose up --build
```

- Frontend: `http://localhost:4200`
- API: `http://localhost:5000`

nginx proxy'uje `/api` do kontenera backendu Angular nie potrzebuje publicznego adresu API.

## Testy (BE)

```bash
cd ProductCatalog.Tests
dotnet test
```
