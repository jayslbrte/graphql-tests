using System.Collections.Generic;

namespace test_acceptance.data;

public class BookQueryResponse
{
    public required List<Book> Books { get; set; }
}

public class Book
{
    public required string Title { get; set; }
    public required Author Author { get; set; }
}

public class Author
{
    public required string Name { get; set; }
}