using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Abstractions
{
    public interface IDispatcher
    {
        Result Dispatch(ICommandDefinition command);
    }
}
