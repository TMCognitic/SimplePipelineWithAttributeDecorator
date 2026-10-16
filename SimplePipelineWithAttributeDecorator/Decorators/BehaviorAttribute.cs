namespace SimplePipelineWithAttributeDecorator
{
    public class BehaviorAttribute(Type behaviorType, int priority = 50) : Attribute
    {
        public Type BehaviorType { get; } = behaviorType;
        public int Priority { get; } = priority;
    }
}