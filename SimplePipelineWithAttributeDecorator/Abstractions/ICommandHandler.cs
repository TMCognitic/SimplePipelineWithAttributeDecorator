using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    /// <summary>
    /// Définit un gestionnaire chargé de traiter une commande donnée.
    /// </summary>
    /// <typeparam name="TCommand">Type de la commande traitée.</typeparam>
    public interface ICommandHandler<TCommand>
        where TCommand : ICommandDefinition
    {
        /// <summary>
        /// Traite la commande.
        /// </summary>
        /// <param name="command">Commande à traiter.</param>
        /// <returns>Un <see cref="Result"/> indiquant le succès ou l'échec du traitement.</returns>
        Result Handle(TCommand command);
    }
}
