using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

/// <summary>
/// Запрос поиска оборудования
/// </summary>
/// <param name="EquipmentId">Идентификатор оборудования (при наличии)</param>
/// <param name="Query">Запрос (начало названия оборудования в любом регистре)</param>
/// <param name="Page">Номер страницы (1-индексация)</param>
public record EquipmentFindRequest(Guid? EquipmentId = null, string? Query = null, [Range(1, Int32.MaxValue)] int Page = 1);