using AspNetServiceLib.DataStructures;
using AspNetServiceLib.Exceptions;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow;
using AspNetServiceLib.ServiceInterface.Implementation.Workflow.WorkflowEvents;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetServiceLib.ServiceInterface.Implementation
{
    internal class WorkflowExecution : IWorkflowReplay
    {
        private readonly ILogger logger = Logging.Factory.CreateLogger<WorkflowExecution>();
        private readonly WorkflowRecording workflowRecording;
        private readonly WorkflowReplayParameters parameters;
        private readonly ReplayReceivedMessageDelegate replayReceivedMessage;
        private readonly object remainingPublishedMessagesMutex = new object();
        private BlockingCollection<WorkflowEvent> blockingEventsQueue;
        private List<WorkflowPublishedMessageEvent> remainingPublishedMessages;

        public delegate Task<ServiceMessage> ReplayReceivedMessageDelegate(ServiceCall serviceCall, ServiceMessage message);
        public WorkflowExecution(
            WorkflowRecording workflowRecording,
            WorkflowReplayParameters parameters,
            ReplayReceivedMessageDelegate replayReceivedMessage)
        {
            this.workflowRecording = workflowRecording;
            this.parameters = parameters;
            this.replayReceivedMessage = replayReceivedMessage;
        }
        public (bool alreadyPerformed, ServiceMessage reply) RecoverPublishedMessage(
            ServiceCall serviceCall,
            ServiceMessage message)
        {
            var publishedMessage = DequeueNextMatchingPublishedMessage(serviceCall);
            if (publishedMessage == null)
            {
                logger.LogWarning(
                    "Workflow record does not contain published message for service call '{serviceCall}'.",
                    serviceCall);
                return (false, null);
            }

            blockingEventsQueue.Add(publishedMessage);

            if (!ServiceMessageComparison.AreEqual(publishedMessage.Message, message))
            {
                logger.LogWarning(
                    "Workflow record contains published message for service call '{serviceCall}', " +
                    "but request messages are not matching.\nReceived: {received}\nExpected: {expected}",
                    serviceCall, publishedMessage.Message.MessageText, message.MessageText);
            }

            return (true, publishedMessage.Reply);
        }

        public async Task<bool> Execute(CancellationToken ct)
        {
            PreparePublishingMessages();
            blockingEventsQueue = new BlockingCollection<WorkflowEvent>();
            try
            {
                var remainingEventHistory = workflowRecording.EventHistory.ToList();
                while (remainingEventHistory.Any())
                {
                    ShowEventHistory(remainingEventHistory);
                    await Task.Yield(); //Let other tasks finish first before proceeding
                    ct.ThrowIfCancellationRequested();

                    var nextEvent = remainingEventHistory.FirstOrDefault();
                    var handledEvent = await HandleNextWorkflowEvent(nextEvent, ct);
                    if (!remainingEventHistory.Remove(handledEvent))
                    {
                        logger.LogWarning(
                            "Event for '{serviceCall}' has been received, even though it has not been recorded in currently executed workflow.",
                            handledEvent.ServiceCall);
                    }
                }
                return true;
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Workflow execution has been cancelled.");
                return false;
            }
            catch (TimeoutException)
            {
                logger.LogError("Workflow execution has timed out.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Workflow execution has encountered an error: {error}", ex.Message);
                return false;
            }
            finally
            {
                blockingEventsQueue.CompleteAdding();
                blockingEventsQueue.Dispose();
            }
        }

        private WorkflowPublishedMessageEvent DequeueNextMatchingPublishedMessage(ServiceCall serviceCall)
        {
            lock (remainingPublishedMessagesMutex)
            {
                var publishedMessage = remainingPublishedMessages
                    .FirstOrDefault(pm => pm.ServiceCall.Equals(serviceCall));
                if (publishedMessage != null)
                {
                    remainingPublishedMessages.Remove(publishedMessage);
                }
                return publishedMessage;
            }
        }

        private void ShowEventHistory(List<WorkflowEvent> eventHistory)
        {
            logger.LogTrace("Remaining events:");
            foreach (var ev in eventHistory)
            {
                logger.LogTrace(" - {eventType} on {serviceCall}", ev.EventType, ev.ServiceCall);
            }
        }

        private void PreparePublishingMessages()
        {
            remainingPublishedMessages = workflowRecording.EventHistory
                .OfType<WorkflowPublishedMessageEvent>()
                .ToList();
        }

        private async Task<WorkflowEvent> HandleNextWorkflowEvent(WorkflowEvent nextEvent, CancellationToken ct)
        {
            if (nextEvent is WorkflowReceivedMessageEvent receivedEvent)
            {
                var reply = await replayReceivedMessage(receivedEvent.ServiceCall, receivedEvent.Message);
                if (!ServiceMessageComparison.AreEqual(receivedEvent.Reply, reply))
                {
                    logger.LogWarning(
                        "Workflow record contains received reply message for service call '{serviceCall}', " +
                        "but reply messages are not matching.\nReceived: {received}\nExpected: {expected}",
                        receivedEvent.ServiceCall, reply.MessageText, receivedEvent.Reply.MessageText);
                }
                return receivedEvent;
            }
            if (nextEvent is WorkflowPublishedMessageEvent)
            {
                logger.LogInformation(
                    "Waiting for arrival of expected event from service call '{serviceCall}' to be triggered during workflow execution.",
                    nextEvent.ServiceCall);
                var blockingEvent = await WaitForBlockingEvent(ct);
                logger.LogInformation(
                    "Received expected event from service call '{serviceCall}' triggered during workflow execution.",
                    blockingEvent.ServiceCall);
                return blockingEvent;
            }

            throw new WorkflowException("Unknown workflow event type.");
        }

        private async Task<WorkflowEvent> WaitForBlockingEvent(CancellationToken ct)
        {
            var timeout = (int)parameters.WaitTimeout.TotalMilliseconds;
            var workflowEvent = await Task.Run(() =>
            {
                if (blockingEventsQueue.TryTake(out var ev, timeout, ct))
                {
                    return ev;
                }
                return null;
            });
            ct.ThrowIfCancellationRequested();
            if (workflowEvent == null)
            {
                throw new TimeoutException();
            }
            return workflowEvent;
        }
    }
}
