using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommandDefinition
    {
        Result Handle(TCommand command);
    }
}
