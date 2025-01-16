using System;

namespace LeadTrackApi.Domain.Exceptions;

public class InvalidRutException : Exception
{
    public InvalidRutException() : base("Rut inválido") { }
    public InvalidRutException(string rutWithDv) : base($"Rut {rutWithDv} inválido") { }
    public InvalidRutException(string rutWithDv, Exception inner) : base($"Rut {rutWithDv} inválido", inner) { }
}
