﻿using ECommers.Dapr.Events;

namespace Payment.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
