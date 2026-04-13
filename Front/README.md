# Módulo RPA (Frontend)

SPA en Vue 3 + Vuetify para:
- Monitoreo de automatizaciones en Grafana (iframe).
- Gestión de automatizaciones (UI): crear/editar, activar/desactivar, ejecutar manual.

## Rutas
- `/rpa`: monitoreo (Grafana).
- `/rpa/automatizaciones`: gestión.

## Configuración
- `VITE_GRAFANA_EMBED_URL` (opcional): URL del dashboard a embeber.
  - Si no se define, se usa un fallback en código.
- `VITE_API_BASE_URL` (opcional): base URL del backend.
  - Si no se define, usa rutas relativas (mismo host).

## Backend (API)
El frontend espera estos endpoints:
- `GET /api/rpa/automatizaciones`
- `POST /api/rpa/automatizaciones`
- `PUT /api/rpa/automatizaciones/{id}`
- `PATCH /api/rpa/automatizaciones/{id}/activo` body: `{ "activo": true|false }`
- `POST /api/rpa/automatizaciones/{id}/ejecutar`
- `DELETE /api/rpa/automatizaciones/{id}`

## Scripts
```bash
npm install
npm run dev
npm run build
```

## Docker
```bash
docker build -t rpa-frontend .
docker run --rm -p 8080:80 rpa-frontend
```
