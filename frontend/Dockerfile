# ============================================================
# Stage 1: Build the Angular application
# ============================================================
FROM node:22-alpine AS build

WORKDIR /app

# Copy dependency manifests first for better caching
COPY package.json package-lock.json ./

# Install dependencies
RUN npm ci

# Copy the rest of the source code
COPY . .

# Build the application (production configuration)
RUN npm run build

# ============================================================
# Stage 2: Serve the application with nginx
# ============================================================
FROM nginx:alpine AS final

# Copy the nginx server configuration
COPY nginx.conf /etc/nginx/templates/default.conf.template

# Copy the built application
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# Expose the port nginx listens on
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
