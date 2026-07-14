# VehicleCRM Tests Agent - Documentação para Testes Unitários

## Visão Geral

Este documento serve como agente especializado para criação e manutenção de testes unitários no projeto **VehicleCRM.Test**.
O projeto utiliza **xUnit** como framework de testes, **NSubstitute** para mocks e **FluentAssertions** para assertions fluentes.

---

## Estrutura do Projeto

O projeto de testes espelha a estrutura do projeto principal, organizado por camadas:

```
VehicleCRM.Test/
├── Application/
│   └── Features/
│       ├── Customers/
│       │   └── Commands/
│       │       ├── CreateCustomer/
│       │       ├── UpdateCustomer/
│       │       └── DeleteCustomer/
│       ├── SaleOpportunities/
│       │   └── Commands/
│       └── Vehicles/
│           └── Commands/
├── Domain/
│   ├── Customers/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   └── Exceptions/
│   ├── SaleOpportunities/
│   │   ├── Entities/
│   │   ├── Services/
│   │   └── Exceptions/
│   └── Vehicles/
│       ├── Entities/
│       └── Exceptions/
```

---

## Bibliotecas e Frameworks

### Tecnologias Utilizadas

- **Framework de Testes**: xUnit 2.9.2
- **Mocking**: NSubstitute 5.3.0
- **Assertions**: FluentAssertions 7.0.0
- **Test Runner**: xUnit.runner.visualstudio 2.8.2
- **SDK de Testes**: Microsoft.NET.Test.Sdk 17.11.1
- **Target Framework**: .NET 10.0

### Usings Globais

O projeto possui os seguintes usings globais configurados no arquivo `.csproj`:

```xml
<Using Include="Xunit" />
<Using Include="NSubstitute" />
<Using Include="FluentAssertions" />
```

---

## Padrões e Convenções de Testes

### 1. Nomenclatura de Classes de Teste

Para cada classe/componente testado, criar uma classe de teste correspondente com o sufixo `Tests`:

- **Handler**: `CreateCustomerCommandHandlerTests`
- **Validator**: `CreateCustomerCommandValidatorTests`
- **Entity**: `CustomerTests`
- **ValueObject**: `EmailTests`
- **Service**: `SaleOpportunityDomainServiceTests`
- **Exception**: `DuplicateCustomerEmailExceptionTests`

### 2. Nomenclatura de Métodos de Teste

Seguir o padrão: `MethodName_Scenario_ExpectedBehavior`

**Exemplos:**
```csharp
Handle_WithValidCommand_ShouldCreateCustomerAndReturnCreatedResponse()
Handle_WhenEmailAlreadyExists_ShouldThrowDuplicateCustomerEmailException()
Create_WithValidData_ShouldCreateCustomer()
Validate_WithEmptyName_ShouldHaveValidationError()
```

### 3. Padrão AAA (Arrange-Act-Assert)

Todos os testes devem seguir o padrão AAA com comentários explícitos:

```csharp
[Fact]
public async Task Handle_WithValidCommand_ShouldCreateCustomerAndReturnCreatedResponse()
{
	// Arrange
	var command = new CreateCustomerCommand(...);
	_repositoryMock.Setup(...);

	// Act
	var result = await _handler.Handle(command, CancellationToken.None);

	// Assert
	result.Should().NotBeNull();
	result.Should().BeOfType<EntityCreatedResponse>();
}
```

---

## Testes de Handlers (Application Layer)

### Estrutura Padrão

```csharp
public class CreateCustomerCommandHandlerTests
{
	private readonly ICustomerRepository _repositoryMock;
	private readonly CreateCustomerCommandHandler _handler;

	public CreateCustomerCommandHandlerTests()
	{
		_repositoryMock = Substitute.For<ICustomerRepository>();
		_handler = new CreateCustomerCommandHandler(_repositoryMock);
	}

	[Fact]
	public async Task Handle_WithValidCommand_ShouldCreateCustomerAndReturnCreatedResponse()
	{
		// Arrange
		var command = new CreateCustomerCommand(...);

		_repositoryMock
			.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
			.Returns(false);

		_repositoryMock
			.InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType<EntityCreatedResponse>();
		result.Id.Should().BeGreaterThan(0);
	}
}
```

### Configuração de Mocks com NSubstitute

**Retorno simples:**
```csharp
_repositoryMock
	.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
	.Returns(false);
```

**Retorno com callback (AndDoes):**
```csharp
_repositoryMock
	.InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
	.Returns(Task.CompletedTask)
	.AndDoes(callInfo =>
	{
		var customer = callInfo.Arg<Customer>();
		typeof(Customer).GetProperty(nameof(Customer.Id))!.SetValue(customer, 1L);
	});
```

