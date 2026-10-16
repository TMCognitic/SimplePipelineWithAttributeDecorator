# SimplePipelineWithAttributeDecorator

> ⚠️ **Ceci est un POC (Proof of Concept).**
> Ce dépôt sert uniquement à explorer une idée. Il n'est ni conçu ni testé pour un usage en production, et son API peut changer ou être incomplète.

## Objectif

Explorer une manière simple de construire un **pipeline de comportements** (à la `MediatR` / décorateur) autour de handlers de commandes, où les comportements sont **déclarés par des attributs** posés directement sur le handler :

```csharp
[Log]
class SimpleCommandHandler : ICommandHandler<SimpleCommand>
{
    public Result Handle(SimpleCommand command) => Result.Success();
}
```

Au démarrage, un scan de l'assembly enregistre les handlers dans le conteneur d'injection de dépendances. Pour chaque handler portant un attribut de comportement, un `IPipeline<TCommand>` est également enregistré.

Un `IDispatcher` permet ensuite d'exécuter une commande sans se soucier de la présence d'un pipeline :

```csharp
IDispatcher dispatcher = provider.GetRequiredService<IDispatcher>();
Result result = dispatcher.Dispatch(new SimpleCommand());
```

## Structure de la solution

| Projet | Rôle |
|---|---|
| `SimplePipelineWithAttributeDecorator` | Bibliothèque : abstractions, pipeline, comportements, attributs, enregistrement DI |
| `SimplePipelineWithAttributeDecorator.Console` | Application console de démonstration |

### Contenu de la bibliothèque

- **Abstractions** : `ICommandDefinition` (commande), `ICommandHandler<TCommand>` (handler), `IPipeline<TCommand>` (pipeline), `IPipelineBehavior<TRequest>` (comportement), `IDispatcher` (point d'entrée d'exécution d'une commande).
- **Dispatching** : `Dispatcher` (implémentation de `IDispatcher`) résout le pipeline de la commande s'il existe, sinon son handler, et l'exécute. Une `InvalidOperationException` est levée si aucun des deux n'est enregistré.
- **Pipelines** : `Pipeline<TCommand>` enchaîne les comportements puis appelle le handler.
- **Behaviors** : `LogBehavior<TCommand>` journalise la réception, la durée et l'issue de la commande.
- **Decorators** : `BehaviorAttribute` (attribut de base) et `LogAttribute` (raccourci pour `LogBehavior`).
- **Results** : `Result` et `Error`, pour retourner succès/échec sans exception.
- **IServiceCollectionExtensions** : `AddHandlersFromAssembly(assembly)` réalise le scan et l'enregistrement.

## Fonctionnement

1. `AddHandlersFromAssembly` recherche les classes implémentant `ICommandHandler<TCommand>`.
2. Chaque handler est enregistré (scope).
3. Le `Dispatcher` est enregistré sous forme de `IDispatcher`
4. S'il porte des `BehaviorAttribute`, les comportements sont enregistrés par priorité décroissante, puis un `IPipeline<TCommand>` est enregistré.
5. `IDispatcher.Dispatch(command)` cherche un `IPipeline<TCommand>` pour le type de la commande ; à défaut, il appelle directement le `ICommandHandler<TCommand>`.
6. À l'exécution, `Pipeline<TCommand>.Execute` appelle les comportements dans l'ordre, chacun pouvant exécuter du code avant/après l'appel à `next()`, jusqu'au handler.

## Créer son propre comportement

```csharp
public class MonBehavior<TCommand> : IPipelineBehavior<TCommand>
    where TCommand : ICommandDefinition
{
    public Result Handle(TCommand request, Func<Result> next)
    {
        // avant
        Result result = next();
        // après
        return result;
    }
}

public class MonAttribute() : BehaviorAttribute(typeof(MonBehavior<>), priority: 10) { }
```

## Lancer la démo

Prérequis : SDK **.NET 10**.

```bash
dotnet run --project SimplePipelineWithAttributeDecorator.Console
```

## Limites connues (POC)

- Un handler sans attribut n'a **pas** de pipeline enregistré : le `Dispatcher` l'appelle alors directement.
- Le `Disptacher` utilise `dynamic` et la réflexion à chaque appel (pas de cache).
- Un seul `ICommandHandler<>` par classe de handler est supporté.
- Les comportements sont enregistrés comme services scopés partagés par type de commande ; l'ordre d'exécution repose sur l'ordre d'enregistrement.
- Pas de gestion des exceptions dans le pipeline, pas de support asynchrone, pas de tests automatisés.
- `LogBehavior` conserve un état d'instance (`_now`) : acceptable en scope, à revoir pour un usage concurrent.

## Documentation du code

Les types et membres publics sont documentés avec des commentaires XML (en français).
