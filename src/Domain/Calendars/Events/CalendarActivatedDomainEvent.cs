﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Calendars.Events;

public record CalendarActivatedDomainEvent(CalendarId CalendarId) : IDomainEvent;
