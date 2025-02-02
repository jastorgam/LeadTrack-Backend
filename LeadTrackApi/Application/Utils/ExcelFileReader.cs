﻿using LeadTrackApi.Domain.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;


namespace LeadTrackApi.Application.Utils;

public class ExcelFileReader(FileSchema fileSchema)
{
    private List<FieldSchema> _fields = fileSchema.BodyFields;

    public IEnumerable<Dictionary<string, object>> ReadFile(Stream fileStream)
    {
        IWorkbook workbook;

        if (fileStream.CanSeek)
        {
            var buffer = new byte[4];
            fileStream.Read(buffer, 0, 4);
            fileStream.Seek(0, SeekOrigin.Begin);

            if (buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04)
            {
                workbook = new XSSFWorkbook(fileStream); // XLSX
            }
            else
            {
                workbook = new HSSFWorkbook(fileStream); // XLS
            }
        }
        else
        {
            throw new ArgumentException("No se puede determinar el formato del archivo porque el Stream no admite operaciones de búsqueda.");
        }

        var worksheet = workbook.GetSheetAt(0);
        var headerRow = worksheet.GetRow(0);

        // Validar que el número de columnas coincida con el esquema
        if (headerRow.LastCellNum < _fields.Count)
        {
            throw new ArgumentException("El número de columnas en el archivo no coincide con el esquema.");
        }

        for (int rowIndex = 1; rowIndex <= worksheet.LastRowNum; rowIndex++)
        {
            var row = worksheet.GetRow(rowIndex);
            if (row == null) continue; // Saltar filas vacías

            var rowData = new Dictionary<string, object>();
            for (int colIndex = 0; colIndex < _fields.Count; colIndex++)
            {
                var field = _fields[colIndex];
                var cell = row.GetCell(colIndex);
                var cellValue = cell?.ToString() ?? string.Empty;

                rowData[field.Name] = ProcessCellValue(cellValue, field);
            }
            yield return rowData;
        }
    }

    private object ProcessCellValue(string value, FieldSchema field)
    {
        //// Validar campo obligatorio
        //if (field.IsRequired && string.IsNullOrWhiteSpace(value))
        //{
        //    throw new ArgumentException($"El campo '{field.Name}' es obligatorio pero está vacío.");
        //}

        //// Validar longitud máxima
        //if (field.MaxLength > 0 && value.Length > field.MaxLength)
        //{
        //    throw new ArgumentException($"El campo '{field.Name}' excede la longitud máxima de {field.MaxLength}.");
        //}

        //// Validar valores permitidos
        //if (field.AllowedValues != null && field.AllowedValues.Length > 0 && !field.AllowedValues.Contains(value))
        //{
        //    throw new ArgumentException($"El valor '{value}' no está permitido para el campo '{field.Name}'.");
        //}

        // Procesar el valor según el tipo de campo
        object resp = field.Type.ToLower() switch
        {
            "int" => int.TryParse(value, out int intValue) ? intValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número entero."),
            "long" => long.TryParse(value, out long longValue) ? longValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número entero."),
            "string" => value,
            "date" => DateTime.TryParseExact(value, field.Format, null, System.Globalization.DateTimeStyles.None, out DateTime dateValue) ? dateValue : throw new ArgumentException($"El campo '{field.Name}={value}' no tiene el formato de fecha correcto ({field.Format})."),
            "decimal" => decimal.TryParse(value, out decimal decimalValue) ? decimalValue : throw new ArgumentException($"El campo '{field.Name}' debe ser un número decimal."),
            _ => throw new NotSupportedException($"Tipo de campo no soportado: {field.Type}")
        };
        return resp;
    }
}
