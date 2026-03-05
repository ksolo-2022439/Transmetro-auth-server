using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TransmetroConecta.Auth.API.Filters;

public class ValidationFilterAttribute : IActionFilter
{
    /// <summary>
    /// Intercepta las peticiones HTTP antes de la ejecución del controlador para validar el modelo y retornar un listado estructurado de errores en caso de fallos.
    /// </summary>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { message = "Errores de validación", errors });
        }
    }

    /// <summary>
    /// Se ejecuta después de la acción del controlador. Obligatorio por la implementación de la interfaz IActionFilter.
    /// </summary>
    public void OnActionExecuted(ActionExecutedContext context) { }
}