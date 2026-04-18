using Dsw2026Ej5.Data;
using Dsw2026Ej5.Domain;

namespace Dsw2026Ej5.Views;

public class Controlador
{
    public static List<VehiculoViewModel> GetVehiculos()
    {
        List<VehiculoViewModel> vehiculos = new List<VehiculoViewModel>();
        foreach (Vehiculo vehiculo in Persistencia.GetVehiculos())
        {
            vehiculos.Add(new VehiculoViewModel(vehiculo));
        }
        return vehiculos;
    }

    public static List<Sucursal> GetSucursales()
    {
        return Persistencia.GetSucursales();
    }

    public static (double, double) CalcularConsumos(Dictionary<string, double> vehiculos)
    {
        double consumoElectricos = 0;
        double consumoCombustible = 0;
        foreach (KeyValuePair<string, double> entry in vehiculos)
        {
            double consumo = 0;
            Vehiculo? vehiculo = Persistencia.GetVehiculo(entry.Key);
            if (vehiculo != null)
            {
                consumo = vehiculo.CalcularConsumo(entry.Value);
                consumoElectricos += vehiculo.EsDe(VehiculoTipo.Electrico) ? consumo : 0;
                consumoCombustible += vehiculo.EsDe(VehiculoTipo.Combustible) ? consumo : 0;
            }
        }
        return (consumoElectricos, consumoCombustible);
    }

    public static void AgregarVehiculo(VehiculoTipo tipo, string patente, string marca, string modelo, int anio, 
        double capacidadCarga, string sucursalCodigo, double parametro1, double parametro2 = 0)
    {
        Sucursal? sucursal = Persistencia.GetSucursales().Find(s => s.GetCodigo() == sucursalCodigo);
        if (sucursal == null)
        {
            throw new ArgumentException("Sucursal no encontrada");
        }

        Vehiculo vehiculo;
        if (tipo == VehiculoTipo.Electrico)
        {
            vehiculo = new VehiculoElectrico(patente, marca, modelo, anio, capacidadCarga, sucursal, parametro1);
        }
        else
        {
            // For combustible: parametro1 = kmPorLitro, parametro2 = litrosExtra
            vehiculo = new VehiculoCombustible(patente, marca, modelo, anio, capacidadCarga, sucursal, parametro1, parametro2);
        }

        Persistencia.AgregarVehiculo(vehiculo);
    }
}
