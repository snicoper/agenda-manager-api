# Agenda Manager - API

## 🌟 Descripción

Agenda Manager API es el backend que gestiona la lógica de negocio para la administración de citas y recursos. Desarrollada en **.NET Core**, sigue principios de **Domain-Driven Design (DDD)**, **Clean Architecture (CA)** y **CQRS**, asegurando una separación clara de responsabilidades, mantenibilidad y extensibilidad.

## 🔎 Tecnologías Utilizadas

- **.NET Core 9** - Framework principal
- **Entity Framework Core** - ORM para acceso a datos
- **CQRS** - Separación entre comandos y consultas
- **MediatR** - Manejo de eventos y patrones de mensajería
- **FluentValidation** - Validaciones robustas para comandos y consultas
- **JWT** - Autenticación basada en tokens
- **Serilog** - Logging estructurado
- **Docker** - Contenedorización del entorno
- **RabbitMQ** - Sistema de mensajería para eventos asincrónicos
- **Outbox Pattern** - Persistencia y publicación robusta de eventos
- **Newtonsoft.Json** - Serialización de eventos con ValueObjects en Outbox

## 🛠️ Arquitectura del Proyecto

El proyecto sigue **Clean Architecture**, separando responsabilidades en diferentes capas:

```
📦 src/
 ┣ 📂 Application/       # Casos de uso (CQRS, Validaciones, MediatR)
 ┣ 📂 Domain/            # Entidades, reglas de negocio, ValueObjects (DDD)
 ┣ 📂 Infrastructure/    # Persistencia, Autenticación, Integraciones externas, Outbox, RabbitMQ
 ┣ 📂 WebApi/            # Controladores, Middlewares, Configuraciones
```

## 🔧 Configuración y Ejecución

Para correr la API localmente:

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/snicoper/agenda-manager-api.git
   cd agenda-manager-api
   ```

2. Configurar variables de entorno (**si aplica**).

3. Levantar los servicios con Docker:

   ```bash
   docker-compose up -d
   ```

   Esto iniciará los siguientes contenedores:

   - **agenda-manager-api** (Backend de la aplicación)
   - **agenda-manager-db** (PostgreSQL)
   - **agenda-manager-seq** (Logging con Serilog + Seq)
   - **agenda-manager-jaeger** (Tracing con OpenTelemetry + Jaeger)
   - **agenda-manager-rabbitmq** (Broker de eventos RabbitMQ)

4. Para detener los contenedores:

   ```bash
   docker-compose down
   ```

5. Ejecutar la API manualmente (si se desea evitar Docker para desarrollo):

   ```bash
   dotnet run --project WebApi
   ```

## 💌 Documentación y Endpoints

- **Swagger:** `https://localhost:7000/swagger`
- **Documentación:** `https://github.com/snicoper/agenda-manager-docs`
- **Colección Bruno:** `https://github.com/snicoper/agenda-manager-bruno`
- **SPA:** `https://github.com/snicoper/agenda-manager-web`

## ✅ Roadmap y Mejoras Pendientes

- [ ] Revisar todos los `DomainErrors`
- [ ] Revisar todos los `DomainEvents`
- [ ] Implementar tests de integración para validaciones complejas
- [ ] Mejorar resiliencia del Outbox (retries, DLQ, logs detallados)
- [ ] Añadir auto-discovery de eventos en dispatcher por assembly scan
- [ ] Añadir documentación sobre patrón Outbox + RabbitMQ
