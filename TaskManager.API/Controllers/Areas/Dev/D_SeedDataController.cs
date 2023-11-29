using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Files.Services;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Static;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Infrastructure.Integrations.FileStorage.Configuration;

namespace TaskManager.API.Controllers.Areas.Dev;

[Route($"{Endpoints.BaseUrl}/seed")]
public class D_SeedDataController : BaseController
{
    private readonly RoleManager<Role> _roleManager;
    private readonly EFContext _context;
    private readonly IS3StorageService _s3StorageService;
    private readonly AmazonS3Client  _s3Client;
    private readonly S3Config  _s3Config;

    public D_SeedDataController(RoleManager<Role> roleManager, EFContext context, S3Config s3Config)
    {
        _roleManager = roleManager;
        _context = context;
        _s3Config = s3Config;
        var connectionConfig = new AmazonS3Config
        {
            ServiceURL = _s3Config.S3Url,
            ForcePathStyle = true
        };
        _s3Client = new AmazonS3Client(_s3Config.AccessKey, _s3Config.SecretKey, connectionConfig);
    }

    [HttpPut("roles")]
    public async Task<IActionResult> CreateTest(CancellationToken cancellationToken)
    {
        var roles = UserRoles.Get();
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                await _roleManager.CreateAsync(role);
            }
        }

        return Ok();
    }

    [HttpPut("move")]
    public async Task<IActionResult> MoveFileToBucket(CancellationToken cancellationToken)
    {
        var files = await _context.Files.ToListAsync(cancellationToken);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
   
        foreach (var file in files)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{file.Name}";
            using var memoryStream = new MemoryStream(file.Data);
            var putRequest = new PutObjectRequest
            {
                BucketName = _s3Config.BucketName,
                Key = uniqueFileName,
                InputStream = memoryStream
            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken);
            
            file.S3Key = uniqueFileName;
            _context.Update(file);
            await _context.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return Ok();
    }
}
