# Transmetro Conecta - Auth Server

Microservicio de autenticación y transacciones para el proyecto "Transmetro Conecta". 

Desarrollado en .NET 8 bajo el enfoque de Clean Architecture, este servidor gestiona la identidad de los usuarios mediante la validación de su DPI/CUI, recuperación de accesos y la simulación de una pasarela de recarga para la billetera virtual.

---

## Requisitos Previos

Para ejecutar este proyecto de manera local, necesitas tener instalado:
* **Docker** y **Docker Compose** (Recomendado para levantar todo el ecosistema).
* **.NET 8 SDK** (Solo si deseas compilar o correr el proyecto fuera de Docker).
* **PostgreSQL** (Si no utilizas el contenedor de base de datos).

---

## Cómo Levantar el Servidor (Docker)

El proyecto está completamente dockerizado. Al iniciar el contenedor, la API aplicará automáticamente las migraciones necesarias a la base de datos PostgreSQL, creando las tablas sin intervención manual.

1. Abre tu terminal en la raíz del repositorio (`Transmetro-auth-server/`).
2. Ejecuta el siguiente comando para construir y levantar los contenedores en segundo plano:

   ```bash
   docker-compose up --build -d
    ```

3. Verifica que los contenedores estén corriendo:
    ```bash
    docker ps
    ```



La API estará disponible en `http://localhost:8080`.

---

## 📖 Documentación de la API (Swagger)

Una vez que el servidor esté en ejecución, puedes acceder a la interfaz interactiva de Swagger, la cual lee los comentarios XML del código para detallar cada endpoint:

* **URL de Swagger UI:** `http://localhost:8080/swagger`

---

## 📡 Endpoints Principales

Todos los endpoints retornan respuestas estandarizadas. En caso de error de validación, recibirás un código HTTP `400 Bad Request` con el detalle de los campos afectados gracias a FluentValidation.

### Autenticación (`/api/auth`)

* **`POST /api/auth/register`**
Crea una cuenta nueva vinculada al Documento Personal de Identificación.


* **Body requerido:** `CUI` (13 dígitos numéricos), `Email` (formato válido), `Password` (min. 6 caracteres).
* **Respuesta exitosa:** Token JWT, ID del usuario y Rol.


* **`POST /api/auth/login`**
Autentica al usuario en el sistema.
* **Body requerido:** `CUI` y `Password`.
* **Respuesta exitosa:** Token JWT para consumir endpoints protegidos.


* **`POST /api/auth/recover-password`**
Genera un token temporal de 15 minutos para la recuperación de la cuenta, garantizando que el usuario no pierda su saldo virtual.


* **Body requerido:** `Email`.
* **Respuesta exitosa:** Token de recuperación (En producción, este token se enviaría por correo electrónico).


* **`POST /api/auth/reset-password`**
Establece una nueva contraseña utilizando el token de recuperación temporal.
* **Body requerido:** `Email`, `Token` (generado en el paso anterior) y `NewPassword`.
* **Respuesta exitosa:** Confirmación de actualización.



### Transacciones y Billetera (`/api/transaction`)

* **`POST /api/transaction/recharge`** 🔒 *(Requiere Token JWT)*
Simula la pasarela de recarga acreditando saldo a la Tarjeta Ciudadana virtual mediante tarjeta de crédito/débito.


* **Headers:** `Authorization: Bearer {tu_token_jwt}`
* **Body requerido:** `CardNumber` (Validado por algoritmo de Luhn), `ExpirationDate`, `CVV` (3 o 4 dígitos), `Amount` (Mayor a 0).
* **Respuesta exitosa:** Confirmación de transacción aprobada con ID de transacción.



---

## 🏗️ Estructura del Proyecto (Clean Architecture)

El código fuente está dividido en cuatro capas principales para garantizar mantenibilidad y separación de responsabilidades:

1. **Domain:** Entidades centrales (`User`) y contratos (`IUserRepository`). No tiene dependencias externas.
2. **Application:** Lógica de negocio (`AuthService`, `TransactionService`), DTOs y validadores con FluentValidation.
3. **Infrastructure:** Acceso a datos con Entity Framework Core (PostgreSQL) y generación/validación de Tokens JWT.
4. **API:** Controladores HTTP, middlewares globales de manejo de excepciones y configuración de inyección de dependencias.