using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Commands
{
    public class CommandRegistry
    {

        private Dictionary<string, Command> _commands;

        public Command GetMatchingCommand(string command)
        {
            return _commands[command];
        }

        public void RegisterCommand(Command command)
        {
            foreach (string alias in command.GetTriggers())
            {
                _commands.Add(alias, command);
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
