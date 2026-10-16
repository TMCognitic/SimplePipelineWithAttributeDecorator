using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Results;

namespace SimplePipelineWithAttributeDecorator.Pipelines
{
    public class Pipeline<TCommand> : IPipeline<TCommand>
        where TCommand : ICommandDefinition
    {
        private readonly IEnumerable<IPipelineBehavior<TCommand>> _behaviors;

        private readonly Func<TCommand, Result> _handler;

        public static IPipeline<TCommand> Create(ICommandHandler<TCommand> handler, IEnumerable<IPipelineBehavior<TCommand>> behaviors)
        {
            return new Pipeline<TCommand>(handler.Handle, behaviors);
        }

        private Pipeline(Func<TCommand, Result> handler, IEnumerable<IPipelineBehavior<TCommand>> behaviors)
        {
            _handler = handler;
            _behaviors = behaviors;
        }

        public Result Execute(TCommand request)
        {
            Func<Result> next = () => _handler(request);

            foreach (var behavior in _behaviors.Reverse())
            {
                var current = next;
                next = () => behavior.Handle(request, current);
            }

            return next();
        }
    }
}
