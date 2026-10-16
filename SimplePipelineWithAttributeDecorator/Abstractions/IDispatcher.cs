using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    /// <summary>
    /// Point d'entrée unique pour exécuter une commande, sans connaître le type de son handler
    /// ni savoir si un pipeline est enregistré pour celui-ci.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Exécute la commande via son <see cref="IPipeline{TCommand}"/> s'il existe,
        /// sinon directement via son <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <param name="command">Commande à exécuter.</param>
        /// <returns>Le résultat de l'exécution de la commande.</returns>
        /// <exception cref="InvalidOperationException">Aucun pipeline ni handler n'est enregistré pour le type de la commande.</exception>
        Result Dispatch(ICommandDefinition command);
    }
}
