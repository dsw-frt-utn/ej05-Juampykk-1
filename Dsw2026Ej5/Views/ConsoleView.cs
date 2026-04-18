using Dsw2026Ej5.Domain;

namespace Dsw2026Ej5.Views;

public class ConsoleView
{
    private static List<VehiculoViewModel> _vehiculos = Controlador.GetVehiculos();
    public static void DibujarMenu()
    {
        string? opcion = null;
        do
        {
            LimpiarPantalla();
            DibujarLinea();
            CentrarTexto("Menú Principal - Empresa de Transporte", out int _);
            DibujarLinea();
            Console.WriteLine("Elija una opción: \n");
            Console.WriteLine("1. Listar vehículos");
            Console.WriteLine("2. Agregar vehículo");
            Console.WriteLine("3. Salir");
            Console.WriteLine("\n");
            Console.WriteLine("Ingrese su opción: ");
            opcion = Console.ReadLine();
            if (opcion == "1")
            {
                Console.WriteLine("Listando vehículos...");
                ListarVehiculos();
            }
            else if (opcion == "2")
            {
                Console.WriteLine("Agregando vehículo...");
                AgregarVehiculo();
            }
        }
        while (opcion != "3");
    }
    public static void CentrarTexto(string? texto, out int usado, int? ancho = null, bool salto = true)
    {
        texto ??= string.Empty;
        ancho ??= Console.WindowWidth;
        int largo = texto.Length;
        if (largo > ancho)
        {
            largo = ancho.Value;
            texto = texto.Substring(0, ancho.Value);
        }
        int espacios = (ancho.Value - largo) / 2;
        espacios = espacios % 2 == 0 ? espacios : espacios + 1;
        string fin = salto ? "\n" : string.Empty;
        string final = new string(' ', espacios) + texto + fin;
        Console.Write(final);
        usado = final.Length;
    }
    public static void LimpiarPantalla()
    {
        Console.Clear();
    }

    public static void DibujarLinea()
    {
        var with = Console.WindowWidth;
        for (int i = 0; i < with; i++)
        {
            Console.Write("-");
        }
    }

