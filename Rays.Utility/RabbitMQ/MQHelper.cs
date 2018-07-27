using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rays.Utility.RabbitMQ
{
    public class MQHelper:IDisposable
    {
        private readonly IBus bus;
        public bool isConnected { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">rabbitmq连接字符串</param>
        public MQHelper()
        {
            string connectionString = ConfigurationManager.AppSettings["RabbitMQConnection"].ToString();
            if (string.IsNullOrEmpty(connectionString))
            {
                isConnected = false;
                return;
            }
            bus = RabbitHutch.CreateBus(connectionString);
            if (!bus.IsConnected)
            {
                //如果没有连接上。是否有重连机制。EasyNetQ有重连机制。
                isConnected = false;
            }
            isConnected = true;
        }
        /// <summary>
        /// 发布一条消息(广播)
        /// </summary>
        /// <param name="message"></param>
        public async Task Publish<TMessage>(TMessage message) where TMessage : class
        {
            await bus.PublishAsync(message);
        }

        /// <summary>
        /// 指定Topic，发布一条消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="topic"></param>
        public async Task PublishWithTopic<TMessage>(TMessage message, string topic) where TMessage : class
        {
            if (string.IsNullOrEmpty(topic))
                await Publish(message);
            else
                await bus.PublishAsync(message, x => x.WithTopic(topic));
        }

        /// <summary>
        /// 发布消息。一次性发布多条
        /// </summary>
        /// <param name="messages"></param>
        public async Task PublishMany<TMessage>(List<TMessage> messages) where TMessage : class
        {
            foreach (var message in messages)
            {
                await Publish(message);
                Thread.Sleep(50);//必须加上，以防消息阻塞
            }
        }

        /// <summary>
        /// 发布消息。一次性发布多条
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="topic"></param>
        public async Task PublishManyWithTopic<TMessage>(List<TMessage> messages, string topic) where TMessage : class
        {
            foreach (var message in messages)
            {
                await PublishWithTopic(message, topic);
                Thread.Sleep(50);//必须加上，以防消息阻塞
            }
        }

        /// <summary>
        /// 给指定队列发送一条信息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="message">消息</param>
        public void Send<TMessage>(string queue, TMessage message) where TMessage : class
        {
            bus.Send(queue, message);
        }

        /// <summary>
        /// 给指定队列批量发送信息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="messages">消息</param>
        public void SendMany<TMessage>(string queue, IList<TMessage> messages) where TMessage : class
        {
            foreach (var message in messages)
            {
                SendAsync(queue, message);
                Thread.Sleep(50);//必须加上，以防消息阻塞
            }
        }

        /// <summary>
        /// 给指定队列发送一条信息（异步）
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async void SendAsync<TMessage>(string queue, TMessage message) where TMessage : class
        {
            await bus.SendAsync(queue, message);
        }

        /// <summary>
        /// 从指定队列接收一天信息，并做相关处理。
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="process">
        /// 消息处理委托方法
        /// <para>
        /// <example>
        /// 例如：
        /// <code>
        /// message=>Task.Factory.StartNew(()=>{
        ///     Console.WriteLine(message);
        /// })
        /// </code>
        /// </example>
        /// </para>
        /// </param>
        public void Receive<TMessage>(string queue, Func<TMessage, Task> process) where TMessage : class
        {
            bus.Receive(queue, process);
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="subscriptionId">消息订阅标识</param>
        /// <param name="process">
        /// 消息处理委托方法
        /// <para>
        /// <example>
        /// 例如：
        /// <code>
        /// message=>Task.Factory.StartNew(()=>{
        ///     Console.WriteLine(message);
        /// })
        /// </code>
        /// </example>
        /// </para>
        /// </param>
        public void Subscribe<TMessage>(string subscriptionId, Func<TMessage, Task> process) where TMessage : class
        {
            bus.SubscribeAsync<TMessage>(subscriptionId, process);
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="subscriptionId">消息订阅标识</param>
        /// <param name="process">
        /// 消息处理委托方法
        /// <para>
        /// <example>
        /// 例如：
        /// <code>
        /// message=>Task.Factory.StartNew(()=>{
        ///     Console.WriteLine(message);
        /// })
        /// </code>
        /// </example>
        /// </para>
        /// </param>
        /// <param name="topic">topic</param>
        public async Task SubscribeWithTopic<TMessage>(string subscriptionId, Func<TMessage, Task> process, string topic) where TMessage : class
        {
            if (string.IsNullOrEmpty(topic))
                bus.SubscribeAsync<TMessage>(subscriptionId, process);
            else
                bus.SubscribeAsync<TMessage>(subscriptionId, process, x => x.WithTopic(topic));

        }

        /// <summary>
        /// 自动订阅
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="subscriptionIdPrefix"></param>
        /// <param name="topic"></param>
        public void AutoSubscribe(string assemblyName, string subscriptionIdPrefix, string topic)
        {
            var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
            if (!string.IsNullOrEmpty(topic))
                subscriber.ConfigureSubscriptionConfiguration = x => x.WithTopic(topic);
            subscriber.Subscribe(Assembly.Load(assemblyName));
        }

        public void Dispose()
        {
            if (bus != null)
                bus.Dispose();
        }

        
    }
}
