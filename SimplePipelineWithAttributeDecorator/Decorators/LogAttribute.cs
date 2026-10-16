using SimplePipelineWithAttributeDecorator.Behaviors;
namespace SimplePipelineWithAttributeDecorator.Decorators
{
    public class LogAttribute() : BehaviorAttribute(typeof(LogBehavior<>), 1)
    {
    }
}
