# API de Gestión de Productos y Stock

**GestiónProducto** es una API backend desarrollada en .NET 10 para gestionar productos, usuarios, roles y movimientos de stock con control de seguridad, validación y transacciones.

## 🚀 Qué hace
- Autenticación JWT con roles y control de acceso (`Admin`, `Vendedor`, etc.)
- Registro y login de usuarios
- Gestión de productos: crear, actualizar, eliminar, activar y consultar
- Gestión de movimientos de stock: entradas y salidas con control de stock disponible
- Uso de un middleware de manejo global de errores
- Documentación automática con Swagger

## 🧱 Arquitectura
- `Controllers` → Endpoints HTTP REST
- `Application/Services` → Lógica de negocio
- `Infra/Repositories` → Persistencia de datos con Entity Framework Core
- `Infra/Persistence` → DbContext, migraciones y configuración del modelo
- `DTOs` → Transferencia de datos entre API y servicios

## 🔐 Seguridad y buenas prácticas
- JWT para autenticación y autorización
- Bcrypt para hash seguro de contraseñas
- Rate limiting en login para evitar ataques de fuerza bruta
- User Secrets / variables de entorno para datos sensibles
- Validación con FluentValidation
- Transacciones al registrar movimientos para mantener consistencia de stock

## 💡 Tecnologías usadas
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core con SQL Server
- FluentValidation
- JWT Bearer Authentication
- Swagger / OpenAPI
- BCrypt.Net

## ✅ Logros técnicos clave
- Refactorización del dominio para separar negocio y persistencia
- Ajuste de stock dentro del servicio, no en el repositorio
- Manejo de DTOs para respuestas más limpias y desacopladas
- Configuración de secretos en entorno local con `UserSecretsId`
- Añadida protección contra errores y validación en capa de servicio

## 📌 Por qué este proyecto es relevante para mi portfolio
Este proyecto muestra mi capacidad para:
- diseñar una API backend escalable
- aplicar buenas prácticas de seguridad
- trabajar con C# y EF Core
- construir una arquitectura limpia y fácil de mantener

## 🛠️ Cómo ejecutar
1. Clona el repositorio
2. Configura los secrets locales
3. Ejecuta `dotnet build ApiProductosStock.sln`
4. Ejecuta `dotnet run --project GestionProducto/GestionProducto.csproj`
5. Abre `https://localhost:5001/swagger` para probar los endpoints

## 📈 ¿Qué sigue?
Me gustaría continuar agregando:
- pruebas unitarias e integración
- endpoints de administración de roles más completos
- mejor separación entre `Domain` y `Models`
- integración con un servicio de secretos en producción
