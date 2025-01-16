using Xunit;
using LeadTrackApi.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadTrack.API.Test;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using LeadTrackApi.Domain.Models;
using LeadTrackApi.Application.Extensions;

namespace LeadTrackApi.Application.Utils.Tests
{
    public class TextFileReaderTests(TestConfiguration testConfiguration, ITestOutputHelper console) : IClassFixture<TestConfiguration>
    {

        [Fact()]
        public void ReadFileTest()
        {
            var schema = new FileSchema()
            {
                Type = "fixed_length", // o "fixed_length"
                //Delimiter = ',',   // Solo para archivos delimitados
                //HeaderFields =
                //[
                //    new () { Name = "file_id", Type = "int", Length = 5 },
                //    new () { Name = "creation_date", Type = "date", Format = "yyyy-MM-dd" }
                //],
                BodyFields =
                [
                    new () { Name = "fecha_proceso", Type = "date", Length=6, Format="yyyyMM" },
                    new () { Name = "cod_descuento", Type = "int", Length = 6 },
                    new () { Name = "rut", Type = "int", Length = 10 },
                    new () { Name = "dv", Type = "string", Length = 1 },
                    new () { Name = "filler", Type = "string", Length = 13 },
                    new () { Name = "monto_descuento", Type = "decimal", Length = 12 },
                    new () { Name = "fecha_vencimiento", Type = "date", Format = "yyyyMM", Length= 6 },
                    new () { Name = "tipo_descuento", Type = "string", Length = 20 },

                ]
            };

            console.WriteLine("Schema JSON:");
            console.WriteLine(schema.Dump());

            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "CAEP_202408.txt");
            var reader = new TextFileReader(schema);
            var (header, body) = reader.ReadFile(filename);

            // Mostrar la cabecera
            console.WriteLine("Cabecera:");
            console.WriteLine(header.Dump());

            // Mostrar el cuerpo
            console.WriteLine("\nCuerpo:");
            body.ForEach(e => console.WriteLine(e.Dump(false)));
        }
    }
}