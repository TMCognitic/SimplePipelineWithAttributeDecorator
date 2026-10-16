using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    public interface IPipeline<TCommand>
        where TCommand : ICommandDefinition
    {
        Result Execute(TCommand request);
    }
}
