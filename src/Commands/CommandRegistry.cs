using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MissionControl.Commands
{
    public class CommandRegistry
    {
        public CommandRegistry()
        {
            Log.I("Initialized command registry. Registering default commands...");
            RegisterDefaultCommands();
        }

        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        public Command GetMatchingCommand(string command)
        {
            if (!_commands.ContainsKey(command.ToLower())) return null;
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
                Log.I("Registered command \"" + alias.ToLower() + "\" with \"" + command.GetType().Name + "\"");
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