### Testes de Exceções

```csharp
[Fact]
public async Task Handle_WhenEmailAlreadyExists_ShouldThrowDuplicateCustomerEmailException()
{
	// Arrange
	var command = new CreateCustomerCommand(...);

	_repositoryMock
		.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
		.Returns(true);

	// Act
	Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

	// Assert
	await act.Should().ThrowAsync<DuplicateCustomerEmailException>()
		.WithMessage("*joao@example.com*");
}
```

---

## Testes de Validators (FluentValidation)

### Estrutura Padrão

```csharp
public class CreateCustomerCommandValidatorTests
{
	private readonly CreateCustomerCommandValidator _validator;

	public CreateCustomerCommandValidatorTests()
	{
		_validator = new CreateCustomerCommandValidator();
	}

	[Fact]
	public void Validate_WithValidData_ShouldNotHaveValidationErrors()
	{
		// Arrange
		var command = new CreateCustomerCommand(...);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void Validate_WithEmptyName_ShouldHaveValidationError()
	{
		// Arrange
		var command = new CreateCustomerCommand("", ...);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(x => x.Name)
			.WithErrorMessage("Nome é obrigatório.");
	}
}
```

### Métodos do FluentValidation.TestHelper

- `TestValidate(command)` - Executa a validação
- `ShouldNotHaveAnyValidationErrors()` - Verifica que não há erros
- `ShouldHaveValidationErrorFor(x => x.Property)` - Verifica erro em propriedade específica
- `WithErrorMessage("mensagem esperada")` - Valida mensagem de erro específica

---

## Testes de Entidades (Domain Layer)

### Testes de Criação

```csharp
[Fact]
public void Create_WithValidData_ShouldCreateCustomer()
{
	// Arrange
	var name = "João Silva";
	var email = "joao@example.com";
	var phone = "(11) 98765-4321";
	var mainInterest = CustomerMainInterest.Sedan;

	// Act
	var customer = Customer.Create(name, email, phone, mainInterest);

	// Assert
	customer.Should().NotBeNull();
	customer.Name.Should().Be(name);
	customer.Email.Value.Should().Be("joao@example.com");
	customer.Phone.Value.Should().Be("11987654321");
	customer.MainInterest.Should().Be(mainInterest);
}
```

### Testes de Comportamento

```csharp
[Fact]
public void Update_WithValidData_ShouldUpdateCustomerProperties()
{
	// Arrange
	var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
	var newName = "João Silva Santos";
	var newPhone = "(11) 91234-5678";
	var newInterest = CustomerMainInterest.Suv;

	// Act
	customer.Update(newName, newPhone, newInterest);

	// Assert
	customer.Name.Should().Be(newName);
	customer.Phone.Value.Should().Be("11912345678");
	customer.MainInterest.Should().Be(newInterest);
}
```

---

## Testes de Value Objects

### Testes de Criação e Normalização

```csharp
[Fact]
public void Create_WithUppercaseEmail_ShouldNormalizeToLowercase()
{
	// Arrange
	var emailString = "TEST@EXAMPLE.COM";

	// Act
	var email = Email.Create(emailString);

	// Assert
	email.Value.Should().Be("test@example.com");
}
```

### Testes de Igualdade

```csharp
[Fact]
public void Equals_WithSameValue_ShouldReturnTrue()
{
	// Arrange
	var email1 = Email.Create("test@example.com");
	var email2 = Email.Create("test@example.com");

	// Act & Assert
	email1.Should().Be(email2);
}
```

---

## Testes de Domain Services

### Estrutura Padrão

```csharp
public class SaleOpportunityDomainServiceTests
{
	private readonly ISaleOpportunityRepository _repositoryMock;
	private readonly SaleOpportunityDomainService _service;

	public SaleOpportunityDomainServiceTests()
	{
		_repositoryMock = Substitute.For<ISaleOpportunityRepository>();
		_service = new SaleOpportunityDomainService(_repositoryMock);
	}

	[Fact]
	public async Task ValidateNewSaleOpportunityAsync_WhenVehicleIsAvailable_AndNoExistingOpportunity_ShouldNotThrowException()
	{
		// Arrange
		var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
		var vehicle = Vehicle.Create("Toyota", "Corolla", 2023, 85000m, "Branco", 15000);

		_repositoryMock
			.HasActiveOpportunityForVehicleAsync(Arg.Any<long>(), Arg.Any<long?>(), Arg.Any<CancellationToken>())
			.Returns(false);

		// Act
		Func<Task> act = async () => await _service.ValidateNewSaleOpportunityAsync(customer, vehicle);

		// Assert
		await act.Should().NotThrowAsync();
	}
}
```

