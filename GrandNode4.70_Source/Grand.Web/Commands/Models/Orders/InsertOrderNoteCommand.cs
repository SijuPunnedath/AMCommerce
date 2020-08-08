﻿using Grand.Core.Domain.Localization;
using Grand.Core.Domain.Orders;
using Grand.Web.Models.Orders;
using MediatR;

namespace Grand.Web.Commands.Models.Orders
{
    public class InsertOrderNoteCommand : IRequest<OrderNote>
    {
        public Order Order { get; set; } 
        public Language Language { get; set; }
        public AddOrderNoteModel OrderNote { get; set; }
    }
}
