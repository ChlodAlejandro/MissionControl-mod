using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Commands
{
    public class CommandRegistry
    {

        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        public Command GetMatchingCommand(string command)
        {
            try
            {
                return _commands[command.ToLower()];
            }
            catch
            {
                return null;
            }
        }

        public void RegisterCommand(Command command)
        {
            foreach (string alias in command.GetTriggers())
            {
                _commands.Add(alias.ToLower(), command);
            }
        }

        public void DeregisterCommand(Command command)
        {
            foreach (string i in _commands.Keys)
            {
                if (Equals(_commands[i], command))
                    _commands.Remove(i);
            }
        }

        public void RegisterDefaultCommands()
        {
            RegisterCommand(new VesselsCommand());
        }

    }
}
