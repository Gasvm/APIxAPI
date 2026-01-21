using PostsApi.DTOs;
using PostsApi.Models;
using PostsApi.Repositories;

namespace PostsApi.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PostService> _logger;

    public PostService(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ILogger<PostService> logger)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetAllPostsAsync();
        var users = await _userRepository.GetAllUsersAsync();
        var userDict = users.ToDictionary(u => u.Id);

        return posts.Select(p => MapToDto(p, userDict.GetValueOrDefault(p.UserId)));
    }

    public async Task<PostResponseDto?> GetPostByIdAsync(int id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        
        if (post == null)
            return null;

        var user = await _userRepository.GetUserByIdAsync(post.UserId);
        
        return MapToDto(post, user);
    }

    public async Task<IEnumerable<PostResponseDto>> GetPostsByUserAsync(int userId)
    {
        var posts = await _postRepository.GetPostsByUserIdAsync(userId);
        var user = await _userRepository.GetUserByIdAsync(userId);

        return posts.Select(p => MapToDto(p, user));
    }

    public async Task<UserSummaryDto?> GetUserSummaryAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
            return null;

        var posts = await _postRepository.GetPostsByUserIdAsync(userId);

        return new UserSummaryDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TotalPosts = posts.Count()
        };
    }

    public async Task<PostResponseDto> CreatePostAsync(string title, string content, int userId)
    {
        var post = new Post
        {
            Title = title,
            Body = content,
            UserId = userId
        };
        
        var createdPost = await _postRepository.CreatePostAsync(post);
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        return MapToDto(createdPost, user);
    }

    public async Task<PostResponseDto> UpdatePostAsync(int id, string title, string content)
    {
        var existingPost = await _postRepository.GetPostByIdAsync(id);
        if (existingPost == null)
            throw new InvalidOperationException($"Post {id} no encontrado");
        
        existingPost.Title = title;
        existingPost.Body = content;
        
        var updatedPost = await _postRepository.UpdatePostAsync(id, existingPost);
        var user = await _userRepository.GetUserByIdAsync(updatedPost.UserId);
        
        return MapToDto(updatedPost, user);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        return await _postRepository.DeletePostAsync(id);
    }

    private PostResponseDto MapToDto(Post post, User? user)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Body,
            AuthorName = user?.Name ?? "Desconocido",
            WordCount = post.Body.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length
        };
    }
}