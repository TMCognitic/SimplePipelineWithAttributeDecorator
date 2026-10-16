using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    /// <summary>
    /// Représente un pipeline d'exécution : une chaîne de comportements (<see cref="IPipelineBehavior{TRequest}"/>)
    /// entourant l'appel au gestionnaire de commande.
    /// </summary>
    /// <typeparam name="TCommand">Type de la commande exécutée.</typeparam>
    public interface IPipeline<TCommand>
        where TCommand : ICommandDefinition
    {
        /// <summary>
        /// Exécute la commande au travers des comportements puis du gestionnaire.
        /// </summary>
        /// <param name="request">Commande à exécuter.</param>
        /// <returns>Le <see cref="Result"/> produit par le pipeline.</returns>
        Result Execute(TCommand request);
    }
}
