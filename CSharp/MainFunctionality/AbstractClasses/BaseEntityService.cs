using Blazored.LocalStorage;
using FormsClone.CSharp.MainFunctionality.Interfaces;
using System.Text.Json;

namespace FormsClone.CSharp.MainFunctionality.AbstractClasses
{
    public abstract class BaseEntityService<T> : IEntityService<T> where T : class
    {
        private readonly ILocalStorageService _localStorage;

        protected BaseEntityService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected abstract string Prefix { get; }

        public async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entities = await GetAllAsync() ?? new List<T>();
            var idProperty = typeof(T).GetProperty("ID");
            if (idProperty != null)
            {
                var newId = entities.Count > 0
                    ? entities.Max(e => int.Parse(idProperty.GetValue(e)?.ToString() ?? "0")) + 1
                    : 1;
                idProperty.SetValue(entity, newId.ToString());
            }
            entities.Add(entity);
            await SaveEntitiesAsync(entities);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var keys = await _localStorage.KeysAsync();
            var entities = await Task.WhenAll(keys
                .Where(key => key.StartsWith(Prefix))
                .Select(async key => await _localStorage.GetItemAsync<T>(key)));

            return entities.Where(entity => entity != null).Cast<T>().ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var json = await _localStorage.GetItemAsync<string>($"form_{id}");
            if (string.IsNullOrEmpty(json))
            {
                throw new InvalidOperationException("Entity not found.");
            }

            return JsonSerializer.Deserialize<T>(json)!;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existingEntity = await GetByIdAsync(int.Parse(typeof(T).GetProperty("ID")?.GetValue(entity)?.ToString()!));
            foreach (var prop in typeof(T).GetProperties())
            {
                prop.SetValue(existingEntity, prop.GetValue(entity));
            }

            await SaveEntityAsync(existingEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var key = Prefix + id;
            await _localStorage.RemoveItemAsync(key);
        }

        protected async Task SaveEntitiesAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await SaveEntityAsync(entity);
            }
        }

        protected async Task SaveEntityAsync(T entity)
        {
            var key = Prefix + typeof(T).GetProperty("ID")?.GetValue(entity);
            if (key != null && key != "")
            {
                await _localStorage.SetItemAsync(key.ToString(), entity);
            }
        }

       
        public abstract Task<List<T>> SearchAsync(string searchTerm);
    }
}
