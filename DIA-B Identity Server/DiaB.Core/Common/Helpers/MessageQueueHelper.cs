// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     MessageQueueHelper.cs
// Created Date:  2019/04/10 6:51 PM
// ------------------------------------------------------------------------

using System;
using System.IO;

//using System.Messaging;
//using CSS.Core.Web.Extensions;

namespace DiaB.Core.Common.Helpers
{
    public static class MessageQueueHelper
    {
        public const string PrivateQueuePath = @".\Private$\";

        public const string PublicQueuePath = @".\";

        public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(3);

        public static string BuildPath(string path, bool isPublicQueue = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return isPublicQueue ? Path.Combine(PublicQueuePath, path) : Path.Combine(PrivateQueuePath, path);
        }

        //public static MessageQueue InitQueue(string path, bool isPublicQueue = false)
        //{
        //    path = BuildPath(path, isPublicQueue);

        //    if (MessageQueue.Exists(path))
        //    {
        //        return new MessageQueue(path);
        //    }

        //    return MessageQueue.Create(path, true);
        //}

        //public static void DestroyQueue(string path, bool isPublicQueue = false)
        //{
        //    path = BuildPath(path, isPublicQueue);

        //    if (MessageQueue.Exists(path))
        //    {
        //        MessageQueue.Delete(path);
        //    }
        //}

        //public static void Send(string path, bool isPublicQueue, string label, object obj)
        //{
        //    using (var transaction = new MessageQueueTransaction())
        //    {
        //        transaction.Begin();

        //        using (var messageQueue = InitQueue(path, isPublicQueue))
        //        {
        //            var message = new Message
        //                          {
        //                              Recoverable = true,
        //                              Body = obj.ToJson()
        //                          };

        //            messageQueue.Send(message, label, transaction);
        //        }

        //        transaction.Commit();
        //    }
        //}

        //public static Message Receive<T>(string path, bool isPublicQueue, Action<Message> process)
        //{
        //    try
        //    {
        //        Message message = null;
        //        using (var transaction = new MessageQueueTransaction())
        //        {
        //            transaction.Begin();

        //            using (var messageQueue = InitQueue(path, isPublicQueue))
        //            {
        //                if (messageQueue.GetAllMessages().Any())
        //                {
        //                    message = messageQueue.Receive(Timeout, transaction);
        //                    if (message != null && message.BodyStream.Length > 0)
        //                    {
        //                        message.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
        //                        message.Body = message.Body.ToString().ToObject<T>();
        //                    }
        //                }
        //            }

        //            // Process message from queue, rollback if any exception
        //            process(message);

        //            DestroyQueue(path, isPublicQueue);

        //            transaction.Commit();
        //        }

        //        return message;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public static List<Message> ReceiveAll<T>(string path, bool isPublicQueue, Action<List<Message>> process)
        //{
        //    var messages = new List<Message>();
        //    using (var transaction = new MessageQueueTransaction())
        //    {
        //        transaction.Begin();

        //        using (var messageQueue = InitQueue(path, isPublicQueue))
        //        {
        //            while (messageQueue.GetAllMessages().Any())
        //            {
        //                var message = messageQueue.Receive(Timeout, transaction);
        //                if (message != null && message.BodyStream.Length > 0)
        //                {
        //                    message.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
        //                    message.Body = message.Body.ToString().ToObject<T>();
        //                    messages.Add(message);
        //                }
        //            }
        //        }

        //        // Process messages from queue, rollback if any exception 
        //        process(messages);

        //        DestroyQueue(path, isPublicQueue);

        //        transaction.Commit();
        //    }

        //    return messages;
        //}

        //public static void ReceiveAsync<T>(string path, bool isPublicQueue, TimeSpan timeout, object stateObject, Action<Message, ReceiveCompletedEventArgs> process)
        //{
        //    var messageQueue = InitQueue(path);
        //    messageQueue.ReceiveCompleted += (source, agrs) =>
        //                                     {
        //                                         var queue = (MessageQueue)source;
        //                                         if (queue.Id != Guid.Empty)
        //                                         {
        //                                             var message = queue.EndReceive(agrs.AsyncResult);
        //                                             if (message?.BodyStream.Length > 0)
        //                                             {
        //                                                 message.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
        //                                                 message.Body = message.Body.ToString().ToObject<T>();
        //                                                 process(message, agrs);
        //                                             }
        //                                         }

        //                                         queue.BeginReceive(timeout, Guid.NewGuid());
        //                                     };
        //    messageQueue.BeginReceive(timeout, stateObject);
        //}

        //private static void MessageQueueReceiveHandler(object source, ReceiveCompletedEventArgs agrs, Action<Message> process)
        //{
        //    // Connect to the queue
        //    var messageQueue = (MessageQueue)source;

        //    // End the asynchronous Receive operation
        //    var message = messageQueue.EndReceive(agrs.AsyncResult);

        //    // Process message
        //    process(message);

        //    // Restart the asynchronous Receive operation
        //    messageQueue.BeginReceive();
        //}
    }
}
