---
name: aspnet-core-manual-crud-patch
description: Правила и паттерны ручной реализации CRUD-контроллеров в ASP.NET Core с использованием раздельных DTO, Change Tracking в EF Core и Merge Patch.
triggers:
  - реализация или рефакторинг ASP.NET Core Web API контроллеров
  - внедрение DTO и защита от overposting
  - реализация методов PUT и PATCH (Merge Patch)
  - работа с Entity Framework Core Change Tracking без прямого изменения State
---

# Роль и назначение
Навык регламентирует создание API-контроллеров в ASP.NET Core без использования автогенерации (scaffolding). Он обеспечивает защиту от Overposting, корректную работу Change Tracking в EF Core и безопасную реализацию частичного обновления ресурсов через HTTP PATCH (Merge Patch).

---

## 1. Архитектурные требования и ограничения

1. **Запрет работы с Entity напрямую**:
   * Доменные сущности (`Entity`) никогда не принимаются в аргументах публичных методов контроллера и не возвращаются клиенту напрямую.
   * Контроллер взаимодействует с внешним миром исключительно через специализированные DTO.

2. **Запрет `EntityState.Modified` для обновлений**:
   * Запрещено использовать конструкцию `_context.Entry(entity).State = EntityState.Modified`.
   * Сущность должна быть предварительно загружена в контекст через `FirstOrDefault` / `FindAsync` (что автоматически переводит её под отслеживание Change Tracking). Изменения применяются к свойствам загруженного объекта, а EF Core формирует SQL-запрос `UPDATE` только по тем колонкам, значения которых реально изменились.

3. **Обязательная пагинация коллекций**:
   * Перед вызовами `.Skip()` и `.Take()` всегда должен выполняться детерминированный `.OrderBy()` (например, по `Id`).

4. **Фиксация серверного времени**:
   * Метаданные аудита (например, `CreatedAt`) инициализируются явно в коде действия (`DateTime.UtcNow`), фиксируя точный момент обработки запроса.

---

## 2. Спецификация DTO

Для каждой сущности создается 4 отдельных контракта:

| DTO | Назначение | Особенности свойств |
| :--- | :--- | :--- |
| `{Entity}DTO` | Ответ на `GET` (полное представление) | Содержит `Id`, дату создания и все доступные для чтения поля. |
| `Create{Entity}DTO` | Входные данные для `POST` | Без `Id` и `CreatedAt`. Обязательные поля не-nullable. |
| `Update{Entity}DTO` | Входные данные для `PUT` (полная замена) | Без `Id` (берется из маршрута). Все поля обязательны к передаче. |
| `Patch{Entity}Dto` | Входные данные для `PATCH` (Merge Patch) | **Все свойства строго nullable** (`string?`, `int?`, `decimal?` и т.д.). Отсутствие поля в JSON сериализуется как `null`. |

---

## 3. Эталонный код

### Контракты DTO (на примере `Book`)

```csharp
public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? Isbn { get; set; }
    public string? Genre { get; set; }
    public int? PublishedYear { get; set; }
    public decimal Price { get; set; }
    public int CopiesCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBookDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? Isbn { get; set; }
    public string? Genre { get; set; }
    public int? PublishedYear { get; set; }
    public decimal Price { get; set; }
    public int CopiesCount { get; set; }
}

public class UpdateBookDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? Isbn { get; set; }
    public string? Genre { get; set; }
    public int? PublishedYear { get; set; }
    public decimal Price { get; set; }
    public int CopiesCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatchBookDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public string? Genre { get; set; }
    public int? PublishedYear { get; set; }
    public decimal? Price { get; set; }
    public int? CopiesCount { get; set; }
}
```

---

### Контроллер с полным циклом CRUD и Merge Patch

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    private static BookDTO ToDto(Book book) => new BookDTO
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn,
        Genre = book.Genre,
        PublishedYear = book.PublishedYear,
        Price = book.Price,
        CopiesCount = book.CopiesCount,
        CreatedAt = book.CreatedAt
    };

    [HttpGet]
    public IActionResult GetAll(int page = 1, int pageSize = 10)
    {
        var books = _context.Books
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList()
            .Select(ToDto)
            .ToList();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        return Ok(ToDto(book));
    }

    [HttpPost]
    public IActionResult Create(CreateBookDTO dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Isbn = dto.Isbn,
            Genre = dto.Genre,
            PublishedYear = dto.PublishedYear,
            Price = dto.Price,
            CopiesCount = dto.CopiesCount,
            CreatedAt = DateTime.UtcNow
        };

        _context.Books.Add(book);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, ToDto(book));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateBookDTO dto)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        // Полная замена полей
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.Isbn = dto.Isbn;
        book.Genre = dto.Genre;
        book.PublishedYear = dto.PublishedYear;
        book.Price = dto.Price;
        book.CopiesCount = dto.CopiesCount;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, PatchBookDto dto)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        // Merge Patch: применяются только явно переданные свойства
        if (dto.Title is not null) book.Title = dto.Title;
        if (dto.Author is not null) book.Author = dto.Author;
        if (dto.ISBN is not null) book.Isbn = dto.ISBN;
        if (dto.Genre is not null) book.Genre = dto.Genre;
        if (dto.PublishedYear is not null) book.PublishedYear = dto.PublishedYear;
        if (dto.Price is not null) book.Price = dto.Price.Value;
        if (dto.CopiesCount is not null) book.CopiesCount = dto.CopiesCount.Value;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        _context.Books.Remove(book);
        _context.SaveChanges();
        return NoContent();
    }
}
```

---

## 4. Чек-лист верификации реализации

1. **Инвариант PATCH с пустым телом**:
   * При вызове `PATCH /api/{resource}/{id}` с телом `{}` ни одно свойство в базе не должно измениться или занулиться. Если значение обнулилось — пропущена проверка `is not null`.
2. **Точечное изменение (Merge Patch)**:
   * При передаче одиночного поля (например, `{"price": 999}`) обновляется только оно; все остальные свойства ресурса остаются неизменными.
3. **Разграничение PUT и PATCH**:
   * `PUT` требует передачи всех полей ресурса для полной замены.
   * `PATCH` принимает частичный JSON с `nullable`-свойствами.
4. **Контроль Change Tracking**:
   * Перед изменением или удалением сущность обязательно загружается в контекст через `FirstOrDefault(...)`. Никаких ручных манипуляций с `EntityState.Modified`.