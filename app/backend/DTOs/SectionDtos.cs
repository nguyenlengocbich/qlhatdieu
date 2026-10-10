namespace CashewCms.Api.DTOs;

public record UpdateSectionRequest(
    string? Name,
    string? Title,
    string? Subtitle,
    string? Description,
    string? Content,
    string? Image,
    string? Status
);

public record CreateItemRequest(
    string Title,
    string? Description,
    string? Icon,
    string? Image,
    string? Status
);

public record UpdateItemRequest(
    string? Title,
    string? Description,
    string? Icon,
    string? Image,
    int? SortOrder,
    string? Status
);
