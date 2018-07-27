using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Utility.RabbitMQ
{
    /// <summary>
    /// RabbitMQ发布/订阅。在发布之前，最好先订阅。因为订阅成功之后才会创建Queues，当你发布一条消息，但是没有订阅的时候，是会丢失的！
    /// 关于subscribeId：如果路由一致的多个相同subscribeId会共轮询理订阅消息，订阅消息之和等于发布消息数。如果是路由一致且多个不同subscribeId会分别处理所有的订阅消息，每个subscribeId对应的Queue订阅消息树等于发布消息。
    /// 简单的说：subscribeId跟Queue有关
    /// 优点：操作简单，消息不会重复消费，可以开多个订阅去消费消息，异步执行。缺点：需要一直保持连接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RabbitMQHelper<T> where T:class,new()
    {
        private static string connection= System.Configuration.ConfigurationManager.AppSettings["RabbitMQConnection"].ToString();
        private static IBus bus ;
        //public RabbitMQHelper()
        //{
        //    if(bus==null|| !bus.IsConnected)
        //    {
        //        bus= RabbitHutch.CreateBus(connection);
        //    }
        //}
        /// <summary>
        /// 异步发布
        /// </summary>
        /// <param name="t">消息体</param>
        /// <param name="topic">主题</param>
        /// <param name="success">成功事件</param>
        /// <param name="fail">失败事件</param>
        public static void MQPublish(T t,string topic,Action success,Action fail)
        {
            bus = Connection();
            bus.PublishAsync(t, topic).ContinueWith(task => {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    success();
                }
                else if (task.IsFaulted)
                {
                    fail();
                }
            });
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subscribeId">接受id</param>
        /// <param name="DoWithMessage">处理消息体事件</param>
        /// <param name="topic">主题</param>
        /// <param name="success">成功事件</param>
        /// <param name="fail">失败时间</param>
        public static void MQSubscribe(string subscribeId,Action<T> DoWithMessage,string topic,Action success, Action fail)
        {
            bus = Connection();
            var subscribeResult = bus.SubscribeAsync<T>(subscribeId, message => Task.Factory.StartNew(() => {
                DoWithMessage(message);
            }).ContinueWith(task => {
                if (task.IsCompleted&&!task.IsFaulted)
                {
                    success();
                }
                else if (task.IsFaulted)
                {
                    fail();
                }
                task.Wait();
            }), o => o.WithTopic(topic));
        }

        
        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseConnection()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public static IBus Connection()
        {
            if (bus == null|| !bus.IsConnected)
            {
                bus = RabbitHutch.CreateBus(connection);
            }
            return bus;
        }
    }
}
