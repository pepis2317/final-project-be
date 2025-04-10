﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace final_project_backend.Models.Item
{
    public class EditItemRequest : IRequest<(ProblemDetails?, ItemResponse?)>
    {
        [SwaggerIgnore]
        public Guid ItemId { get; set; }
        public string? ItemName {  get; set; }
        public string? ItemDesc { get; set; }
        public int? Quantity { get; set; }
        public int? HargaPerItem { get; set; }
    }
}
