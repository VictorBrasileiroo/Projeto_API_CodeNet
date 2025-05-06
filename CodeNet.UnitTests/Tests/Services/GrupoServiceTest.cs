using System;
using System.Threading.Tasks;
using CodeNet.Application.Services.Grupo;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using Moq;
using Xunit;

namespace CodeNet.UnitTests.Tests.Services
{
    public class GrupoServiceTest
    {
        private readonly Mock<IGrupoMembroRepository> _mockGrupoMembroRepository;
        private readonly Mock<IGrupoRepository> _mockGrupoRepository;
        private readonly GrupoService _grupoService;

        public GrupoServiceTest()
        {
            _mockGrupoMembroRepository = new Mock<IGrupoMembroRepository>();
            _mockGrupoRepository = new Mock<IGrupoRepository>();
            _grupoService = new GrupoService(_mockGrupoMembroRepository.Object, _mockGrupoRepository.Object);
        }

        [Fact]
        public async Task ExcluirGrupo_GrupoExistenteEUsuarioAdmin_DeveExcluirGrupoComSucesso()
        {
            // Arrange
            var grupoId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var grupo = new GrupoModel { Id = grupoId };
            var membro = new GrupoMembroModel { IdUser = userId, Papel = "Admin" };

            _mockGrupoMembroRepository.Setup(r => r.GetMembro(userId, grupoId)).ReturnsAsync(membro);
            _mockGrupoRepository.Setup(r => r.GetById(grupoId)).ReturnsAsync(grupo);
            _mockGrupoRepository.Setup(r => r.DeleteGrupo(grupo)).ReturnsAsync(grupo);

            // Act
            var result = await _grupoService.ExcluirGrupo(grupoId, userId);

            // Assert
            Assert.Equal(grupoId, result.Id); 
            _mockGrupoRepository.Verify(r => r.DeleteGrupo(grupo), Times.Once);
        }
    }
}
