using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimplePipelineWithAttributeDecorator;
using SimplePipelineWithAttributeDecorator.Abstractions;
using SimplePipelineWithAttributeDecorator.Decorators;
using SimplePipelineWithAttributeDecorator.Results;
using System.Text.Json;

// POC : composition root minimale (conteneur DI + logging console).
IServiceCollection services = new ServiceCollection();
services.AddLogging(o => o.AddConsole());

// Scanne l'assembly courant : enregistre les handlers et, via les attributs, leurs pipelines.
services.AddHandlersFromAssembly(typeof(Program).Assembly);

IServiceProvider provider = services.BuildServiceProvider();

// Les services sont enregistrés en scope : on crée donc un scope pour les résoudre.
provider = provider.CreateScope().ServiceProvider;

// Si le handler n'a pas d'attribut de comportement, aucun pipeline n'est enregistré : on appelle alors le handler directement.
IDispatcher dispatcher = provider.GetRequiredService<IDispatcher>();
Result result = dispatcher.Dispatch(new SimpleCommand());

Console.WriteLine(JsonSerializer.Serialize(result));


/// <summary>
/// Commande d'exemple, sans donnée, utilisée pour la démonstration.
/// </summary>
public class SimpleCommand : ICommandDefinition
{

}

/// <summary>
/// Handler d'exemple de <see cref="SimpleCommand"/>. L'attribut <see cref="LogAttribute"/>
/// déclenche la création d'un pipeline avec le comportement de journalisation.
/// </summary>
[Log]
public class SimpleCommandHandler : ICommandHandler<SimpleCommand>
{
    /// <summary>
    /// Traite la commande en retournant toujours un succès.
    /// </summary>
    /// <param name="command">Commande à traiter.</param>
    /// <returns>Un résultat en succès.</returns>
    public Result Handle(SimpleCommand command)
    {
        return Result.Success();
    }
}