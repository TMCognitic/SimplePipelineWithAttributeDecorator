using Microsoft.Extensions.Logging;
using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Results;
using System.Text.Json;

namespace SimplePipelineWithAttributeDecorator.Behaviors
{
    internal class LogBehavior<TCommand>(ILogger<LogBehavior<TCommand>> logger) : IPipelineBehavior<TCommand>
        where TCommand : ICommandDefinition
    {
        private DateTime _now;
        private readonly ILogger _logger = logger;

        public Result Handle(TCommand request, Func<Result> next)
        {
            string requestName = request.GetType().Name;
            _now = DateTime.UtcNow;
            _logger.LogInformation("Requete {RequestName} reçue à {Time}", requestName, _now);
            
            Result result = next();
            
            _logger.LogInformation("La requête {RequestName} exécutée en {Milliseconds} ms", requestName, (DateTime.UtcNow - _now).TotalMilliseconds);
            if (result.IsFailure)
            {
                _logger.LogWarning("La requête {RequestName} a échouée : {Result}", requestName, JsonSerializer.Serialize(result.Error));
            }
            else
            {
                _logger.LogInformation("La requête {RequestName} a réussi avec succès", requestName);
            }
            
            return result;
        }
    }
}
