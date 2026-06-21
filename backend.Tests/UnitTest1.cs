using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UpStock.Models;
using UpStock.Interfaces;
using UpStock.Controllers;
using UpStock.DTOs;
using Microsoft.Extensions.Logging;

namespace UpStock.Tests;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockService;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockService = new Mock<ICategoryService>();
        _controller = new CategoryController(_mockService.Object);
    }

    [Fact]
    public async Task GetCategories_Retorna200_CuandoHayCategorias()
    {
        // Arrange
        var categorias = new List<Category>
        {
            new() { CategoryID = Guid.NewGuid(), NameCategory = "Audio" },
            new() { CategoryID = Guid.NewGuid(), NameCategory = "Iluminación" }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(categorias);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetCategories_Retorna404_CuandoNoHayCategorias()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(new List<Category>());

        // Act
        var result = await _controller.GetCategories();

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PostCategory_Retorna201_CuandoDatosValidos()
    {
        // Arrange
        var categoria = new Category { CategoryID = Guid.NewGuid(), NameCategory = "Sonido" };
        _mockService.Setup(s => s.CreateAsync(categoria)).ReturnsAsync(categoria);

        // Act
        var result = await _controller.PostCategory(categoria);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PostCategory_Retorna400_CuandoNombreEsVacio()
    {
        // Arrange
        var categoria = new Category { CategoryID = Guid.NewGuid(), NameCategory = "" };

        // Act
        var result = await _controller.PostCategory(categoria);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteCategory_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Category?)null);

        // Act
        var result = await _controller.DeleteCategory(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

public class AssetControllerTests
{
    private readonly Mock<IAssetService> _mockService;
    private readonly AssetController _controller;

    public AssetControllerTests()
    {
        _mockService = new Mock<IAssetService>();
        _controller = new AssetController(_mockService.Object);
    }

    [Fact]
    public async Task GetAssets_Retorna200_CuandoHayActivos()
    {
        // Arrange
        var activos = new List<Asset>
        {
            new() { AssetId = Guid.NewGuid(), Name = "Micrófono", CodeId = "MIC-001", CategoryId = Guid.NewGuid(), StatusId = Guid.NewGuid() },
            new() { AssetId = Guid.NewGuid(), Name = "Parlante", CodeId = "PAR-001", CategoryId = Guid.NewGuid(), StatusId = Guid.NewGuid() }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(activos);

        // Act
        var result = await _controller.GetAssets();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetAssets_Retorna404_CuandoNoHayActivos()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(new List<Asset>());

        // Act
        var result = await _controller.GetAssets();

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetAsset_Retorna200_CuandoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var activo = new Asset { AssetId = id, Name = "Micrófono", CodeId = "MIC-001", CategoryId = Guid.NewGuid(), StatusId = Guid.NewGuid() };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(activo);

        // Act
        var result = await _controller.GetAsset(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetAsset_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Asset?)null);

        // Act
        var result = await _controller.GetAsset(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PostAsset_Retorna201_CuandoDatosValidos()
    {
        // Arrange
        var activo = new Asset
        {
            AssetId = Guid.NewGuid(),
            Name = "Micrófono",
            CodeId = "MIC-001",
            CategoryId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };
        _mockService.Setup(s => s.CreateAsync(activo)).ReturnsAsync(activo);

        // Act
        var result = await _controller.PostAsset(activo);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PostAsset_Retorna400_CuandoNombreEsVacio()
    {
        // Arrange
        var activo = new Asset
        {
            AssetId = Guid.NewGuid(),
            Name = "",
            CodeId = "MIC-001",
            CategoryId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.PostAsset(activo);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PostAsset_Retorna400_CuandoCodigoEsVacio()
    {
        // Arrange
        var activo = new Asset
        {
            AssetId = Guid.NewGuid(),
            Name = "Micrófono",
            CodeId = "",
            CategoryId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.PostAsset(activo);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PostAsset_Retorna400_CuandoCategoriaEsVacia()
    {
        // Arrange
        var activo = new Asset
        {
            AssetId = Guid.NewGuid(),
            Name = "Micrófono",
            CodeId = "MIC-001",
            CategoryId = Guid.Empty,
            StatusId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.PostAsset(activo);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PutAsset_Retorna400_CuandoIdsNoCoinciden()
    {
        // Arrange
        var activo = new Asset
        {
            AssetId = Guid.NewGuid(),
            Name = "Micrófono",
            CodeId = "MIC-001",
            CategoryId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.PutAsset(Guid.NewGuid(), activo);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteAsset_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Asset?)null);

        // Act
        var result = await _controller.DeleteAsset(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

public class StatusControllerTests
{
    private readonly Mock<IStatusService> _mockService;
    private readonly StatusController _controller;

    public StatusControllerTests()
    {
        _mockService = new Mock<IStatusService>();
        _controller = new StatusController(_mockService.Object);
    }

    [Fact]
    public async Task GetStatuses_Retorna200_CuandoHayEstados()
    {
        // Arrange
        var estados = new List<Status>
        {
            new() { StatusId = Guid.NewGuid(), NameStatus = "Disponible" },
            new() { StatusId = Guid.NewGuid(), NameStatus = "En uso" }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(estados);

        // Act
        var result = await _controller.GetStatuses();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetStatuses_Retorna404_CuandoNoHayEstados()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(new List<Status>());

        // Act
        var result = await _controller.GetStatuses();

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetStatus_Retorna200_CuandoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var estado = new Status { StatusId = id, NameStatus = "Disponible" };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(estado);

        // Act
        var result = await _controller.GetStatus(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetStatus_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Status?)null);

        // Act
        var result = await _controller.GetStatus(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PostStatus_Retorna201_CuandoDatosValidos()
    {
        // Arrange
        var estado = new Status { StatusId = Guid.NewGuid(), NameStatus = "Reservado" };
        _mockService.Setup(s => s.CreateAsync(estado)).ReturnsAsync(estado);

        // Act
        var result = await _controller.PostStatus(estado);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PostStatus_Retorna400_CuandoNombreEsVacio()
    {
        // Arrange
        var estado = new Status { StatusId = Guid.NewGuid(), NameStatus = "" };

        // Act
        var result = await _controller.PostStatus(estado);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PutStatus_Retorna400_CuandoIdsNoCoinciden()
    {
        // Arrange
        var estado = new Status { StatusId = Guid.NewGuid(), NameStatus = "Disponible" };

        // Act
        var result = await _controller.PutStatus(Guid.NewGuid(), estado);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteStatus_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Status?)null);

        // Act
        var result = await _controller.DeleteStatus(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

public class ClientsControllerTests
{
    private readonly Mock<IClientService> _mockService;
    private readonly ClientsController _controller;

    public ClientsControllerTests()
    {
        _mockService = new Mock<IClientService>();
        _controller = new ClientsController(_mockService.Object);
    }

    [Fact]
    public async Task GetClients_Retorna200_CuandoHayClientes()
    {
        // Arrange
        var clientes = new List<Client>
        {
            new() { ClientID = Guid.NewGuid(), Name = "Juan Pérez", DniCuit = "20-12345678-9", Phone = "1123456789" },
            new() { ClientID = Guid.NewGuid(), Name = "María García", DniCuit = "27-98765432-1", Phone = "1187654321" }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(clientes);

        // Act
        var result = await _controller.GetClients();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetClient_Retorna200_CuandoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cliente = new Client { ClientID = id, Name = "Juan Pérez", DniCuit = "20-12345678-9", Phone = "1123456789" };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(cliente);

        // Act
        var result = await _controller.GetClient(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetClient_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Client?)null);

        // Act
        var result = await _controller.GetClient(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateClient_Retorna201_CuandoDatosValidos()
    {
        // Arrange
        var cliente = new Client
        {
            ClientID = Guid.NewGuid(),
            Name = "Juan Pérez",
            DniCuit = "20-12345678-9",
            Phone = "1123456789",
            IsActive = true
        };
        _mockService.Setup(s => s.CreateAsync(cliente)).ReturnsAsync(cliente);

        // Act
        var result = await _controller.CreateClient(cliente);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task CreateClient_Retorna400_CuandoNombreEsVacio()
    {
        // Arrange
        var cliente = new Client
        {
            ClientID = Guid.NewGuid(),
            Name = "",
            DniCuit = "20-12345678-9",
            Phone = "1123456789"
        };

        // Act
        var result = await _controller.CreateClient(cliente);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateClient_Retorna400_CuandoDniCuitEsVacio()
    {
        // Arrange
        var cliente = new Client
        {
            ClientID = Guid.NewGuid(),
            Name = "Juan Pérez",
            DniCuit = "",
            Phone = "1123456789"
        };

        // Act
        var result = await _controller.CreateClient(cliente);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteClient_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteClient(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();
        _controller = new AuthController(_mockService.Object);
    }

    [Fact]
    public async Task Register_Retorna200_CuandoDatosValidos()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@mail.com", Password = "123456" };
        _mockService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync("token_jwt_falso");

        // Act
        var result = await _controller.Register(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Register_Retorna400_CuandoEmailYaExiste()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@mail.com", Password = "123456" };
        _mockService.Setup(s => s.RegisterAsync(dto)).ReturnsAsync((string?)null);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_Retorna200_CuandoCredencialesValidas()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@mail.com", Password = "123456" };
        _mockService.Setup(s => s.LoginAsync(dto)).ReturnsAsync("token_jwt_falso");

        // Act
        var result = await _controller.Login(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Login_Retorna401_CuandoCredencialesInvalidas()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@mail.com", Password = "wrongpass" };
        _mockService.Setup(s => s.LoginAsync(dto)).ReturnsAsync((string?)null);

        // Act
        var result = await _controller.Login(dto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_Retorna401_CuandoUsuarioNoExiste()
    {
        // Arrange
        var dto = new LoginDto { Email = "noexiste@mail.com", Password = "123456" };
        _mockService.Setup(s => s.LoginAsync(dto)).ReturnsAsync((string?)null);

        // Act
        var result = await _controller.Login(dto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }
}

public class RentalsControllerTests
{
    private readonly Mock<IRentalService> _mockService;
    private readonly Mock<ILogger<RentalsController>> _mockLogger;
    private readonly RentalsController _controller;

    public RentalsControllerTests()
    {
        _mockService = new Mock<IRentalService>();
        _mockLogger = new Mock<ILogger<RentalsController>>();
        _controller = new RentalsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetRentals_Retorna200_CuandoHayAlquileres()
    {
        // Arrange
        var alquileres = new List<Rental>
        {
            new() { RentalID = Guid.NewGuid(), ClientID = Guid.NewGuid(), UserID = Guid.NewGuid(), StatusID = Guid.NewGuid() },
            new() { RentalID = Guid.NewGuid(), ClientID = Guid.NewGuid(), UserID = Guid.NewGuid(), StatusID = Guid.NewGuid() }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(alquileres);

        // Act
        var result = await _controller.GetRentals();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetRental_Retorna200_CuandoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var alquiler = new Rental { RentalID = id, ClientID = Guid.NewGuid(), UserID = Guid.NewGuid(), StatusID = Guid.NewGuid() };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(alquiler);

        // Act
        var result = await _controller.GetRental(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetRental_Retorna404_CuandoNoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Rental?)null);

        // Act
        var result = await _controller.GetRental(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task PostRental_Retorna201_CuandoDatosValidos()
    {
        // Arrange
        var alquiler = new Rental
        {
            RentalID = Guid.NewGuid(),
            ClientID = Guid.NewGuid(),
            UserID = Guid.NewGuid(),
            StatusID = Guid.NewGuid(),
            RentalDate = DateTime.UtcNow,
            RentalDateExpected = DateTime.UtcNow.AddDays(7)
        };
        _mockService.Setup(s => s.CreateAsync(alquiler)).ReturnsAsync(alquiler);

        // Act
        var result = await _controller.PostRental(alquiler);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }
}