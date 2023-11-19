﻿using Dapr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IntegrationEvents;
using Order.Application.Services.OrderPaymentFailed;
using Order.Application.Services.OrderPaymentSucceeded;
using Order.Application.Services.OrderStockConfirmed;
using Order.Application.Services.OrderStockRejected;
using Order.Application.Services.UserCheckoutAccepted;

namespace Order.API.Controllers;

[Route("api/v1/[controller]")]
[AllowAnonymous]
[ApiController]
public class OrderingProcessEventController : ControllerBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";
    private readonly IMediator _mediator;

    public OrderingProcessEventController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("UserCheckoutAccepted")]
    [Topic(DAPR_PUBSUB_NAME, "UserCheckoutAcceptedIntegrationEvent")]
    public async Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent)
    {
        await _mediator.Publish(new UserCheckoutAcceptedNotification(
                integrationEvent.UserId,
                integrationEvent.UserEmail,
                integrationEvent.City,
                integrationEvent.Street,
                integrationEvent.State,
                integrationEvent.Country,
                integrationEvent.CardNumber,
                integrationEvent.CardHolderName,
                integrationEvent.CardExpiration,
                integrationEvent.CardSecurityNumber,
                integrationEvent.RequestId,
                integrationEvent.Basket
            ));
    }

    [HttpPost("OrderStockConfirmed")]
    [Topic(DAPR_PUBSUB_NAME, "OrderStockConfirmedIntegrationEvent")]
    public async Task HandleAsync(OrderStockConfirmedIntegrationEvent integrationEvent)
    {
        await _mediator.Publish(new OrderStockConfirmedNotification(integrationEvent.OrderId));
    }

    [HttpPost("OrderStockRejected")]
    [Topic(DAPR_PUBSUB_NAME, "OrderStockRejectedIntegrationEvent")]
    public async Task HandleAsync(OrderStockRejectedIntegrationEvent integrationEvent)
    {
        await _mediator.Publish(new OrderStockRejectedNotification(integrationEvent.Id, integrationEvent.OrderStockItems));
    }

    [HttpPost("OrderPaymentSucceeded")]
    [Topic(DAPR_PUBSUB_NAME, "OrderPaymentSucceededIntegrationEvent")]
    public async Task HandleAsync(OrderPaymentSucceededIntegrationEvent integrationEvent)
    {
        await _mediator.Publish(new OrderPaymentSucceededNotification(integrationEvent.OrderId));
    }

    [HttpPost("OrderPaymentFailed")]
    [Topic(DAPR_PUBSUB_NAME, "OrderPaymentFailedIntegrationEvent")]
    public async Task HandleAsync(OrderPaymentFailedIntegrationEvent integrationEvent)
    {
        await _mediator.Publish(new OrderPaymentFailedNotification(integrationEvent.OrderId));
    }

}
