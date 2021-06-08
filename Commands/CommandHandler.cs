using System;
using System.Collections.Generic;

namespace MonoPaint.Commands
{
    public class CommandHandler
    {

        List<ICommand> recentCommands;
        
        List<ICommand> undidCommands;

        public CommandHandler()
        {
            recentCommands = new List<ICommand>();
            undidCommands = new List<ICommand>();
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
                undidCommands.Add(recentCommands[recentCommands.Count-1]);
                recentCommands.RemoveAt(recentCommands.Count-1);
            }else{
                Console.WriteLine("Nothing there to undo!");
            }
        }

        public void Redo()
        {
            if(undidCommands.Count > 0){
                undidCommands[undidCommands.Count-1].Execute();
                recentCommands.Add(undidCommands[undidCommands.Count-1]);
                undidCommands.RemoveAt(undidCommands.Count-1);
            }else{
                Console.WriteLine("Nothing there to redo!");
            }
        }
    }
}