using System.IO.Compression;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.EnableForHttps = true;
    options.MimeTypes = ["text/html", "text/css", "text/xml", "image/gif", "image/jpeg", "application/x-javascript", "application/atom+xml", "application/rss+xml", "application/json", "text/json", "text/mathml", "text/plain", "text/vnd.sun.j2me.app-descriptor", "text/vnd.wap.wml", "text/x-component", "image/png", "image/tiff", "image/vnd.wap.wbmp", "image/x-icon", "image/x-jng", "image/x-ms-bmp", "image/svg+xml", "image/webp", "application/java-archive", "application/mac-binhex40", "application/msword", "application/pdf", "application/postscript", "application/rtf", "application/vnd.ms-excel", "application/vnd.ms-powerpoint", "application/vnd.wap.wmlc", "application/vnd.google-earth.kml+xml", "application/vnd.google-earth.kmz", "application/x-7z-compressed", "application/x-cocoa", "application/x-java-archive-diff", "application/x-java-jnlp-file", "application/x-makeself", "application/x-perl", "application/x-pilot", "application/x-rar-compressed", "application/x-redhat-package-manager", "application/x-sea", "application/x-shockwave-flash", "application/x-stuffit", "application/x-tcl", "application/x-x509-ca-cert", "application/x-xpinstall", "application/xhtml+xml", "application/zip", "application/octet-stream", "audio/midi", "audio/mpeg", "audio/ogg", "audio/x-realaudio", "audio/x-m4a", "video/3gpp", "video/mpeg", "video/quicktime", "video/x-flv", "video/x-mng", "video/x-ms-asf", "video/x-ms-wmv", "video/x-msvideo", "video/mp4", "video/webm", "video/x-m4v"];
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; });
builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; });
builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCompression();
app.UseResponseCaching();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=86400"); }
});

app.UseCors(corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
app.Run();