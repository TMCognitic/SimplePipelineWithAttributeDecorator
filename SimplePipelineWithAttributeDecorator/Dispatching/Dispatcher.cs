using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Dispatching
{
    /// <summary>
    /// Implémentation de <see cref="IDispatcher"/> qui résout dynamiquement le pipeline ou le handler
    /// de la commande à partir de son type d'exécution, via le <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceProvider">Fournisseur de services utilisé pour résoudre pipelines et handlers.</param>
    public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
    {
        /// <summary>
        /// Exécute la commande via son <see cref="IPipeline{TCommand}"/> s'il est enregistré,
        /// sinon directement via son <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <remarks>
        /// La résolution s'appuie sur <c>dynamic</c> : le type de la commande est déterminé à l'exécution.
        /// </remarks>
        /// <param name="command">Commande à exécuter.</param>
        /// <returns>Le résultat de l'exécution de la commande.</returns>
        /// <exception cref="InvalidOperationException">Aucun pipeline ni handler n'est enregistré pour le type de la commande.</exception>
        public Result Dispatch(ICommandDefinition command)
        {
            Type pipelineInterface = typeof(IPipeline<>).MakeGenericType(command.GetType());

            dynamic? pipeline = serviceProvider.GetService(pipelineInterface);

            if (pipeline is not null)
                return pipeline.Execute((dynamic)command);

            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic? handler = serviceProvider.GetService(handlerType);

            if (handler is null)
            {
                throw new InvalidOperationException($"the type {handlerType.FullName} is'nt registered");
            }

            return handler.Handle((dynamic)command);

        }
    }
}
