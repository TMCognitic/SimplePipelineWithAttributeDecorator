using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Pipelines
{
    /// <summary>
    /// Implémentation par défaut de <see cref="IPipeline{TCommand}"/> : enchaîne les comportements
    /// puis appelle le gestionnaire de commande.
    /// </summary>
    /// <typeparam name="TCommand">Type de la commande exécutée.</typeparam>
    public class Pipeline<TCommand> : IPipeline<TCommand>
        where TCommand : ICommandDefinition
    {
        private readonly IEnumerable<IPipelineBehavior<TCommand>> _behaviors;

        private readonly Func<TCommand, Result> _handler;

        /// <summary>
        /// Crée un pipeline.
        /// </summary>
        /// <param name="handler">Gestionnaire appelé en bout de chaîne.</param>
        /// <param name="behaviors">Comportements à appliquer, le premier de la liste étant le plus externe.</param>
        /// <returns>Le pipeline créé.</returns>
        public static IPipeline<TCommand> Create(ICommandHandler<TCommand> handler, IEnumerable<IPipelineBehavior<TCommand>> behaviors)
        {
            return new Pipeline<TCommand>(handler.Handle, behaviors);
        }

        private Pipeline(Func<TCommand, Result> handler, IEnumerable<IPipelineBehavior<TCommand>> behaviors)
        {
            _handler = handler;
            _behaviors = behaviors;
        }

        /// <inheritdoc />
        public Result Execute(TCommand request)
        {
            Func<Result> next = () => _handler(request);

            // Construction de la chaîne en partant du dernier comportement,
            // afin que le premier soit appelé en premier à l'exécution.
            foreach (var behavior in _behaviors.Reverse())
            {
                var current = next;
                next = () => behavior.Handle(request, current);
            }

            return next();
        }
    }
}
