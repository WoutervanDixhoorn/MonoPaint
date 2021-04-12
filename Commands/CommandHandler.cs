using System;
using System.Collections.Generic;

namespace MonoPaint.Commands
{
    public class CommandHandler
    {
        
        List<ICommand> recentCommands;

        public CommandHandler()
        {
            recentCommands = new List<ICommand>();
        }

        public void Execute(ICommand iCommand)
        {
            iCommand.Execute();
            recentCommands.Add(iCommand);
        }

        public void Undo()
        {
            if(recentCommands.Count > 0){
                recentCommands[recentCommands.Count-1].Undo();
                recentCommands.RemoveAt(recentCommands.Count-1);
            }else{
                Console.WriteLine("Nothing there to undo!");
            }
        }
    }
}