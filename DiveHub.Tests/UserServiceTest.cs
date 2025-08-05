using Moq;
using DiveHub.Infrastructure.repositories;
using DiveHub.Core.Entities;
using AutoMapper;
using DiveHub.Application.Services;
using DiveHub.Application.Dto;

namespace DiveHub.Application.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            var userDto = new UserDto { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john@example.com", result.Email);

        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 999;
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var createUserDto = new UserCreateDto { FirstName = "New", LastName ="User", Email = "newuser@example.com" };
            var user = new User { UserId = 1, FirstName = "New", LastName = "User", Email = "newuser@example.com" };
            var userDto = new UserDto { UserId = 1, FirstName = "New", LastName = "User", Email = "newuser@example.com" };

            _mockMapper.Setup(m => m.Map<User>(createUserDto)).Returns(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New", result.FirstName);
            Assert.Equal("User", result.LastName);
            Assert.Equal("newuser@example.com", result.Email);
            _mockUserRepository.Verify(r => r.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnUpdatedUser_WhenUserExists()
        {
            // Arrange
            var userId = 21;
            var updateUserDto = new UserUpdateDto { FirstName = "Updated", LastName = "User", Email = "updated@example.com" };
            var existingUser = new User { UserId = userId, FirstName = "Old", LastName = "User", Email = "old@example.com" };
            var updatedUser = new User { UserId = userId, FirstName = "Updated", LastName = "User", Email = "updated@example.com" };
            var userDto = new UserDto { UserId = userId, FirstName = "Updated", LastName = "User", Email = "updated@example.com" };

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockMapper.Setup(m => m.Map(updateUserDto, existingUser)).Returns(updatedUser);
            _mockUserRepository.Setup(r => r.UpdateAsync(updatedUser)).Returns(Task.FromResult(true));
            _mockMapper.Setup(m => m.Map<UserDto>(updatedUser)).Returns(userDto);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("User", result.FirstName);
            Assert.Equal("updated@example.com", result.Email);

            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 999;
            var updateUserDto = new UserUpdateDto { FirstName = "Updated", LastName = "User", Email = "updated@example.com" };
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, FirstName = "User to Delete", Email = "delete@example.com" };
            
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            // Remplacez cette ligne :
            // _mockUserRepository.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true);

            // Par celle-ci :
            _mockUserRepository.Setup(r => r.DeleteAsync(userId)).Returns(Task.FromResult(true));
            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
        }

    }
}
