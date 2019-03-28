using System;
using PictureLiker.DAL;
using Xunit;

namespace PictureLiker.Tests.DAL
{
    public class PictureTests
    {
        [Fact]
        public void SetBytesWithNullBytesShouldThrowArgumentNullException()
        {
            var picture = new Picture();

            Action action = () => picture.SetBytes(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetBytesWithValidBytesShouldSetCorrectly()
        {
            var picture = new Picture();
            var validBytes = new byte[10];
            new Random().NextBytes(validBytes);

            picture.SetBytes(validBytes);

            Assert.Equal(validBytes, picture.Bytes);
        }
    }
}