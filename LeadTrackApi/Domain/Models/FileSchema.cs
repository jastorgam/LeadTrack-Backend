namespace LeadTrackApi.Domain.Models;

public class FileSchema
{
    private readonly List<string> errors = [];

    public string Type { get; set; } // "delimited", "fixed_length", "excel"
    public string Delimiter { get; set; } // Solo para archivos delimitados
    public List<FieldSchema> HeaderFields { get; set; } // Campos de la cabecera
    public List<FieldSchema> BodyFields { get; set; } // Campos del cuerpo

    public List<string> ValidateSchema()
    {
        if (Type == "fixed_length")
        {
            foreach (var field in HeaderFields ?? Enumerable.Empty<FieldSchema>())
                if (field.Length == null || field.Length <= 0) AddError($"ERROR: {field.Name} debe tener largo");

            foreach (var field in BodyFields ?? Enumerable.Empty<FieldSchema>())
                if (field.Length == null || field.Length <= 0) AddError($"ERROR: {field.Name} debe tener largo");
        }
        else if (Type == "delimited" && Delimiter == null) AddError($"Tipo: {Type} debe tener seteado un delimitador");
        else AddError($"{Type} no es un tipo de archivo soportado.");
        return errors;
    }

    private void AddError(string error)
    {
        errors.Add(error);
    }

    public string PrintErrors()
    {
        var msj = "";
        foreach (string s in errors) msj += s + "\n";
        return msj;
    }


}
