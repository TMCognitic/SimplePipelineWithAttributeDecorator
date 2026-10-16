using Microsoft.Extensions.DependencyInjection;
using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Pipelines;
using System.Reflection;

namespace SimplePipelineWithAttributeDecorator
{
    /// <summary>
    /// Méthodes d'extension d'enregistrement des gestionnaires et pipelines dans le conteneur d'injection de dépendances.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            /// <summary>
            /// Scanne un assembly à la recherche des implémentations de <see cref="ICommandHandler{TCommand}"/>,
            /// les enregistre (scope) et, pour celles portant au moins un <see cref="BehaviorAttribute"/>,
            /// enregistre les comportements correspondants ainsi qu'un <see cref="IPipeline{TCommand}"/>.
            /// </summary>
            /// <remarks>
            /// Les comportements sont enregistrés par priorité décroissante. Un handler sans attribut
            /// n'a pas de pipeline enregistré : il doit alors être appelé directement.
            /// </remarks>
            /// <param name="assembly">Assembly à analyser.</param>
            /// <returns>La collection de services, pour chaînage.</returns>
            public IServiceCollection AddHandlersFromAssembly(Assembly assembly)
            {
                Type[] handlers = assembly.GetTypes().Where(t => t.GetInterfaces().Any(IsHandlerInterface)).ToArray();

                foreach (Type handler in handlers)
                {
                    Type interfaceHandlerType = handler.GetInterfaces().Single(IsHandlerInterface);
                    Type genericParameter = interfaceHandlerType.GetGenericArguments().Single();

                    IEnumerable<BehaviorAttribute> behaviorAttributes = handler.GetCustomAttributes<BehaviorAttribute>();

                    services.AddScoped(interfaceHandlerType, handler);

                    if (behaviorAttributes.Any())
                    {
                        Type behaviorInterface = typeof(IPipelineBehavior<>).MakeGenericType(genericParameter);

                        foreach (BehaviorAttribute attribute in behaviorAttributes.OrderByDescending(a => a.Priority))
                        {
                            Type behaviorType = attribute.BehaviorType;
                            behaviorType = behaviorType.MakeGenericType(genericParameter);
                            services.AddScoped(behaviorInterface, behaviorType);
                        }

                        Type pipelineInterface = typeof(IPipeline<>).MakeGenericType(genericParameter);
                        Type pipelineType = typeof(Pipeline<>).MakeGenericType(genericParameter);

                        MethodInfo createMethod = pipelineType
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Single(m => m.Name == "Create" && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == interfaceHandlerType.GetGenericTypeDefinition());

                        services.AddScoped(pipelineInterface, sp =>
                        {
                            object handler = sp.GetRequiredService(interfaceHandlerType);
                            object behaviors = sp.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(behaviorInterface));
                            return createMethod.Invoke(null, [handler, behaviors])!;
                        });
                    }
                }

                return services;
            }
        }

        extension(Type type)
        {
            /// <summary>
            /// Indique si le type est une construction générique de <see cref="ICommandHandler{TCommand}"/>.
            /// </summary>
            /// <returns><c>true</c> si le type est une interface de handler ; sinon <c>false</c>.</returns>
            internal bool IsHandlerInterface()
            {
                Type[] handlerInterfaces = [typeof(ICommandHandler<>)];

                if (!type.IsGenericType)
                    return false;

                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                return handlerInterfaces.Contains(genericTypeDefinition);
            }
        }
    }
}
