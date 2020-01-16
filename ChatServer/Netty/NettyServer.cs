using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Netty
{
    public class NettyServer
    {
        private IEventLoopGroup bossGroup;
        private IEventLoopGroup workerGroup;
        private IChannel boundChannel;
        private bool running;

        public bool Running { get { return running; } }

        public async void Start()
        {
            if (running)
            {
                return;
            }
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();

            ServerBootstrap bootstrap = new ServerBootstrap()
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Handler(new LoggingHandler("SRV-LSTN"))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                    pipeline.AddLast("framing", new DelimiterBasedFrameDecoder(2048, Delimiters.LineDelimiter()));
                    pipeline.AddLast("echo", new EchoServerHandler());
                }));

            IChannel boundChannel = await bootstrap.BindAsync(2580);
            running = true;
        }

        public void Stop()
        {
            if (!running)
            {
                return;
            }
            boundChannel.CloseAsync();
            Task.WhenAll(
                bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));

            bossGroup = null;
            workerGroup = null;
            boundChannel = null;
            running = false;
        }
    }
}
