using System.Text.Json;

namespace TaskTrackerCLI;
    public class TaskService
    {
        public List<TaskModel> ReadFileData(string pathToFile) {
            CreateFileIfNotExists(pathToFile);
            return ReadFileContents(pathToFile);
        }

        public void ProcessCommand(string? command, List<TaskModel> taskModels, string pathToFile) {
            if (command == null) {
                return;
            }
            string commandCopy = command.ToLower();
            if (commandCopy == "list") {
                ListAllTasks(taskModels);
                return;
            }
            if (commandCopy == "list done") {
                ListDoneTasks(taskModels);
                return;
            }
            if (commandCopy == "list todo") {
                ListTodoTasks(taskModels);
                return;
            } 
            if (commandCopy == "list in-progress") {
                ListInProgressTasks(taskModels);
                return;
            } 
            if (commandCopy.StartsWith("mark-in-progress")) {
                int id = GetIdFromCommand(commandCopy);
                if (id == -1) {
                    return;
                } else {
                    UpdateStatus(taskModels, id, "in-progress");
                }
            }
            if (commandCopy.StartsWith("mark-done")) {
                int id = GetIdFromCommand(commandCopy);
                if (id == -1) {
                    return;
                } else {
                    UpdateStatus(taskModels, id, "done");
                }
            }
            if (commandCopy.StartsWith("delete")) {
                int id = GetIdFromCommand(commandCopy);
                if (id == -1) {
                    return;
                } else {
                    taskModels.RemoveAll(taskModel => taskModel.Id == id);
                }
            }
            if (commandCopy.StartsWith("add")) {
                AddTask(taskModels, command);
            }
            if (commandCopy.StartsWith("update")) {
                UpdateTask(taskModels, command);
            }
            UpdateFileContents(pathToFile, taskModels);
        }

        private void UpdateFileContents(string pathToFile, List<TaskModel> taskModels) {
            string jsonString = JsonSerializer.Serialize(taskModels);
            File.WriteAllText(pathToFile, jsonString);
        }

        public int GetIdFromCommand(string commandCopy) {
            string[] commandParts = commandCopy.Split(" ");
            if (commandParts.Length < 2) {
                return -1;
            }
            return int.Parse(commandParts[1]);
        }

        public void UpdateTask(List<TaskModel> taskModels, string command) {
            string[] commandParts = command.Split(" ", 2);
            string[] parts = commandParts[1].Split(" ", 2);
            int id = int.Parse(parts[0]);
            string description = parts[1];
            TaskModel? taskModel = taskModels.FirstOrDefault(taskModel => taskModel.Id == id);
            if (taskModel == null) {
                Console.WriteLine("Task not found.");
                return;
            }
            taskModel.Description = description;
            taskModel.Updated_at = DateTime.Now;
        }

        public void AddTask(List<TaskModel> taskModels, string command) {
            string[] commandParts = command.Split(" ", 2);
            string description = commandParts[1];
            int Id = GetUniqueId(taskModels);
            TaskModel taskModel = new TaskModel(Id, description, "todo", DateTime.Now, DateTime.Now);
            taskModels.Add(taskModel);
        }

        private int GetUniqueId(List<TaskModel> taskModels) {
            int maxId = 0;
            foreach (TaskModel taskModel in taskModels) {
                if (taskModel.Id > maxId) {
                    maxId = taskModel.Id;
                }
            }
            return maxId + 1;
        }

        public void UpdateStatus(List<TaskModel> taskModels, int id, string status) {
            TaskModel? taskModel = taskModels.FirstOrDefault(taskModel => taskModel.Id == id);
            if (taskModel == null) {
                Console.WriteLine("Task not found.");
                return;
            }
            taskModel.Status = status;
            taskModel.Updated_at = DateTime.Now;
        }

        private void ListAllTasks(List<TaskModel> taskModels) {
            foreach (TaskModel taskModel in taskModels) {
                Console.WriteLine(taskModel);
            }
        }

        private void ListDoneTasks(List<TaskModel> taskModels) {
            foreach (TaskModel taskModel in taskModels) {
                if (taskModel.Status == "done") {
                    Console.WriteLine(taskModel);
                }
            }
        }

        private void ListTodoTasks(List<TaskModel> taskModels) {
            foreach (TaskModel taskModel in taskModels) {
                if (taskModel.Status == "todo") {
                    Console.WriteLine(taskModel);
                }
            }
        }

        private void ListInProgressTasks(List<TaskModel> taskModels) {
            foreach (TaskModel taskModel in taskModels) {
                if (taskModel.Status == "in-progress") {
                    Console.WriteLine(taskModel);
                }
            }
        }

        private void CreateFileIfNotExists(string pathToFile) {
            if (File.Exists(pathToFile)) {
                Console.WriteLine("File exists.");
            } else {
                using Stream stream = File.Create(pathToFile);
            }
        }

        private List<TaskModel> ReadFileContents(string pathToFile) {
            string fileContent = File.ReadAllText(pathToFile);
            List<TaskModel> taskModels;
            if (fileContent == "") {
                return [];
            }
            taskModels = JsonSerializer.Deserialize<List<TaskModel>>(fileContent) ?? [];
            return taskModels;
        }


    }