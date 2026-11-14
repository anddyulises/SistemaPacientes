# Sistema de Pacientes

CRUD de un sistema de pacientes con un **API en .NET** (N capas, con interfaces) y un **cliente web en JavaScript puro** que lo consume.

## Arquitectura en capas

| Proyecto | Capa | Contenido |
|---|---|---|
| `SistemaPacientes.Domain` | Dominio | `Core/BaseEntity`, 12 entidades en `Entities/`, contrato `Repository/IBaseRepository<T>` |
| `SistemaPacientes.Infrastructure` | Infraestructura | `Context` (EF Core + SQLite, relaciones y datos iniciales), `Core/BaseRepository<T>`, `Interfaces/I…Repository`, `Repositories`, `Exceptions` |
| `SistemaPacientes.Application` | Servicios | `Contract/I…Service`, `Core` (ServiceResult, BaseService), `Dtos` (Add / Update / Dto por entidad), `Services` con validación de cada campo |
| `SistemaPacientes.Api` | Presentación (Web API) | 12 controllers REST, `Program.cs` (inyección de dependencias, CORS, Swagger), `appsettings.json` |
| `SistemaPacientes.Web` | Cliente | HTML + CSS + JS puro que consume el API (sin MVC) |

Flujo: `Controller → I…Service → I…Repository → DbContext`. Todo se inyecta por interfaces.

## Tablas (12)

Especialidades, Seguros, Pacientes, Médicos, Consultorios, Citas, Diagnósticos, Consultas, Medicamentos, Recetas, RecetaDetalles, Alergias. Con llaves foráneas entre ellas (por ejemplo Cita → Paciente, Médico y Consultorio; RecetaDetalle → Receta y Medicamento). El borrado es lógico (`Deleted`).

## Cómo ejecutarlo

Requiere el SDK de .NET (el proyecto usa `net11.0`).

```bash
# 1) API (queda en http://localhost:5080, Swagger en /swagger)
dotnet run --project SistemaPacientes.Api

# 2) Cliente web (en otra terminal)
cd SistemaPacientes.Web
python3 -m http.server 5500
# abrir http://localhost:5500
```

La base de datos SQLite (`sistemapacientes.db`) se crea sola al iniciar el API, con algunos datos de ejemplo. Si cambia el puerto del API, ajuste `SistemaPacientes.Web/config.js`.

## Endpoints

Para cada tabla (`especialidades`, `seguros`, `pacientes`, `medicos`, `consultorios`, `citas`, `diagnosticos`, `consultas`, `medicamentos`, `recetas`, `recetadetalles`, `alergias`):

`GET /api/{tabla}` · `GET /api/{tabla}/{id}` · `POST /api/{tabla}` · `PUT /api/{tabla}/{id}` · `DELETE /api/{tabla}/{id}`

Todas las respuestas tienen la forma `{ success, notFound, message, data }`.

## Validaciones (ejemplos)

Campos obligatorios y largos máximos, cédula de 11 dígitos y única, exequátur único, correo y teléfono con formato válido, fecha de nacimiento no futura, rangos numéricos, valores permitidos (sexo, estado de la cita, severidad) y verificación de que las llaves foráneas existan.
