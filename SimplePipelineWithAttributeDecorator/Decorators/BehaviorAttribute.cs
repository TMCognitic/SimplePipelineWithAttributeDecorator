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

    /// <summary>
    /// Variante générique fortement typée de <see cref="BehaviorAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Ne dérive pas de <see cref="BehaviorAttribute"/> : elle n'est donc pas prise en compte
    /// par <see cref="IServiceCollectionExtensions.AddHandlersFromAssembly"/> dans l'état actuel du POC.
    /// </remarks>
    /// <typeparam name="TBehavior">Type du comportement.</typeparam>
    /// <typeparam name="TCommand">Type de la commande concernée.</typeparam>
    /// <param name="priority">Priorité du comportement. 50 par défaut.</param>
    public class BehaviorAttribute<TBehavior, TCommand>(int priority = 50) : Attribute
        where TBehavior : IPipelineBehavior<TCommand>
        where TCommand : ICommandDefinition
    {
        /// <summary>
        /// Type du comportement à appliquer.
        /// </summary>
        public Type BehaviorType { get; } = typeof(TBehavior);

        /// <summary>
        /// Priorité du comportement dans le pipeline.
        /// </summary>
        public int Priority { get; } = priority;
    }
}