---

## Testes de Exceções Customizadas

```csharp
[Fact]
public void Constructor_ShouldSetPropertiesCorrectly()
{
	// Arrange
	var email = "test@example.com";

	// Act
	var exception = new DuplicateCustomerEmailException(email);

	// Assert
	exception.Email.Should().Be(email);
	exception.Message.Should().Contain(email);
}
```

---

## FluentAssertions - Principais Métodos

### Comparações Básicas
```csharp
result.Should().Be(expected);
result.Should().NotBe(unexpected);
result.Should().BeNull();
result.Should().NotBeNull();
```

### Strings
```csharp
text.Should().Contain("substring");
text.Should().StartWith("prefix");
text.Should().EndWith("suffix");
text.Should().BeEmpty();
```

### Coleções
```csharp
collection.Should().HaveCount(3);
collection.Should().Contain(item);
collection.Should().BeEmpty();
collection.Should().NotContain(item);
```

### Números
```csharp
number.Should().BeGreaterThan(10);
number.Should().BeLessThan(100);
number.Should().BeInRange(1, 10);
```

### Tipos
```csharp
obj.Should().BeOfType<Customer>();
obj.Should().BeAssignableTo<IEntity>();
```

### Exceções (Assíncronas)
```csharp
await act.Should().ThrowAsync<CustomException>();
await act.Should().ThrowAsync<CustomException>()
	.WithMessage("*expected text*");
await act.Should().NotThrowAsync();
```

### Exceções (Síncronas)
```csharp
act.Should().Throw<CustomException>();
act.Should().Throw<CustomException>()
	.WithMessage("*expected text*");
act.Should().NotThrow();
```

---

## NSubstitute - Principais Recursos

### Criação de Mocks
```csharp
var mock = Substitute.For<IRepository>();
```

### Configuração de Retornos
```csharp
// Retorno simples
mock.GetAsync(1).Returns(customer);

// Retorno de Task
mock.GetAsync(1).Returns(Task.FromResult(customer));

// Retorno de ValueTask
mock.GetAsync(1).Returns(new ValueTask<Customer>(customer));
```

### Matchers de Argumentos
```csharp
// Qualquer valor
mock.Method(Arg.Any<int>()).Returns(result);

// Valor específico
mock.Method(Arg.Is<int>(x => x > 10)).Returns(result);

// Qualquer CancellationToken
mock.MethodAsync(Arg.Any<CancellationToken>()).Returns(result);
```

### Callbacks
```csharp
mock.MethodAsync(Arg.Any<Entity>(), Arg.Any<CancellationToken>())
	.Returns(Task.CompletedTask)
	.AndDoes(callInfo =>
	{
		var entity = callInfo.Arg<Entity>();
		// Modificar entity
	});
```

### Verificação de Chamadas
```csharp
// Verificar que foi chamado
await mock.Received().MethodAsync(Arg.Any<CancellationToken>());

// Verificar número de chamadas
await mock.Received(2).MethodAsync(Arg.Any<CancellationToken>());

// Verificar que NÃO foi chamado
await mock.DidNotReceive().MethodAsync(Arg.Any<CancellationToken>());
```

---

## Atributos xUnit

### Fact
Marca um método como teste unitário simples:
```csharp
[Fact]
public void MyTest() { }
```

### Theory com InlineData
Para testes parametrizados:
```csharp
[Theory]
[InlineData("test@example.com", true)]
[InlineData("invalid-email", false)]
[InlineData("", false)]
public void Validate_Email(string email, bool isValid)
{
	// Test implementation
}
```

### MemberData
Para dados complexos:
```csharp
public static IEnumerable<object[]> TestData =>
	new List<object[]>
	{
		new object[] { "João", "joao@test.com" },
		new object[] { "Maria", "maria@test.com" }
	};

[Theory]
[MemberData(nameof(TestData))]
public void Test_WithMemberData(string name, string email)
{
	// Test implementation
}
```

---

## Organização de Testes por Camada

### Application Layer
- **Command Handlers**: Testar lógica de orquestração, chamadas a repositórios, mapeamentos
- **Validators**: Testar todas as regras de validação (campos obrigatórios, formatos, limites)

### Domain Layer
- **Entities**: Testar métodos de criação, atualização, comportamentos de negócio
- **Value Objects**: Testar criação, validação, normalização e igualdade
- **Domain Services**: Testar lógica de negócio complexa que envolve múltiplas entidades
- **Exceptions**: Testar construção e propriedades das exceções customizadas

---

## Cenários de Teste

