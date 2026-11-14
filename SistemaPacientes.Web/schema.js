// Generado a partir de las entidades del API. Describe cada tabla para el CRUD.
window.SCHEMA = [
 {
  "name": "Especialidad",
  "route": "especialidades",
  "label": "Especialidades",
  "labelfmt": "{nombre}",
  "fields": [
   {
    "n": "nombre",
    "t": "str",
    "label": "Nombre",
    "req": true,
    "max": 100
   },
   {
    "n": "descripcion",
    "t": "str",
    "label": "Descripción",
    "req": false,
    "max": 300
   }
  ]
 },
 {
  "name": "Seguro",
  "route": "seguros",
  "label": "Seguros (ARS)",
  "labelfmt": "{nombre}",
  "fields": [
   {
    "n": "nombre",
    "t": "str",
    "label": "Nombre",
    "req": true,
    "max": 100
   },
   {
    "n": "telefono",
    "t": "str",
    "label": "Teléfono",
    "req": false,
    "max": 20
   },
   {
    "n": "cobertura",
    "t": "dec",
    "label": "Cobertura (%)",
    "req": true,
    "min": 0,
    "maxv": 100
   }
  ]
 },
 {
  "name": "Paciente",
  "route": "pacientes",
  "label": "Pacientes",
  "labelfmt": "{nombres} {apellidos}",
  "fields": [
   {
    "n": "cedula",
    "t": "str",
    "label": "Cédula",
    "req": true,
    "max": 11
   },
   {
    "n": "nombres",
    "t": "str",
    "label": "Nombres",
    "req": true,
    "max": 100
   },
   {
    "n": "apellidos",
    "t": "str",
    "label": "Apellidos",
    "req": true,
    "max": 100
   },
   {
    "n": "fechaNacimiento",
    "t": "date",
    "label": "Fecha de nacimiento",
    "req": true
   },
   {
    "n": "sexo",
    "t": "choice",
    "label": "Sexo",
    "req": true,
    "choices": [
     "M",
     "F"
    ]
   },
   {
    "n": "telefono",
    "t": "str",
    "label": "Teléfono",
    "req": false,
    "max": 20
   },
   {
    "n": "email",
    "t": "str",
    "label": "Correo",
    "req": false,
    "max": 150
   },
   {
    "n": "direccion",
    "t": "str",
    "label": "Dirección",
    "req": false,
    "max": 200
   },
   {
    "n": "seguroId",
    "t": "fk",
    "label": "Seguro",
    "req": false,
    "fk": "seguros"
   }
  ]
 },
 {
  "name": "Medico",
  "route": "medicos",
  "label": "Médicos",
  "labelfmt": "Dr(a). {nombres} {apellidos}",
  "fields": [
   {
    "n": "nombres",
    "t": "str",
    "label": "Nombres",
    "req": true,
    "max": 100
   },
   {
    "n": "apellidos",
    "t": "str",
    "label": "Apellidos",
    "req": true,
    "max": 100
   },
   {
    "n": "exequatur",
    "t": "str",
    "label": "Exequátur",
    "req": true,
    "max": 30
   },
   {
    "n": "telefono",
    "t": "str",
    "label": "Teléfono",
    "req": false,
    "max": 20
   },
   {
    "n": "email",
    "t": "str",
    "label": "Correo",
    "req": false,
    "max": 150
   },
   {
    "n": "especialidadId",
    "t": "fk",
    "label": "Especialidad",
    "req": true,
    "fk": "especialidades"
   }
  ]
 },
 {
  "name": "Consultorio",
  "route": "consultorios",
  "label": "Consultorios",
  "labelfmt": "{codigo}",
  "fields": [
   {
    "n": "codigo",
    "t": "str",
    "label": "Código",
    "req": true,
    "max": 20
   },
   {
    "n": "ubicacion",
    "t": "str",
    "label": "Ubicación",
    "req": false,
    "max": 150
   },
   {
    "n": "piso",
    "t": "int",
    "label": "Piso",
    "req": true,
    "min": 0,
    "maxv": 50
   }
  ]
 },
 {
  "name": "Cita",
  "route": "citas",
  "label": "Citas",
  "labelfmt": "Cita #{id}",
  "fields": [
   {
    "n": "pacienteId",
    "t": "fk",
    "label": "Paciente",
    "req": true,
    "fk": "pacientes"
   },
   {
    "n": "medicoId",
    "t": "fk",
    "label": "Médico",
    "req": true,
    "fk": "medicos"
   },
   {
    "n": "consultorioId",
    "t": "fk",
    "label": "Consultorio",
    "req": true,
    "fk": "consultorios"
   },
   {
    "n": "fechaHora",
    "t": "datetime",
    "label": "Fecha y hora",
    "req": true
   },
   {
    "n": "motivo",
    "t": "str",
    "label": "Motivo",
    "req": true,
    "max": 300
   },
   {
    "n": "estado",
    "t": "choice",
    "label": "Estado",
    "req": true,
    "choices": [
     "Programada",
     "Completada",
     "Cancelada"
    ]
   }
  ]
 },
 {
  "name": "Diagnostico",
  "route": "diagnosticos",
  "label": "Diagnósticos",
  "labelfmt": "{nombre}",
  "fields": [
   {
    "n": "nombre",
    "t": "str",
    "label": "Nombre",
    "req": true,
    "max": 150
   },
   {
    "n": "codigoCie10",
    "t": "str",
    "label": "Código CIE-10",
    "req": false,
    "max": 10
   },
   {
    "n": "descripcion",
    "t": "str",
    "label": "Descripción",
    "req": false,
    "max": 300
   }
  ]
 },
 {
  "name": "Consulta",
  "route": "consultas",
  "label": "Consultas",
  "labelfmt": "Consulta #{id}",
  "fields": [
   {
    "n": "citaId",
    "t": "fk",
    "label": "Cita",
    "req": true,
    "fk": "citas"
   },
   {
    "n": "diagnosticoId",
    "t": "fk",
    "label": "Diagnóstico",
    "req": true,
    "fk": "diagnosticos"
   },
   {
    "n": "notas",
    "t": "str",
    "label": "Notas",
    "req": false,
    "max": 500
   },
   {
    "n": "costo",
    "t": "dec",
    "label": "Costo (RD$)",
    "req": true,
    "min": 0,
    "maxv": 999999999.99
   }
  ]
 },
 {
  "name": "Medicamento",
  "route": "medicamentos",
  "label": "Medicamentos",
  "labelfmt": "{nombre}",
  "fields": [
   {
    "n": "nombre",
    "t": "str",
    "label": "Nombre",
    "req": true,
    "max": 150
   },
   {
    "n": "presentacion",
    "t": "str",
    "label": "Presentación",
    "req": false,
    "max": 100
   },
   {
    "n": "stock",
    "t": "int",
    "label": "Stock",
    "req": true,
    "min": 0,
    "maxv": 1000000
   }
  ]
 },
 {
  "name": "Receta",
  "route": "recetas",
  "label": "Recetas",
  "labelfmt": "Receta #{id}",
  "fields": [
   {
    "n": "consultaId",
    "t": "fk",
    "label": "Consulta",
    "req": true,
    "fk": "consultas"
   },
   {
    "n": "fecha",
    "t": "date",
    "label": "Fecha",
    "req": true
   },
   {
    "n": "indicaciones",
    "t": "str",
    "label": "Indicaciones",
    "req": false,
    "max": 300
   }
  ]
 },
 {
  "name": "RecetaDetalle",
  "route": "recetadetalles",
  "label": "Detalle de receta",
  "labelfmt": "Detalle #{id}",
  "fields": [
   {
    "n": "recetaId",
    "t": "fk",
    "label": "Receta",
    "req": true,
    "fk": "recetas"
   },
   {
    "n": "medicamentoId",
    "t": "fk",
    "label": "Medicamento",
    "req": true,
    "fk": "medicamentos"
   },
   {
    "n": "dosis",
    "t": "str",
    "label": "Dosis",
    "req": true,
    "max": 100
   },
   {
    "n": "duracionDias",
    "t": "int",
    "label": "Duración (días)",
    "req": true,
    "min": 1,
    "maxv": 365
   }
  ]
 },
 {
  "name": "Alergia",
  "route": "alergias",
  "label": "Alergias",
  "labelfmt": "{descripcion}",
  "fields": [
   {
    "n": "pacienteId",
    "t": "fk",
    "label": "Paciente",
    "req": true,
    "fk": "pacientes"
   },
   {
    "n": "descripcion",
    "t": "str",
    "label": "Descripción",
    "req": true,
    "max": 200
   },
   {
    "n": "severidad",
    "t": "choice",
    "label": "Severidad",
    "req": true,
    "choices": [
     "Leve",
     "Moderada",
     "Grave"
    ]
   }
  ]
 }
];
