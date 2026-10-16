using SimplePipelineWithAttributeDecorator.Abstractions;

namespace SimplePipelineWithAttributeDecorator
{
    /// <summary>
    /// Attribut à poser sur un handler pour lui associer un comportement de pipeline.
    /// Les attributs dérivés (ex. <see cref="Decorators.LogAttribute"/>) fournissent un raccourci pour un comportement précis.
    /// </summary>
    /// <param name="behaviorType">Type générique ouvert du comportement (ex. <c>typeof(LogBehavior&lt;&gt;)</c>).</param>
    /// <param name="priority">Priorité du comportement ; les valeurs les plus élevées sont enregistrées en premier. 50 par défaut.</param>
    public class BehaviorAttribute(Type behaviorType, int priority = 50) : Attribute
    {
        /// <summary>
        /// Type générique ouvert du comportement à appliquer.
        /// </summary>
        public Type BehaviorType { get; } = behaviorType;

        /// <summary>
        /// Priorité du comportement dans le pipeline.
        /// </summary>
        public int Priority { get; } = priority;
    }
}
