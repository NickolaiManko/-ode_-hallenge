using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            CorrelationId = Guid.NewGuid();
            CreationDate = DateTimeOffset.Now;
        }
        public IntegrationBaseEvent(Guid id, DateTimeOffset createDate)
        {
            CorrelationId = id;
            CreationDate = createDate;
        }

        public Guid CorrelationId { get; private set; }
        public DateTimeOffset CreationDate { get; private set; }
    }
}
