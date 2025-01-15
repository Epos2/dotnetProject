using TaskTrackerCLI;

string pathToFile = "../TaskTrackerCLI/Task.json";

TaskService taskService = new();
List<TaskModel> taskModels = taskService.ReadFileData(pathToFile);

while (true) {
    Console.WriteLine("Enter a command: ");
    string? command = Console.ReadLine();
    if (command == "exit") {
      break;
    }
    taskService.ProcessCommand(command, taskModels, pathToFile);
    
    
}


