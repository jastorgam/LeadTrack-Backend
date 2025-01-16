using LeadTrack.API.Test;
using Xunit.Abstractions;
using LeadTrackApi.Domain.Models;
using LeadTrackApi.Application.Extensions;

namespace LeadTrackApi.Application.Utils.Tests;

public class ExcelFileReaderTests(TestConfiguration testConfiguration, ITestOutputHelper console) : IClassFixture<TestConfiguration>
{
    [Fact()]
    public void ReadFileTest()
    {
        var schema = new FileSchema()
        {
            Type = "excel",
            BodyFields =
                 [
                    new () { Name = "nombre", Type = "string" },
                    new () { Name = "apellido", Type = "string" },
                    new () { Name = "email", Type = "string" },
                    new () { Name = "cargo", Type = "string" },
                    new () { Name = "industria", Type = "string" },
                    new () { Name = "nombre_company", Type = "string" },
                    new () { Name = "address", Type = "string" },
                    new () { Name = "size", Type = "string" },
                    new () { Name = "domain", Type = "string" },
                    new () { Name = "linkedin", Type = "string" },
                ]
        };

        var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "BaseClientesEmpresas pequeñas.xlsx");
        var reader = new ExcelFileReader(schema);
        var body = reader.ReadFile(filename);

        foreach (var item in body)
        {
            console.WriteLine(item.Dump(false));
        }
    }
}