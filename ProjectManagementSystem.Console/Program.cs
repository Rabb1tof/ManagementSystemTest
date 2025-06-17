using System;
using System.Threading.Tasks;
using ProjectManagementSystem.Console.Models;
using ProjectManagementSystem.Console.Repositories;
using ProjectManagementSystem.Console.Services;

namespace ProjectManagementSystem.Console
{
    class Program
    {
        private static IAuthService _authService;
        private static IProjectService _projectService;
        private static ITaskService _taskService;
        private static User _currentUser;
        private static IUserRepository _userRepository;

        static async Task Main(string[] args)
        {
            InitializeServices();
            await RunAsync();
        }

        private static void InitializeServices()
        {
            var userRepository = new JsonUserRepository("users.json");
            var projectRepository = new JsonProjectRepository("projects.json");
            var taskRepository = new JsonTaskRepository("tasks.json");

            _authService = new AuthService(userRepository);
            _projectService = new ProjectService(projectRepository);
            _taskService = new TaskService(taskRepository, projectRepository, userRepository);
            _userRepository = userRepository;
        }

        private static async Task RunAsync()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    await ShowLoginMenuAsync();
                }
                else
                {
                    await ShowMainMenuAsync();
                }
            }
        }

        private static async Task ShowLoginMenuAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Система управления проектами ===");
            System.Console.WriteLine("1. Вход");
            System.Console.WriteLine("2. Регистрация (только для управляющих)");
            System.Console.WriteLine("0. Выход");
            System.Console.Write("Выберите действие: ");

            var choice = System.Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await LoginAsync();
                    break;
                case "2":
                    await RegisterAsync();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                    System.Console.ReadKey();
                    break;
            }
        }

        private static async Task ShowMainMenuAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine($"=== Система управления проектами ===");
            System.Console.WriteLine($"Текущий пользователь: {_currentUser.Username} ({_currentUser.Role})");
            System.Console.WriteLine("1. Управление проектами");
            System.Console.WriteLine("2. Управление задачами");
            if (_currentUser.Role == UserRole.Manager)
            {
                System.Console.WriteLine("3. Управление пользователями");
            }
            System.Console.WriteLine("0. Выход");
            System.Console.Write("Выберите действие: ");

            var choice = System.Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowProjectMenuAsync();
                    break;
                case "2":
                    await ShowTaskMenuAsync();
                    break;
                case "3":
                    if (_currentUser.Role == UserRole.Manager)
                    {
                        await ShowUserManagementMenuAsync();
                    }
                    break;
                case "0":
                    _currentUser = null;
                    break;
                default:
                    System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                    System.Console.ReadKey();
                    break;
            }
        }

        private static async Task LoginAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Вход в систему ===");
            System.Console.Write("Имя пользователя: ");
            var username = System.Console.ReadLine();
            System.Console.Write("Пароль: ");
            var password = ReadPassword();

            try
            {
                _currentUser = await _authService.AuthenticateAsync(username, password);
                if (_currentUser == null)
                {
                    System.Console.WriteLine("Неверное имя пользователя или пароль.");
                    System.Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при входе: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task RegisterAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Регистрация нового пользователя ===");
            System.Console.Write("Имя пользователя: ");
            var username = System.Console.ReadLine();
            System.Console.Write("Пароль: ");
            var password = ReadPassword();
            System.Console.Write("Подтвердите пароль: ");
            var confirmPassword = ReadPassword();

            if (password != confirmPassword)
            {
                System.Console.WriteLine("Пароли не совпадают.");
                System.Console.ReadKey();
                return;
            }

            try
            {
                _currentUser = await _authService.RegisterAsync(username, password, UserRole.Manager);
                System.Console.WriteLine("Регистрация успешно завершена.");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при регистрации: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task ShowProjectMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление проектами ===");
                System.Console.WriteLine("1. Создать проект");
                System.Console.WriteLine("2. Просмотреть все проекты");
                System.Console.WriteLine("3. Редактировать проект");
                System.Console.WriteLine("4. Удалить проект");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("Выберите действие: ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateProjectAsync();
                        break;
                    case "2":
                        await ListProjectsAsync();
                        break;
                    case "3":
                        await EditProjectAsync();
                        break;
                    case "4":
                        await DeleteProjectAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task ShowTaskMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление задачами ===");
                System.Console.WriteLine("1. Создать задачу");
                System.Console.WriteLine("2. Просмотреть задачи проекта");
                System.Console.WriteLine("3. Просмотреть мои задачи");
                System.Console.WriteLine("4. Изменить статус задачи");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("Выберите действие: ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateTaskAsync();
                        break;
                    case "2":
                        await ListProjectTasksAsync();
                        break;
                    case "3":
                        await ListMyTasksAsync();
                        break;
                    case "4":
                        await UpdateTaskStatusAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task ShowUserManagementMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Управление пользователями ===");
                System.Console.WriteLine("1. Зарегистрировать нового пользователя");
                System.Console.WriteLine("2. Просмотреть всех пользователей");
                System.Console.WriteLine("0. Назад");
                System.Console.Write("Выберите действие: ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisterNewUserAsync();
                        break;
                    case "2":
                        await ListUsersAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task CreateProjectAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Создание проекта ===");
            System.Console.Write("Название проекта: ");
            var name = System.Console.ReadLine();
            System.Console.Write("Описание проекта: ");
            var description = System.Console.ReadLine();

            try
            {
                var project = await _projectService.CreateProjectAsync(name, description);
                System.Console.WriteLine($"Проект успешно создан с ID: {project.Id}");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при создании проекта: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task ListProjectsAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список проектов ===");

            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                foreach (var project in projects)
                {
                    System.Console.WriteLine($"ID: {project.Id}");
                    System.Console.WriteLine($"Название: {project.Name}");
                    System.Console.WriteLine($"Описание: {project.Description}");
                    System.Console.WriteLine("------------------------");
                }
                System.Console.WriteLine("Нажмите любую клавишу для продолжения...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при получении списка проектов: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task CreateTaskAsync()
        {
            if (_currentUser.Role != UserRole.Manager)
            {
                System.Console.WriteLine("У вас нет прав для создания задач.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Clear();
            System.Console.WriteLine("=== Создание задачи ===");
            System.Console.Write("ID проекта: ");
            if (!int.TryParse(System.Console.ReadLine(), out int projectId))
            {
                System.Console.WriteLine("Неверный формат ID проекта.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Write("Название задачи: ");
            var title = System.Console.ReadLine();
            System.Console.Write("Описание задачи: ");
            var description = System.Console.ReadLine();
            System.Console.Write("ID исполнителя: ");
            if (!int.TryParse(System.Console.ReadLine(), out int assignedToUserId))
            {
                System.Console.WriteLine("Неверный формат ID исполнителя.");
                System.Console.ReadKey();
                return;
            }

            try
            {
                var task = await _taskService.CreateTaskAsync(projectId, title, description, assignedToUserId);
                System.Console.WriteLine($"Задача успешно создана с ID: {task.Id}");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при создании задачи: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task ListProjectTasksAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список задач проекта ===");
            System.Console.Write("Введите ID проекта: ");
            if (!int.TryParse(System.Console.ReadLine(), out int projectId))
            {
                System.Console.WriteLine("Неверный формат ID проекта.");
                System.Console.ReadKey();
                return;
            }

            try
            {
                var tasks = await _taskService.GetTasksByProjectAsync(projectId);
                foreach (var task in tasks)
                {
                    System.Console.WriteLine($"ID: {task.Id}");
                    System.Console.WriteLine($"Название: {task.Title}");
                    System.Console.WriteLine($"Описание: {task.Description}");
                    System.Console.WriteLine($"Статус: {task.Status}");
                    System.Console.WriteLine($"Исполнитель ID: {task.AssignedToUserId}");
                    System.Console.WriteLine("------------------------");
                }
                System.Console.WriteLine("Нажмите любую клавишу для продолжения...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при получении списка задач: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task ListMyTasksAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Мои задачи ===");

            try
            {
                var tasks = await _taskService.GetTasksByUserAsync(_currentUser.Id);
                foreach (var task in tasks)
                {
                    System.Console.WriteLine($"ID: {task.Id}");
                    System.Console.WriteLine($"Название: {task.Title}");
                    System.Console.WriteLine($"Описание: {task.Description}");
                    System.Console.WriteLine($"Статус: {task.Status}");
                    System.Console.WriteLine("------------------------");
                }
                System.Console.WriteLine("Нажмите любую клавишу для продолжения...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при получении списка задач: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task UpdateTaskStatusAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Изменение статуса задачи ===");
            System.Console.Write("Введите ID задачи: ");
            if (!int.TryParse(System.Console.ReadLine(), out int taskId))
            {
                System.Console.WriteLine("Неверный формат ID задачи.");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine("Выберите новый статус:");
            System.Console.WriteLine("1. To Do");
            System.Console.WriteLine("2. In Progress");
            System.Console.WriteLine("3. Done");
            System.Console.Write("Ваш выбор: ");

            if (!int.TryParse(System.Console.ReadLine(), out int statusChoice) || statusChoice < 1 || statusChoice > 3)
            {
                System.Console.WriteLine("Неверный выбор статуса.");
                System.Console.ReadKey();
                return;
            }

            var newStatus = (ProjectTaskStatus)(statusChoice - 1);

            try
            {
                var success = await _taskService.UpdateTaskStatusAsync(taskId, newStatus);
                if (success)
                {
                    System.Console.WriteLine("Статус задачи успешно обновлен.");
                }
                else
                {
                    System.Console.WriteLine("Не удалось обновить статус задачи.");
                }
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при обновлении статуса задачи: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task RegisterNewUserAsync()
        {
            if (_currentUser.Role != UserRole.Manager)
            {
                System.Console.WriteLine("У вас нет прав для регистрации новых пользователей.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Clear();
            System.Console.WriteLine("=== Регистрация нового пользователя ===");
            System.Console.Write("Имя пользователя: ");
            var username = System.Console.ReadLine();
            System.Console.Write("Пароль: ");
            var password = ReadPassword();
            System.Console.Write("Подтвердите пароль: ");
            var confirmPassword = ReadPassword();

            if (password != confirmPassword)
            {
                System.Console.WriteLine("Пароли не совпадают.");
                System.Console.ReadKey();
                return;
            }

            System.Console.WriteLine("Выберите роль:");
            System.Console.WriteLine("1. Сотрудник");
            System.Console.WriteLine("2. Управляющий");
            System.Console.Write("Ваш выбор: ");

            if (!int.TryParse(System.Console.ReadLine(), out int roleChoice) || roleChoice < 1 || roleChoice > 2)
            {
                System.Console.WriteLine("Неверный выбор роли.");
                System.Console.ReadKey();
                return;
            }

            var role = roleChoice == 1 ? UserRole.Employee : UserRole.Manager;

            try
            {
                var user = await _authService.RegisterAsync(username, password, role);
                System.Console.WriteLine($"Пользователь успешно зарегистрирован с ID: {user.Id}");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task EditProjectAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Редактирование проекта ===");
            System.Console.Write("Введите ID проекта: ");
            if (!int.TryParse(System.Console.ReadLine(), out int projectId))
            {
                System.Console.WriteLine("Неверный формат ID проекта.");
                System.Console.ReadKey();
                return;
            }

            try
            {
                var project = await _projectService.GetProjectAsync(projectId);
                if (project == null)
                {
                    System.Console.WriteLine("Проект не найден.");
                    System.Console.ReadKey();
                    return;
                }

                System.Console.WriteLine($"Текущее название: {project.Name}");
                System.Console.Write("Новое название (оставьте пустым, чтобы не менять): ");
                var newName = System.Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    project.Name = newName;
                }

                System.Console.WriteLine($"Текущее описание: {project.Description}");
                System.Console.Write("Новое описание (оставьте пустым, чтобы не менять): ");
                var newDescription = System.Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    project.Description = newDescription;
                }

                var success = await _projectService.UpdateProjectAsync(project);
                if (success)
                {
                    System.Console.WriteLine("Проект успешно обновлен.");
                }
                else
                {
                    System.Console.WriteLine("Не удалось обновить проект.");
                }
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при обновлении проекта: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task DeleteProjectAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Удаление проекта ===");
            System.Console.Write("Введите ID проекта: ");
            if (!int.TryParse(System.Console.ReadLine(), out int projectId))
            {
                System.Console.WriteLine("Неверный формат ID проекта.");
                System.Console.ReadKey();
                return;
            }

            try
            {
                var success = await _projectService.DeleteProjectAsync(projectId);
                if (success)
                {
                    System.Console.WriteLine("Проект успешно удален.");
                }
                else
                {
                    System.Console.WriteLine("Не удалось удалить проект.");
                }
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при удалении проекта: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static async Task ListUsersAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== Список пользователей ===");

            try
            {
                var users = await _userRepository.GetAllAsync();
                foreach (var user in users)
                {
                    System.Console.WriteLine($"ID: {user.Id}");
                    System.Console.WriteLine($"Имя пользователя: {user.Username}");
                    System.Console.WriteLine($"Роль: {user.Role}");
                    System.Console.WriteLine($"Дата создания: {user.CreatedAt}");
                    System.Console.WriteLine("------------------------");
                }
                System.Console.WriteLine("Нажмите любую клавишу для продолжения...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}");
                System.Console.ReadKey();
            }
        }

        private static string ReadPassword()
        {
            var password = "";
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    System.Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        System.Console.Write("\b \b");
                    }
                }
                else
                {
                    password += key.KeyChar;
                    System.Console.Write("*");
                }
            }
            return password;
        }
    }
}