### 1. Cenário de Sucesso (Happy Path)
Sempre criar primeiro o teste do caminho feliz:
```csharp
[Fact]
public async Task Handle_WithValidCommand_ShouldCreateCustomer()
{
	// Teste do cenário ideal onde tudo funciona corretamente
}
```

### 2. Cenários de Validação
Testar todas as validações de entrada:
```csharp
[Fact]
public void Validate_WithEmptyName_ShouldHaveValidationError()

[Fact]
public void Validate_WithInvalidEmail_ShouldHaveValidationError()

[Fact]
public void Validate_WithNameExceedingMaxLength_ShouldHaveValidationError()
```

### 3. Cenários de Exceção
Testar todas as exceções de negócio:
```csharp
[Fact]
public async Task Handle_WhenEmailAlreadyExists_ShouldThrowException()

[Fact]
public async Task Create_WhenVehicleNotAvailable_ShouldThrowException()
```

### 4. Cenários de Edge Cases
Testar casos limites:
```csharp
[Fact]
public void Create_WithNullValue_ShouldHandleCorrectly()

[Fact]
public void Create_WithEmptyString_ShouldHandleCorrectly()

[Fact]
public void Update_WithSameValues_ShouldNotChangeState()
```

### 5. Cenários de Verificação de Comportamento
Verificar que os métodos corretos foram chamados:
```csharp
[Fact]
public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
{
	// Arrange & Act
	await _handler.Handle(command, CancellationToken.None);

	// Assert
	await _repositoryMock.Received().InsertAsync(
		Arg.Is<Customer>(c => c.Name == command.Name),
		Arg.Any<CancellationToken>()
	);
}
```

---

## Diretrizes Gerais

### ✅ FAZER

1. **Usar dados realistas** nos testes (nomes brasileiros, emails válidos, telefones formatados)
2. **Manter testes independentes** - cada teste deve poder rodar isoladamente
3. **Nomear claramente** - o nome do teste deve descrever exatamente o que está sendo testado
4. **Seguir o padrão AAA** - sempre separar Arrange, Act e Assert com comentários
5. **Testar um comportamento por teste** - não misturar múltiplas asserções não relacionadas
6. **Usar FluentAssertions** para assertions mais legíveis
7. **Mockar apenas dependências externas** - não mockar classes do próprio domínio
8. **Cobrir cenários de exceção** - testar tanto o happy path quanto os casos de erro

### ❌ NÃO FAZER

1. **Não mockar entidades de domínio** - usar instâncias reais das entidades
2. **Não deixar testes dependentes entre si** - cada teste deve preparar seu próprio estado
3. **Não usar valores mágicos** - sempre declarar variáveis com nomes significativos
4. **Não testar detalhes de implementação** - focar no comportamento público
5. **Não misturar testes de unidade com testes de integração**
6. **Não ignorar testes falhando** - corrigir ou remover testes quebrados

---

## Cobertura de Código

### Prioridade de Testes

**Alta Prioridade:**
- Lógica de negócio crítica (Domain Layer)
- Regras de validação (Validators)
- Handlers que orquestram múltiplas operações

**Média Prioridade:**
- Value Objects
- Exception customizadas
- Mapeamentos complexos

**Baixa Prioridade:**
- DTOs simples
- Models de resposta sem lógica
- Configurações estáticas

---

## Regras de Isolamento

### ⚠️ IMPORTANTE: Restrições de Modificação

1. **NÃO ALTERAR arquivos fora do projeto de testes** (`tests/VehicleCRM.Test/`)
2. **NÃO MODIFICAR código de produção** para acomodar testes
3. **APENAS TESTAR** o comportamento público das classes
4. **USAR MOCKS** para isolar dependências externas

### 🔒 Política de Instalação de Bibliotecas

**SEMPRE PEDIR PERMISSÃO** ao usuário antes de:
- Adicionar novas bibliotecas/packages ao projeto de testes
- Atualizar versões de bibliotecas existentes
- Instalar ferramentas de teste adicionais

**Bibliotecas pré-aprovadas** (já presentes no projeto):
- xUnit 2.9.2
- NSubstitute 5.3.0
- FluentAssertions 7.0.0
- xUnit.runner.visualstudio 2.8.2
- Microsoft.NET.Test.Sdk 17.11.1

---

## Exemplos de Testes Completos

### Exemplo 1: Teste de Handler

