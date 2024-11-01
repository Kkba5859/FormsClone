using Blazored.LocalStorage;
using FormsClone.CSharp.MainFunctionality.Interfaces;
using System.Text.Json;

namespace FormsClone.CSharp.MainFunctionality.AbstractClasses
{
    public abstract class BaseEntityService<T> : IEntityService<T> where T : class
    {
        private readonly ILocalStorageService _localStorage; // Локальная переменная для хранения зависимости сервиса локального хранилища.

        protected BaseEntityService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage; // Внедрение зависимости через конструктор.
        }

        protected abstract string Prefix { get; } // Абстрактный префикс, уникальный для каждого типа, задается в наследниках.

        // Асинхронный метод для создания новой сущности.
        public async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity)); // Проверка на null аргумента для предотвращения ошибок.

            var entities = await GetAllAsync() ?? new List<T>(); // Получаем все сущности или создаем новый список, если он пуст.
            var idProperty = typeof(T).GetProperty("ID"); // Получаем свойство "ID" через рефлексию.

            if (idProperty != null)
            {
                // Генерация нового ID на основе максимального ID в коллекции, чтобы ID были уникальными.
                var newId = entities.Count > 0
                    ? entities.Max(e => int.Parse(idProperty.GetValue(e)?.ToString() ?? "0")) + 1
                    : 1;
                idProperty.SetValue(entity, newId.ToString()); // Установка нового ID для создаваемой сущности.
            }

            entities.Add(entity); // Добавление новой сущности в список.
            await SaveEntitiesAsync(entities); // Сохранение обновленного списка сущностей в локальном хранилище.
        }

        // Асинхронный метод для получения всех сущностей из локального хранилища.
        public async Task<List<T>> GetAllAsync()
        {
            var keys = await _localStorage.KeysAsync(); // Получаем все ключи из локального хранилища.
            var entities = await Task.WhenAll(keys
                .Where(key => key.StartsWith(Prefix)) // Отбираем только те ключи, которые начинаются с заданного префикса.
                .Select(async key => await _localStorage.GetItemAsync<T>(key))); // Загружаем данные для каждого подходящего ключа.

            return entities.Where(entity => entity != null).Cast<T>().ToList(); // Формируем и возвращаем список сущностей, исключая значения null.
        }

        // Асинхронный метод для получения сущности по её идентификатору.
        public async Task<T> GetByIdAsync(int id)
        {
            var json = await _localStorage.GetItemAsync<string>($"form_{id}"); // Загружаем данные в формате JSON по заданному ключу.
            if (string.IsNullOrEmpty(json))
            {
                throw new InvalidOperationException("Entity not found."); // Выбрасываем исключение, если сущность не найдена.
            }

            return JsonSerializer.Deserialize<T>(json)!; // Десериализуем JSON обратно в объект типа T.
        }

        // Асинхронный метод для обновления сущности.
        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity)); // Проверка на null аргумента для предотвращения ошибок.

            // Получаем ID сущности и находим существующую сущность с этим ID.
            var existingEntity = await GetByIdAsync(int.Parse(typeof(T).GetProperty("ID")?.GetValue(entity)?.ToString()!));

            // Копируем значения всех свойств из новой сущности в найденную.
            foreach (var prop in typeof(T).GetProperties())
            {
                prop.SetValue(existingEntity, prop.GetValue(entity));
            }

            await SaveEntityAsync(existingEntity); // Сохраняем обновленную сущность в локальном хранилище.
        }

        // Асинхронный метод для удаления сущности по идентификатору.
        public async Task DeleteAsync(int id)
        {
            var key = Prefix + id; // Формируем ключ для удаляемой сущности, объединяя префикс и ID.
            await _localStorage.RemoveItemAsync(key); // Удаляем элемент из локального хранилища по сформированному ключу.
        }

        // Метод для сохранения списка сущностей.
        protected async Task SaveEntitiesAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await SaveEntityAsync(entity); // Сохраняем каждую сущность отдельно.
            }
        }

        // Метод для сохранения одной сущности.
        protected async Task SaveEntityAsync(T entity)
        {
            var key = Prefix + typeof(T).GetProperty("ID")?.GetValue(entity); // Формируем ключ для сущности.
            if (key != null && key != "")
            {
                await _localStorage.SetItemAsync(key.ToString(), entity); // Сохраняем сущность в локальном хранилище.
            }
        }

        // Абстрактный метод поиска, который должен быть реализован в классах-наследниках.
        public abstract Task<List<T>> SearchAsync(string searchTerm);
    }
}
