using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Behaviors;
namespace SimplePipelineWithAttributeDecorator.Decorators
{
    /// <summary>
    /// Ajoute le comportement <see cref="LogBehavior{TCommand}"/> (journalisation) au handler décoré, avec la priorité 1.
    /// </summary>
    public class LogAttribute() : BehaviorAttribute(typeof(LogBehavior<>), 1)
    {
    }
}
