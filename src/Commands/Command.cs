using System.Net.Sockets;

namespace MissionControl.Commands
{
    public abstract class Command
    {
        public abstract string[] GetTriggers();
        public abstract string RunCommand(TcpClient sender, string command, string trigger, string[] arguments);
    }
}