```csharp
public class CreateCustomerCommandHandlerTests
{
	private readonly ICustomerRepository _repositoryMock;
	private readonly CreateCustomerCommandHandler _handler;

	public CreateCustomerCommandHandlerTests()
	{
		_repositoryMock = Substitute.For<ICustomerRepository>();
		_handler = new CreateCustomerCommandHandler(_repositoryMock);
	}

	[Fact]
	public async Task Handle_WithValidCommand_ShouldCreateCustomerAndReturnCreatedResponse()
	{
		// Arrange
		var command = new CreateCustomerCommand(
			Name: "João Silva",
			Email: "joao@example.com",
			Phone: "(11) 98765-4321",
			MainInterest: CustomerMainInterest.Sedan
		);

		_repositoryMock
			.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
			.Returns(false);

		_repositoryMock
			.InsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType<EntityCreatedResponse>();
		result.Id.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task Handle_WhenEmailAlreadyExists_ShouldThrowDuplicateCustomerEmailException()
	{
		// Arrange
		var command = new CreateCustomerCommand(
			"João Silva",
			"joao@example.com",
			"11987654321",
			CustomerMainInterest.Sedan
		);

		_repositoryMock
			.ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
			.Returns(true);

		// Act
		Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

		// Assert
		await act.Should().ThrowAsync<DuplicateCustomerEmailException>()
			.WithMessage("*joao@example.com*");
	}
}
```

### Exemplo 2: Teste de Validator

```csharp
public class CreateCustomerCommandValidatorTests
{
	private readonly CreateCustomerCommandValidator _validator;

	public CreateCustomerCommandValidatorTests()
	{
		_validator = new CreateCustomerCommandValidator();
	}

	[Fact]
	public void Validate_WithValidData_ShouldNotHaveValidationErrors()
	{
		// Arrange
		var command = new CreateCustomerCommand(
			Name: "João Silva",
			Email: "joao@example.com",
			Phone: "(11) 98765-4321",
			MainInterest: CustomerMainInterest.Sedan
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	public void Validate_WithInvalidName_ShouldHaveValidationError(string invalidName)
	{
		// Arrange
		var command = new CreateCustomerCommand(
			invalidName,
			"joao@example.com",
			"11987654321",
			CustomerMainInterest.Sedan
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(x => x.Name);
	}
}
```

### Exemplo 3: Teste de Entity

```csharp
public class CustomerTests
{
	[Fact]
	public void Create_WithValidData_ShouldCreateCustomer()
	{
		// Arrange
		var name = "João Silva";
		var email = "joao@example.com";
		var phone = "(11) 98765-4321";
		var mainInterest = CustomerMainInterest.Sedan;

		// Act
		var customer = Customer.Create(name, email, phone, mainInterest);

		// Assert
		customer.Should().NotBeNull();
		customer.Name.Should().Be(name);
		customer.Email.Value.Should().Be("joao@example.com");
		customer.Phone.Value.Should().Be("11987654321");
		customer.MainInterest.Should().Be(mainInterest);
	}

	[Fact]
	public void Create_WithEmailInUppercase_ShouldNormalizeEmail()
	{
		// Arrange
		var email = "JOAO@EXAMPLE.COM";

		// Act
		var customer = Customer.Create("João Silva", email, "11987654321", CustomerMainInterest.Sedan);

		// Assert
		customer.Email.Value.Should().Be("joao@example.com");
	}

	[Fact]
	public void Update_WithValidData_ShouldUpdateCustomerProperties()
	{
		// Arrange
		var customer = Customer.Create("João Silva", "joao@example.com", "11987654321", CustomerMainInterest.Sedan);
		var newName = "João Silva Santos";
		var newPhone = "(11) 91234-5678";
		var newInterest = CustomerMainInterest.Suv;

		// Act
		customer.Update(newName, newPhone, newInterest);

		// Assert
		customer.Name.Should().Be(newName);
		customer.Phone.Value.Should().Be("11912345678");
		customer.MainInterest.Should().Be(newInterest);
	}
}
```

---

## Comandos Úteis

### Executar Todos os Testes
```powershell
dotnet test
```

### Executar Testes com Cobertura
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

### Executar Testes de uma Classe Específica
```powershell
dotnet test --filter "FullyQualifiedName~CreateCustomerCommandHandlerTests"
```

### Executar Testes por Categoria
```powershell
dotnet test --filter "Category=Unit"
```

---

## Conclusão

Este documento deve ser usado como referência ao criar ou modificar testes unitários no projeto VehicleCRM. 
Manter a consistência nos padrões de teste facilita a manutenção e compreensão do código por toda a equipe.

**Lembre-se:**
- ✅ Sempre pedir permissão antes de instalar novas bibliotecas
- ✅ Não alterar arquivos fora do projeto de testes
- ✅ Seguir os padrões estabelecidos neste documento
- ✅ Manter os testes simples, claros e focados
