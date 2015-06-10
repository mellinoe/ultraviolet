﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TwistedLogik.Nucleus.Collections;
using System.Reflection;

namespace TwistedLogik.Nucleus.Messages
{
    /// <summary>
    /// Represents a message queue which exists entirely within the local process.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message which is published by the queue.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public partial class LocalMessageQueue<TMessageType> : IMessageQueue<TMessageType> where TMessageType : IEquatable<TMessageType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMessageQueue{TMessageType}"/> class with the default capacity.
        /// </summary>
        public LocalMessageQueue()
            : this(4)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMessageQueue{TMessageType}"/> class with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the message queue's pools.</param>
        public LocalMessageQueue(Int32 capacity)
        {
            this.queue     = new Queue<EnqueuedMessage>(capacity);
            this.queuePool = new ExpandingPool<EnqueuedMessage>(capacity);
        }

        /// <summary>
        /// Creates or retrieves an instance of the specified message data type.
        /// The instance may be retrieved from a pool; if so, it will be returned to the pool 
        /// once it has been published.
        /// </summary>
        /// <typeparam name="TMessageData">The type of message data object to create.</typeparam>
        /// <returns>The instance that was created or retrieved.</returns>
        public TMessageData CreateMessageData<TMessageData>() where TMessageData : MessageData, new()
        {
            TMessageData data;
            lock (messageDataPools)
            {
                data = messageDataPools.Get<TMessageData>(1).Retrieve();
            }
            data.Reset();
            return data;
        }

        /// <summary>
        /// Subscribes a receiver to the specified message type.
        /// </summary>
        /// <param name="receiver">The receiver to subscribe to the specified message type.</param>
        /// <param name="type">The message type to which to subscribe the receiver.</param>
        public void Subscribe(IMessageSubscriber<TMessageType> receiver, TMessageType type)
        {
            lock (subscribers)
            {
                subscribers[type].Add(receiver);
            }
        }

        /// <summary>
        /// Unsubcribes a receiver from all message types.
        /// </summary>
        /// <param name="receiver">The receiver to unsubscribe from all message types.</param>
        public void Unsubscribe(IMessageSubscriber<TMessageType> receiver)
        {
            lock (subscribers)
            {
                subscribers.RemoveFromAll(receiver);
            }
        }

        /// <summary>
        /// Unsubscribes a receiver from the specified message type.
        /// </summary>
        /// <param name="receiver">The receiver to unsubscribe from the specified message type.</param>
        /// <param name="type">The message type from which to unsubscribe the receiver.</param>
        public void Unsubscribe(IMessageSubscriber<TMessageType> receiver, TMessageType type)
        {
            lock (subscribers)
            {
                subscribers[type].Remove(receiver);
            }
        }

        /// <summary>
        /// Publishes a message to the queue.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        public void Publish(TMessageType type, MessageData data)
        {
            // If the specified data is potentially mergable, scan the queue
            // for an existing message that can be merged with it and perform the merge.
            if (data != null && data.IsMergeable)
            {
                lock (queue)
                {
                    foreach (var enqueuedMessage in queue)
                    {
                        if (enqueuedMessage.Type.Equals(type))
                            continue;

                        var merged = data.EvaluateMerge(enqueuedMessage.Data);
                        if (merged != null)
                        {
                            if (!enqueuedMessage.Data.GetType().GetTypeInfo().IsAssignableFrom(merged.GetType().GetTypeInfo()))
                                throw new InvalidOperationException(NucleusStrings.MessageMergeTypeConflict);
                            
                            enqueuedMessage.Data = merged;
                            return;
                        }
                    }
                }
            }

            // If the message could not be merged, add it to the message queue.
            lock (queue)
            {
                queue.Enqueue(CreateEnqueuedMessage(type, data));
            }
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        public void Process()
        {
            while (true)
            {
                // Retrieve the next message from the queue.
                EnqueuedMessage evt;
                lock (queue)
                {
                    if (queue.Count == 0)
                    {
                        break;
                    }
                    evt = queue.Dequeue();
                }

                // Broadcast the message to all subscribers.
                try
                {
                    lock (subscribers)
                    {
                        subscribers.ReceiveMessage(evt.Type, evt.Data);
                    }
                }
                finally
                {
                    if (evt.Data != null)
                    {
                        lock (messageDataPools)
                        {
                            var pool = messageDataPools.Get(evt.Data.GetType());
                            if (pool == null)
                            {
                                throw new InvalidOperationException(NucleusStrings.MessagePoolNotFound);
                            }
                            pool.Release(evt.Data);
                        }
                    }
                    lock (queue)
                    {
                        queuePool.Release(evt);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an enqueued message.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        /// <returns>The enqueued message.</returns>
        private EnqueuedMessage CreateEnqueuedMessage(TMessageType type, MessageData data)
        {
            var evt = queuePool.Retrieve();
            evt.Type = type;
            evt.Data = data;
            return evt;
        }

        // The queue's message data pools.
        private readonly ExpandingPoolRegistry messageDataPools = 
            new ExpandingPoolRegistry();
 
        // The queue's pending messages.
        private readonly Queue<EnqueuedMessage> queue;
        private readonly IPool<EnqueuedMessage> queuePool;

        // The queue's subscriber registry.
        private readonly SubscriberCollection<TMessageType> subscribers = 
            new SubscriberCollection<TMessageType>();
    }
}
