namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    /// <summary>
    /// Interface de marquage représentant une commande (ou requête) pouvant être traitée par un <see cref="ICommandHandler{TCommand}"/>
    /// et décorée par des <see cref="IPipelineBehavior{TRequest}"/>.
    /// </summary>
    public interface ICommandDefinition
    {

    }
}
