using LeadTrackApi.Domain.Exceptions;
using LeadTrackApi.Domain.Models;

namespace LeadTrackApi.Application.Utils;

public class TextFileReader
{
    private FileSchema _schema;

    public TextFileReader(FileSchema schema)
    {
        _schema = schema;
        if (_schema.ValidateSchema().Count > 0) throw new BussinesException(_schema.PrintErrors());
    }

    public (Dictionary<string, object> Header, List<Dictionary<string, object>> Body) ReadFile(string filePath)
    {
        var header = new Dictionary<string, object>();
        using var reader = new StreamReader(filePath);

        if (_schema.HeaderFields != null && _schema.HeaderFields.Count > 0)
        {
            var headerLine = reader.ReadLine();
            if (headerLine != null) header = ParseAndProcessLine(headerLine, _schema.HeaderFields);
        }

        var body = new List<Dictionary<string, object>>();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var row = ParseAndProcessLine(line, _schema.BodyFields);
            body.Add(row);
        }

        return (header, body);
    }

    private Dictionary<string, object> ParseAndProcessLine(string line, List<FieldSchema> fields)
    {
        var parsedLine = ParseLine(line, fields);
        return ProcessRow(parsedLine, fields);
    }

    private string[] ParseLine(string line, List<FieldSchema> fields)
    {
        if (_schema.Type == "delimited")
        {
            return line.Split(_schema.Delimiter);
        }
        else if (_schema.Type == "fixed_length")
        {
            var row = new List<string>();
            int start = 0;
            foreach (var field in fields)
            {
                row.Add(line.Substring(start, field.Length).Trim());
                start += field.Length;
            }
            return [.. row];
        }
        else
        {
            throw new NotSupportedException("Tipo de archivo no soportado.");
        }
    }

    private Dictionary<string, object> ProcessRow(string[] row, List<FieldSchema> fields)
    {
        var processedRow = new Dictionary<string, object>();
        for (int i = 0; i < fields.Count; i++)
        {
            var field = fields[i];
            var value = row[i];

            processedRow[field.Name] = field.Type.ToLower() switch
            {
                "int" => int.TryParse(value, out int intValue) ? intValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número entero."),
                "long" => long.TryParse(value, out long longValue) ? longValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número entero."),
                "string" => value,
                "date" => DateTime.TryParseExact(value, field.Format, null, System.Globalization.DateTimeStyles.None, out DateTime dateValue) ? dateValue : throw new ArgumentException($"El campo '{field.Name}' no tiene el formato de fecha correcto ({field.Format})."),
                "decimal" => decimal.TryParse(value, out decimal decimalValue) ? decimalValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número decimal."),
                _ => throw new NotSupportedException($"Tipo de campo no soportado: {field.Type}")
            };
        }
        return processedRow;
    }
}
