using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    public interface IPipelineBehavior<TRequest>
        where TRequest : ICommandDefinition
    {
        Result Handle(TRequest request, Func<Result> next);
    }
}