    private static void ListarVehiculos()
    {
        LimpiarPantalla();
        string[] columnas = { "Patente", "Vehículo", "Tipo", "Cap. Carga", "Km/l", "Año", "L.Extra", "Kms a recorrer" };
        DibujarEncabezado(columnas);
        DibjuarDatos(columnas.Length);
        DibujarLinea();
        Console.Write("\n");
        Console.Write("\n");
        Console.WriteLine("Presione una tecla para calcular el total de consumos...");
        Console.ReadLine();
        Dictionary<string, double> vehiculos = new Dictionary<string, double>();
        foreach (VehiculoViewModel vehiculo in _vehiculos)
        {
            vehiculos.Add(vehiculo.GetPatente(), vehiculo.GetKmARecorrer());
        }
        (double, double) totalConsumos = Controlador.CalcularConsumos(vehiculos);
        DibujarLinea();
        Console.WriteLine($"Total consumo Vehículos Eléctricos: {totalConsumos.Item1:F2} kWh");
        Console.WriteLine($"Total consumo Vehículos Combustible: {totalConsumos.Item2:F2} Litros");
        DibujarLinea();
        Console.Write("\n");
        Console.Write("\n");
        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadLine();
    }
    private static void DibujarEncabezado(params string[] columnas)
    {
        DibujarLinea();
        int ancho = Console.WindowWidth / columnas.Length;

        foreach (var columna in columnas)
        {
            Console.Write("|");
            CentrarTexto(columna, out int l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
        }
        Console.Write("\n");
        DibujarLinea();
    }
    private static void DibjuarDatos(int columnas)
    {
        int ancho = Console.WindowWidth / columnas;
        foreach (var vehiculo in _vehiculos)
        {
            Console.Write("|");
            CentrarTexto(vehiculo.GetPatente(), out int l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetVehiculo(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetTipo(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetCapacidadCarga().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetKmPorLitro().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetAnio().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetLitrosExtra().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.Write("|");
            CentrarTexto(vehiculo.GetKmARecorrer().ToString(), out l, ancho - 1, false);
            Console.Write("".PadRight(ancho - 1 - l));
            Console.WriteLine("|");
        }
    }

    private static void AgregarVehiculo()
    {
        LimpiarPantalla();
        DibujarLinea();
        CentrarTexto("Agregar Nuevo Vehículo", out int _);
        DibujarLinea();
        Console.WriteLine();

        try
        {
            // Tipo de vehículo
            Console.WriteLine("Tipo de vehículo:");
            Console.WriteLine("1. Eléctrico");
            Console.WriteLine("2. Combustible");
            Console.Write("Seleccione tipo (1-2): ");
            string? tipoInput = Console.ReadLine();
            if (tipoInput != "1" && tipoInput != "2")
            {
                Console.WriteLine("Tipo inválido. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }
            VehiculoTipo tipo = tipoInput == "1" ? VehiculoTipo.Electrico : VehiculoTipo.Combustible;

            // Patente
            Console.Write("Patente: ");
            string? patente = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(patente))
            {
                Console.WriteLine("Patente requerida. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            // Marca
            Console.Write("Marca: ");
            string? marca = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(marca))
            {
                Console.WriteLine("Marca requerida. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            // Modelo
            Console.Write("Modelo: ");
            string? modelo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(modelo))
            {
                Console.WriteLine("Modelo requerido. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            // Año
            Console.Write("Año: ");
            if (!int.TryParse(Console.ReadLine(), out int anio) || anio < 1900 || anio > DateTime.Now.Year + 1)
            {
                Console.WriteLine("Año inválido. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            // Capacidad de carga
            Console.Write("Capacidad de carga (kg): ");
            if (!double.TryParse(Console.ReadLine(), out double capacidadCarga) || capacidadCarga <= 0)
            {
                Console.WriteLine("Capacidad de carga inválida. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            // Sucursal
            Console.WriteLine("Sucursales disponibles:");
            var sucursales = Controlador.GetSucursales();
            foreach (var suc in sucursales)
            {
                Console.WriteLine($"{suc.GetCodigo()} - {suc.GetCiudad()}");
            }
            Console.Write("Código de sucursal: ");
            string? sucursalCodigo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(sucursalCodigo) || !sucursales.Any(s => s.GetCodigo() == sucursalCodigo))
            {
                Console.WriteLine("Sucursal inválida. Presione una tecla para continuar...");
                Console.ReadLine();
                return;
            }

            double parametro1 = 0, parametro2 = 0;

            if (tipo == VehiculoTipo.Electrico)
            {
                // kWh base
                Console.Write("Consumo base (kWh por km): ");
                if (!double.TryParse(Console.ReadLine(), out parametro1) || parametro1 <= 0)
                {
                    Console.WriteLine("Consumo base inválido. Presione una tecla para continuar...");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                // Km por litro
                Console.Write("Kilómetros por litro: ");
                if (!double.TryParse(Console.ReadLine(), out parametro1) || parametro1 <= 0)
                {
                    Console.WriteLine("Kilómetros por litro inválidos. Presione una tecla para continuar...");
                    Console.ReadLine();
                    return;
                }

                // Litros extra
                Console.Write("Litros extra: ");
                if (!double.TryParse(Console.ReadLine(), out parametro2) || parametro2 < 0)
                {
                    Console.WriteLine("Litros extra inválidos. Presione una tecla para continuar...");
                    Console.ReadLine();
                    return;
                }
            }

            // Agregar el vehículo
            Controlador.AgregarVehiculo(tipo, patente, marca, modelo, anio, capacidadCarga, sucursalCodigo, parametro1, parametro2);

            // Actualizar la lista local
            _vehiculos = Controlador.GetVehiculos();

            Console.WriteLine("Vehículo agregado exitosamente. Presione una tecla para continuar...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar vehículo: {ex.Message}. Presione una tecla para continuar...");
            Console.ReadLine();
        }
    }
}
