# Brazilian Identifiers Validator Dependency Injection Library

This library provides dependency injection extensions for the NBrzId.Validator package in .NET applications.

## Table of Contents
- [Overview](#overview)
- [Getting Started](#getting-started)
- [Installation](#installation)
- [Usage](#usage)
- [Compatibility](#compatibility)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)

## Overview

This library provides extension methods to easily register Brazilian identifier validators in the .NET dependency injection container:

- **`UseBrzValidators`**: Registers default internal validators for `Cnpj` and `Cpf` and the `IBrzValidator` service for generic validation.
- **`UseValidator<T, TValidator>`**: Registers a specific validator for a given identifier type, allowing custom implementations to be injected.

## Getting Started

### Installation

Add the library via NuGet:

```sh
dotnet add package NBrzId.Validator.DependencyInjection --version 0.2.2
```

## Usage

Registering the default validators and a custom one:

```csharp
// Dependency injection setup
private static void ConfigureServices(IServiceCollection services)
{
    // Registers internal validators (CNPJ and CPF)
    services.UseBrzValidators();

    // Optionally register a custom validator
    services.UseValidator<MyIdentifier, MyValidator>(myIdentifierInstance);
}
```

After registration, the validators can be injected into services or controllers:

```csharp
//Actual injection and usage of the CPF validator
public class MyService
{
    private readonly IBrzIdentifierValidator<Cpf> _cpfValidator;

    public MyService(IBrzIdentifierValidator<Cpf> cpfValidator)
    {
        _cpfValidator = cpfValidator;
    }
}

//Injection and usage of IBrzValidator, which contains operation to validate multiple identifier types
[ApiController]
[Route("api/{controller}")]
public class CustomerController : ControllerBase
{
    private readonly IBrzValidator _brzValidator;

    public MyApiController(IBrzValidator brzValidator)
    {
        _brzValidator = brzValidator;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
    {
        if (_brzValidator.ValidateCnpj(updatedCustomer.CnpjCpf, removeFormatters: false, pad: true)
         && _brzValidator.ValidateCpf(updatedCustomer.CnpjCpf, removeFormatters: false, pad: true))
        {
            return BadRequest("Invalid CNPJ/CPF was provided");
        }

        //[...]

        return Ok("Updated");
    }
}
```

## Compatibility

This library targets .NET Standard 2.0, ensuring compatibility with .NET Core, .NET 5/6/7/8, and .NET Framework 4.6.1+.

## Roadmap

Future releases may include additional identifier types.

## Contributing

Contributions are welcome! Please open an issue to report bugs or discuss improvements.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
