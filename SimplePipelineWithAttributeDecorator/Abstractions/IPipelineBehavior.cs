using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    /// <summary>
    /// Comportement transverse (journalisation, validation, etc.) s'insérant dans un pipeline
    /// avant et/ou après l'exécution du gestionnaire de commande.
    /// </summary>
    /// <typeparam name="TRequest">Type de la commande concernée.</typeparam>
    public interface IPipelineBehavior<TRequest>
        where TRequest : ICommandDefinition
    {
        /// <summary>
        /// Exécute le comportement.
        /// </summary>
        /// <param name="request">Commande en cours de traitement.</param>
        /// <param name="next">Délégué appelant l'élément suivant du pipeline (comportement suivant ou gestionnaire).
        /// Ne pas l'appeler court-circuite le reste de la chaîne.</param>
        /// <returns>Le <see cref="Result"/> du traitement.</returns>
        Result Handle(TRequest request, Func<Result> next);
    }
}
