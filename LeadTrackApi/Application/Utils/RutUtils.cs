using LeadTrack.API.Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace LeadTrack.API.Application.Utils
{
    public static class RutUtils
    {

        /// <summary>
        /// Limpia y separa el rut y dv, no Valida el rut
        /// </summary>
        /// <param name="rutWithDv">Rut con digito verificador</param>
        /// <returns>rut integer</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static (int rut, string dv) GetRut(string rutWithDv)
        {
            try
            {
                string rutClean = CleanRut(rutWithDv);
                var rut = rutClean[0..^1];
                return (Convert.ToInt32(rut), rutClean[^1..]);
            }
            catch (Exception ex)
            {
                throw new InvalidRutException(rutWithDv, ex);
            }
        }

        private static string CleanRut(string rutWithDv)
        {
            string expression = "\\.|-|_| ";
            rutWithDv = rutWithDv ?? throw new ArgumentNullException(nameof(rutWithDv));
            var rutClean = Regex.Replace(rutWithDv.ToUpper(), expression, "");
            return rutClean;
        }

        public static string FormatRutWithOutDots(string rutWithDv)
        {
            var (rut, dv) = GetRut(rutWithDv);
            return $"{rut}-{dv}";
        }

        public static bool IsValidRut(string rutWithDv)
        {
            try
            {
                var (rut, dv) = GetRut(rutWithDv);

                int s = 1;
                for (int m = 0; rut != 0; rut /= 10) s = (s + rut % 10 * (9 - m++ % 6)) % 11;

                return dv[0] == (char)(s != 0 ? s + 47 : 75);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
