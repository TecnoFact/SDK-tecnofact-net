using TecnoFact.SDK.Config;
using TecnoFact.SDK.Enums;
using TecnoFact.SDK.Models;
using TecnoFact.SDK.Http;

namespace TecnoFact.SDK.Examples;

/// <summary>
/// Ejemplo básico de uso del SDK de TecnoFact
/// </summary>
public class BasicUsageExample
{
    public static void Main()
    {
        // 1. Crear configuración
        var config = new TecnoFactConfig(
            apiKey: "TU_API_KEY",
            apiSecret: "TU_API_SECRET",
            environment: TecnoFactEnvironment.Sandbox,
            timeout: 30,
            retries: 3
        );

        Console.WriteLine($"Entorno: {config.GetEnvironment().Label()}");
        Console.WriteLine($"URL Base: {config.GetBaseUrl()}");

        // 2. Crear datos del emisor
        var emisor = new Emisor(
            rfc: "XAXX010101000",
            nombre: "EMPRESA EMISORA SA DE CV",
            regimenFiscal: "601",
            cp: "06300"
        );

        // 3. Crear datos del receptor
        var receptor = new Receptor(
            rfc: "XAXX010101001",
            nombre: "CLIENTE RECEPTOR",
            usoCfdi: "G03",
            domicilioFiscalReceptor: "06300",
            regimenFiscalReceptor: "612"
        );

        // 4. Crear concepto con impuestos
        var concepto = new Concepto(
            claveProdServ: "01010101",
            cantidad: 1m,
            claveUnidad: "E48",
            descripcion: "Servicio de desarrollo de software",
            valorUnitario: 10000.00m,
            importe: 10000.00m,
            objetoImp: "02",
            impuestos: new ImpuestosConcepto(
                traslados: new List<Traslado>
                {
                    new Traslado(
                        baseImporte: 10000.00m,
                        impuesto: "002",
                        tipoFactor: "Tasa",
                        tasaOCuota: 0.160000m,
                        importe: 1600.00m
                    )
                }
            )
        );

        // 5. Crear solicitud CFDI 4.0
        var request = new Cfdi4Request(
            emisor: emisor,
            receptor: receptor,
            conceptos: new List<Concepto> { concepto }
        )
        {
            TipoComprobante = "I",
            FormaPago = "01",
            MetodoPago = "PUE",
            Moneda = "MXN",
            Subtotal = 10000.00m,
            Total = 11600.00m,
            Impuestos = new Impuestos(
                totalImpuestosTrasladados: 1600.00m,
                traslados: new List<TrasladoGlobal>
                {
                    new TrasladoGlobal(
                        impuesto: "002",
                        tipoFactor: "Tasa",
                        tasaOCuota: 0.160000m,
                        importe: 1600.00m
                    )
                }
            )
        };

        // 6. Convertir a diccionario para enviar
        var requestDict = request.ToDictionary();
        Console.WriteLine("Solicitud CFDI creada exitosamente");
        Console.WriteLine($"Total: ${request.Total:N2}");

        // 7. Crear cliente HTTP (para futuras peticiones)
        using var httpClient = new TecnoFactHttpClient(config);
        Console.WriteLine("Cliente HTTP creado y listo para usar");
    }
}
