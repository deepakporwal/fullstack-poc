# Full Stack Development POC

React + .NET 8 Web API + MongoDB + JWT + Docker Compose.

## Architecture

Browser -> Nginx/React (3000) -> .NET API (8080) -> MongoDB (27017)

## Features
- JWT register/login authentication
- BCrypt password hashing
- `[Authorize]` protected product APIs
- Global exception handling middleware returning JSON + trace ID
- Structured ASP.NET logging for API errors and important events
- Swagger API documentation
- MongoDB persistence with Docker volume
- React frontend served by Nginx
- Nginx reverse proxy for `/api`

## Run everything

```bash
docker compose up --build
```

Open http://localhost:3000

Swagger: http://localhost:8080/swagger
Health: http://localhost:8080/health

## API endpoints
- POST `/api/auth/register`
- POST `/api/auth/login`
- GET `/api/products` (JWT required)
- GET `/api/products/{id}` (JWT required)
- POST `/api/products` (JWT required)
- DELETE `/api/products/{id}` (JWT required)

## Test login
Default demo values are pre-filled in the UI:
`demo@example.com` / `Password@123`

Register first, then use the product dashboard.

## Logs

```bash
docker compose logs -f api
```

The global middleware logs unhandled exceptions with request path, method and trace ID. For production, replace console logging with Serilog/OpenTelemetry and ship logs to CloudWatch, ELK, Application Insights, etc.

## Security notes
The JWT key in compose is intentionally a demo value. Use Docker secrets or an environment/secret manager in real deployments. HTTPS, refresh tokens, rate limiting, validation, role-based authorization, and secret rotation should be added before production use.
