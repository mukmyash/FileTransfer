using FluentAssertions;
using System;
using Xunit;

namespace CFT.FileProvider.SMB.Test
{
    public class PathStringUnitTests
    {
        [Theory()]
        [InlineData("smb://192.168.1.1", true, "smb://192.168.1.1/path/to/folder", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", true, "smb://192.168.1.2/path/to/folder", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", true, "/path/to/folder", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", true, "path/to/folder", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", false, "smb://192.168.1.1/path/to/folder", "path/to/folder")]
        [InlineData("smb://192.168.1.1", false, "smb://192.168.1.2/path/to/folder/", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", false, "/path/to/folder/", "path/to/folder/")]
        [InlineData("smb://192.168.1.1", false, "path/to/folder", "path/to/folder")]
        public void PathString_PrepareStringPath(string startWith, bool isDictionary, string path, string expectedPath)
        {
            PathString.PrepareStringPath(path, startWith, isDictionary)
                .Should()
                .NotBeNull()
                .And.Be(expectedPath);
        }
    }
}
