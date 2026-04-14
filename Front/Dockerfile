FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .

# Argumentos para la build de Vite
ARG VITE_API_BASE_URL
ARG VITE_GRAFANA_EMBED_URL

# Exponer variables de entorno para la build
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
ENV VITE_GRAFANA_EMBED_URL=$VITE_GRAFANA_EMBED_URL

RUN npm run build

FROM nginx:1.27-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
