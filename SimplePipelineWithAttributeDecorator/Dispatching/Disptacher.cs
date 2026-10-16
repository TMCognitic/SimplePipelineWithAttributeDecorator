using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Dispatching
{
    public class Disptacher(IServiceProvider serviceProvider) : IDispatcher
    {
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
