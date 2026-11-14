using SistemaPacientes.Domain.Entities;

namespace SistemaPacientes.Infrastructure.Context;

// Datos iniciales de ejemplo para poder probar el sistema.
public static class DataSeeder
{
    public static void Seed(SistemaPacientesContext db)
    {
        if (db.Especialidades.Any()) return;

        db.Especialidades.AddRange(
            new Especialidad { Nombre = "Medicina general", Descripcion = "Atención primaria" },
            new Especialidad { Nombre = "Pediatría", Descripcion = "Salud infantil" },
            new Especialidad { Nombre = "Cardiología", Descripcion = "Corazón y circulación" });
        db.Seguros.AddRange(
            new Seguro { Nombre = "ARS Ejemplo Salud", Telefono = "809-555-0100", Cobertura = 80 },
            new Seguro { Nombre = "Plan Básico", Telefono = "809-555-0101", Cobertura = 60 });
        db.Consultorios.AddRange(
            new Consultorio { Codigo = "C-101", Ubicacion = "Primer piso", Piso = 1 },
            new Consultorio { Codigo = "C-201", Ubicacion = "Segundo piso", Piso = 2 });
        db.Diagnosticos.AddRange(
            new Diagnostico { Nombre = "Hipertensión arterial", CodigoCie10 = "I10" },
            new Diagnostico { Nombre = "Faringitis aguda", CodigoCie10 = "J02" });
        db.Medicamentos.AddRange(
            new Medicamento { Nombre = "Paracetamol", Presentacion = "Tabletas 500 mg", Stock = 200 },
            new Medicamento { Nombre = "Amoxicilina", Presentacion = "Cápsulas 500 mg", Stock = 120 });
        db.SaveChanges();
    }
}
