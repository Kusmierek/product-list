namespace ProductCatalog.API.Exceptions;

public class ProductCodeAlreadyExistsException(string code)
    : Exception($"Product with code '{code}' already exists.");
