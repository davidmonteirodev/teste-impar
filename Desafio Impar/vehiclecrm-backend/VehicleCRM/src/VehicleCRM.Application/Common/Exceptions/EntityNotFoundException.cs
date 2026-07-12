namespace VehicleCRM.Application.Common.Exceptions;

/// <summary>
/// Exceção lançada quando uma entidade não é encontrada no banco de dados.
/// Esta exceção pertence à camada de Application pois representa uma preocupação de orquestração.
/// </summary>
public class EntityNotFoundException : Exception
{
    public string EntityName { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"Entidade '{entityName}' com id '{entityId}' não foi encontrada.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public EntityNotFoundException(string entityName, object entityId, string message)
        : base(message)
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
